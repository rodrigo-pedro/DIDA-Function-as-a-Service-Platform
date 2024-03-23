using DIDAWorker;
using Grpc.Core;
using Grpc.Net.Client;
using StorageLocator;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace DIDAStorage
{
    public class StorageServer : DIDAStorageService.DIDAStorageServiceBase, IDIDAStorage
    {
        private ConcurrentDictionary<string, File> files = new ConcurrentDictionary<string, File>();
        private SortedDictionary<int, int> storageHashes = new SortedDictionary<int, int>();
        private Dictionary<int, string> storageUrls = new Dictionary<int, string>();
        private Queue<string> filesToGossip = new Queue<string>();

        private int serverId;
        private string name;

        private const int gossipInterval = 2000;

        System.Timers.Timer gossipTimer;


        public StorageServer(int serverId, string name, int gossipDelay)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            this.serverId = serverId;
            this.name = name;

            gossipTimer = new System.Timers.Timer(gossipInterval + gossipDelay);
            gossipTimer.Elapsed += OnTimedEvent;
            gossipTimer.AutoReset = true;
            gossipTimer.Enabled = true;
        }

        public override Task<DIDARecordReply> read(DIDAReadRequest request, ServerCallContext context)
        {
            DIDAWorker.DIDARecordReply record = ConvertRecordToReply(GetNullRecord());

            if (!files.ContainsKey(request.Id))
            {
                record = ConvertRecordToReply(GetNullRecord());
            }

            else
            {
                lock (files[request.Id])
                {
                    record = read(new DIDAWorker.DIDAReadRequest
                    {
                        Id = request.Id,
                        Version = new DIDAWorker.DIDAVersion { ReplicaId = request.Version.ReplicaId, VersionNumber = request.Version.VersionNumber }
                    });
                }
            }
            return Task.FromResult(ConvertRecordReplyToGrpc(record));
        }

        public DIDAWorker.DIDARecordReply read(DIDAWorker.DIDAReadRequest request)
        {

            DIDARecord record = files[request.Id].GetRecord(request.Version);
            return ConvertRecordToReply(record);
        }


        public override Task<DIDAVersion> write(DIDAWriteRequest request, ServerCallContext context)
        {
            DIDAWorker.DIDAVersion version = write(new DIDAWorker.DIDAWriteRequest { Id = request.Id, Val = request.Val });

            return Task.FromResult(new DIDAVersion
            {
                ReplicaId = version.ReplicaId,
                VersionNumber = version.VersionNumber
            });
        }


        public DIDAWorker.DIDAVersion write(DIDAWorker.DIDAWriteRequest request)
        {
            DIDAWorker.DIDAVersion version;

            if (!files.ContainsKey(request.Id))
            {
                files.TryAdd(request.Id, new File());
            }

            lock (files[request.Id])
            {
                while (files[request.Id].isInStandBy)
                {
                    Monitor.Wait(files[request.Id]);
                }

                version = files[request.Id].WriteRecord(request.Val, serverId, request.Id);

                if (!filesToGossip.Contains(request.Id))
                    filesToGossip.Enqueue(request.Id);
            }

            return version;
        }

        public override Task<DIDAVersion> updateIfValueIs(DIDAUpdateIfRequest request, ServerCallContext context)
        {
            DIDAWorker.DIDAVersion version = new DIDAWorker.DIDAVersion { ReplicaId = -1, VersionNumber = -1 };


            List<int> cluster = Locator.LocateStorages(request.Id, storageHashes);
            cluster.Remove(serverId);

            files.TryAdd(request.Id, new File());
            lock (files[request.Id])
            {
                while (files[request.Id].isInStandBy)
                {
                    Monitor.Wait(files[request.Id]);
                }

                files[request.Id].isInStandBy = true;




                foreach (int server in cluster)
                {
                    var channel = GrpcChannel.ForAddress(storageUrls[server]);
                    var storage = new DIDAStorageService.DIDAStorageServiceClient(channel);
                    try
                    {

                        var reply = storage.prepareStorages(new DIDAPrepareRequest { Id = request.Id });
                        if (reply.Version.VersionNumber != -1)
                        {
                            if (!files.ContainsKey(request.Id))
                            {
                                files.TryAdd(request.Id, new File());
                            }
                            files[request.Id].AddRecord(ConvertGrpcToRecord(reply));
                        }
                    }
                    catch (Exception)
                    {
                        storageHashes.Remove(storageHashes.FirstOrDefault(x => x.Value == server).Key);
                        storageUrls.Remove(server);
                    }
                }

                version = updateIfValueIs(new DIDAWorker.DIDAUpdateIfRequest { Id = request.Id, Newvalue = request.Newvalue, Oldvalue = request.Oldvalue });


                foreach (int server in cluster)
                {
                    var channel = GrpcChannel.ForAddress(storageUrls[server]);
                    var storage = new DIDAStorageService.DIDAStorageServiceClient(channel);


                    storage.continueStorages(new DIDAContinueRequest { Id = request.Id });

                }

                files[request.Id].isInStandBy = false;
                Monitor.PulseAll(files[request.Id]);
            }




            return Task.FromResult(new DIDAVersion
            {
                ReplicaId = version.ReplicaId,
                VersionNumber = version.VersionNumber
            });
        }


        public DIDAWorker.DIDAVersion updateIfValueIs(DIDAWorker.DIDAUpdateIfRequest request)
        {
            string id = request.Id, oldvalue = request.Oldvalue, newvalue = request.Newvalue;

            DIDAWorker.DIDARecordReply old_rec = read(new DIDAWorker.DIDAReadRequest { Id = id, Version = new DIDAWorker.DIDAVersion { ReplicaId = -1, VersionNumber = -1 } });

            if (old_rec.Val == oldvalue)
            {
                DIDAWorker.DIDAVersion version = files[id].WriteRecord(newvalue, serverId, id);
                if (!filesToGossip.Contains(id))
                    filesToGossip.Enqueue(id);
                return version;
            }
            else
            {
                return new DIDAWorker.DIDAVersion { ReplicaId = -1, VersionNumber = -1 };
            }

        }

        public DIDAListServerReply ListServer()
        {
            List<DIDARecordReply> output = new List<DIDARecordReply>();

            //FIXME possibly locking the dictionary lock(files)
            foreach (var entry in files)
            {
                foreach (var record in entry.Value.records)
                {
                    if (record.Version.VersionNumber != -1)
                        output.Add(new DIDARecordReply
                        {
                            Id = record.Id,
                            Val = record.Val,
                            Version = new global::DIDAVersion
                            {
                                ReplicaId = record.Version.ReplicaId,
                                VersionNumber = record.Version.VersionNumber
                            }
                        });
                }
            }

            DIDAListServerReply reply = new DIDAListServerReply();
            reply.Objects.Add(output);

            return reply;
        }


        public DIDARecordReply Prepare(DIDAPrepareRequest request)
        {
            DIDARecord record = GetNullRecord();

            if (files.ContainsKey(request.Id))
            {


                lock (files[request.Id])
                {
                    while (files[request.Id].isInStandBy)
                    {
                        Monitor.Wait(files[request.Id]);
                    }
                    files[request.Id].isInStandBy = true;


                    record = new DIDARecord
                    {
                        Id = request.Id,
                        Val = files[request.Id].records.First.Value.Val,
                        Version = new DIDAWorker.DIDAVersion { ReplicaId = files[request.Id].records.First.Value.Version.ReplicaId, VersionNumber = files[request.Id].records.First.Value.Version.VersionNumber }
                    };

                }

            }
            else
            {

                files.TryAdd(request.Id, new File());

                lock (files[request.Id])
                {

                    files[request.Id].isInStandBy = true;
                }
            }




            return new DIDARecordReply
            {
                Id = record.Id,
                Val = record.Val,
                Version = new DIDAVersion { ReplicaId = record.Version.ReplicaId, VersionNumber = record.Version.VersionNumber }
            };
        }

        public DIDAContinueReply Continue(DIDAContinueRequest request)
        {
            lock (files[request.Id])
            {
                files[request.Id].isInStandBy = false;
                Monitor.PulseAll(files[request.Id]);
            }

            return new DIDAContinueReply { Ok = true };
        }


        public DIDARegisterStoragesReply RegisterStorages(DIDARegisterStoragesRequest request)
        {
            foreach (var entry in request.Hashes)
            {
                storageHashes.Add(entry.Key, entry.Value);
            }

            foreach (var entry in request.Urls)
            {
                storageUrls.Add(entry.Key, entry.Value);
            }

            return new DIDARegisterStoragesReply { Ok = true };

        }


        //FIXME quando e que ja n e para dar gossip
        public DIDAGossipReply ProcessGossip(DIDAGossipRequest request)
        {
            DIDAGossipReply reply = new DIDAGossipReply();

            if (!files.ContainsKey(request.FileName))
                files.TryAdd(request.FileName, new File());

            lock (files[request.FileName])
            {


                foreach (var rec in request.Records)
                {


                    var newRec = new DIDARecord
                    {
                        Id = rec.Id,
                        Val = rec.Val,
                        Version = new DIDAWorker.DIDAVersion { ReplicaId = rec.Version.ReplicaId, VersionNumber = rec.Version.VersionNumber }
                    };



                    bool result = files[rec.Id].AddRecord(newRec);

                    if (result && !filesToGossip.Contains(rec.Id))
                    {
                        filesToGossip.Enqueue(rec.Id);
                    }

                }
                foreach (var i in files[request.FileName].records)
                {
                    foreach (var j in request.Records)
                    {
                        if (i.Version.VersionNumber == j.Version.VersionNumber &&
                            i.Version.ReplicaId == j.Version.ReplicaId)
                            break;
                    }
                    reply.Records.Add(ConvertRecordReplyToGrpc(ConvertRecordToReply(i)));
                }
            }

            return reply;
        }


        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            string fileName = filesToGossip.Dequeue();

            List<int> cluster = Locator.LocateStorages(fileName, storageHashes);

            cluster.Remove(serverId);

            Random rnd = new Random();
            int replicaToGossip = cluster[rnd.Next(cluster.Count)];

            var channel = GrpcChannel.ForAddress(storageUrls[replicaToGossip]);
            var storage = new DIDAStorageService.DIDAStorageServiceClient(channel);


            DIDAGossipRequest req = new DIDAGossipRequest
            {
                FileName = fileName
            };


            foreach (var record in files[fileName].records)
            {
                req.Records.Add(ConvertRecordReplyToGrpc(ConvertRecordToReply(record)));
            }

            DIDAGossipReply reply = new DIDAGossipReply();
            try
            {
                reply = storage.processGossip(req);
            }
            catch (Exception)
            {
                storageHashes.Remove(storageHashes.FirstOrDefault(x => x.Value == replicaToGossip).Key);
                storageUrls.Remove(replicaToGossip);
            }


            foreach (var record in reply.Records)
            {
                files[fileName].AddRecord(ConvertGrpcToRecord(record));
            }

            filesToGossip.Enqueue(fileName);

        }

        private DIDAStatusReply Status()
        {
            var strOut = $"Node: {name} with Status: ACTIVE";
            Console.WriteLine(strOut);

            return new DIDAStatusReply { Status = strOut };
        }


        public override Task<DIDAStatusReply> status(DIDAStatusRequest request, ServerCallContext context)
        {
            return Task.FromResult(Status());
        }

        public override Task<DIDAListServerReply> listServer(DIDAListServerRequest request, ServerCallContext context)
        {
            return Task.FromResult(ListServer());
        }

        public override Task<DIDARecordReply> prepareStorages(DIDAPrepareRequest request, ServerCallContext context)
        {
            return Task.FromResult(Prepare(request));
        }

        public override Task<DIDAContinueReply> continueStorages(DIDAContinueRequest request, ServerCallContext context)
        {
            return Task.FromResult(Continue(request));
        }

        public override Task<DIDARegisterStoragesReply> registerStorages(DIDARegisterStoragesRequest request, ServerCallContext context)
        {
            return Task.FromResult(RegisterStorages(request));
        }

        public override Task<DIDAGossipReply> processGossip(DIDAGossipRequest request, ServerCallContext context)
        {
            return Task.FromResult(ProcessGossip(request));
        }


        private DIDAWorker.DIDARecordReply ConvertRecordToReply(DIDARecord record) =>
            new DIDAWorker.DIDARecordReply
            {
                Id = record.Id,
                Val = record.Val,
                Version = record.Version
            };

        private DIDARecord ConvertGrpcToRecord(DIDARecordReply recordReply) =>
            new DIDARecord
            {
                Id = recordReply.Id,
                Val = recordReply.Val,
                Version = new DIDAWorker.DIDAVersion
                {
                    VersionNumber = recordReply.Version.VersionNumber,
                    ReplicaId = recordReply.Version.ReplicaId
                }
            };

        // creates a null record
        private DIDARecord GetNullRecord() =>
            new DIDARecord
            {
                Id = "",
                Val = "",
                Version = new DIDAWorker.DIDAVersion
                {
                    ReplicaId = -1,
                    VersionNumber = -1
                }
            };

        private DIDARecordReply ConvertRecordReplyToGrpc(DIDAWorker.DIDARecordReply record) =>
            new DIDARecordReply
            {
                Id = record.Id,
                Val = record.Val,
                Version = new DIDAVersion
                {
                    ReplicaId = record.Version.ReplicaId,
                    VersionNumber = record.Version.VersionNumber
                }
            };
    }


    public class Storage
    {
        static void Main(string[] args)
        {
            string urlParsed = args[2].Remove(0, 7); //remover http://
            Console.WriteLine("Storage node starting at " + args[2] + " with name " + args[1] + " and id " + int.Parse(args[0]));
            Server server = new Server()
            {
                Services = { DIDAStorageService.BindService(new StorageServer(int.Parse(args[0]), args[1], int.Parse(args[3]))) },
                // assuming the url starts with http://
                Ports = { new ServerPort(urlParsed.Split(":")[0],
                    int.Parse(urlParsed.Split(":")[1]), ServerCredentials.Insecure) }
            };

            server.Start();

            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
    }

    public class File
    {
        public LinkedList<DIDARecord> records { get; }

        public bool isInStandBy
        {
            get; set;
        }

        public const int maxVersions = 5;

        public File()
        {
            records = new LinkedList<DIDARecord>();
            records.AddFirst(new DIDARecord
            {
                Id = "",
                Val = "",
                Version = new DIDAWorker.DIDAVersion
                {
                    ReplicaId = -1,
                    VersionNumber = -1
                }
            });
            isInStandBy = false;
        }

        private void RemoveOldestRecord()
        {
            if (records.Count > maxVersions)
                records.RemoveFirst();
        }

        //write
        public DIDAWorker.DIDAVersion WriteRecord(string val, int replicaId, string id)
        {
            DIDAWorker.DIDAVersion version;

            if (records.First.Value.Version.VersionNumber == -1)
                records.RemoveFirst();

            if (records.Count == 0)
            {
                version = new DIDAWorker.DIDAVersion
                {
                    VersionNumber = 1,
                    ReplicaId = replicaId
                };
            }
            else
            {
                version = records.Last.Value.Version;
                version.VersionNumber++;
            }

            records.AddLast(new DIDARecord
            {
                Id = id,
                Val = val,
                Version = version
            });

            RemoveOldestRecord();

            return version;
        }


        // gossip
        // retorna true se acrescentou novo record, false se ja tem record igual
        public bool AddRecord(DIDARecord recToAdd)
        {
            if (records.First.Value.Version.VersionNumber == -1)
                records.RemoveFirst();

            foreach (var record in records)
            {
                if (recToAdd.Version.VersionNumber == record.Version.VersionNumber)
                {

                    if (recToAdd.Version.ReplicaId > record.Version.ReplicaId)
                    {
                        records.AddBefore(records.Find(record), recToAdd);
                        records.Remove(record);
                        return true;
                    }
                    return false;
                }

                if (recToAdd.Version.VersionNumber < record.Version.VersionNumber)
                {
                    records.AddBefore(records.Find(record), recToAdd);
                    RemoveOldestRecord();
                    return true;
                }
            }
            records.AddLast(recToAdd);
            RemoveOldestRecord();
            return true;
        }

        public DIDARecord GetRecord(DIDAWorker.DIDAVersion version)
        {
            if (version.VersionNumber == -1)
                return records.Last.Value;

            foreach (var record in records)
            {
                if (record.Version.VersionNumber == version.VersionNumber)
                    return record;
            }

            return new DIDARecord
            {
                Id = "",
                Val = "",
                Version = new DIDAWorker.DIDAVersion
                {
                    VersionNumber = -1,
                    ReplicaId = -1
                }
            };
        }
    }
}


