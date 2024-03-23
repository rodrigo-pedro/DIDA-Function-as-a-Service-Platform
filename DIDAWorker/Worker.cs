using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DIDAWorker
{
    class Worker : DIDAWorkerService.DIDAWorkerServiceBase
    {
        private int gossip_delay;
        private Dictionary<int, DIDAStorageNode> storages;
        private SortedDictionary<int, int> storageHashes;
        private DIDASchedulerService.DIDASchedulerServiceClient scheduler;
        private bool debug;

        // proxy

        public Worker(string server_id, int gossip_delay, bool debug)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            this.serverID = server_id;
            this.gossip_delay = gossip_delay;
            this.debug = debug;
            storages = new Dictionary<int, DIDAStorageNode>();
            storageHashes = new SortedDictionary<int, int>();
        }

        public string serverID { set; get; }



        public override Task<DIDAReqReply> DIDAProcessRequest(DIDAReqRequest request, ServerCallContext context)
        {
            return Task.FromResult(ProcessRequest(request));
        }


        public override Task<DIDARegisterStorageReply> DIDARegisterStorage(DIDARegisterStorageRequest request, ServerCallContext context)
        {

            return Task.FromResult(RegisterStorage(request));
        }

        public override Task<DIDARegisterSchedulerReply> DIDARegisterScheduler(DIDARegisterSchedulerRequest request, ServerCallContext context)
        {
            var channel = GrpcChannel.ForAddress(request.Url);
            scheduler = new DIDASchedulerService.DIDASchedulerServiceClient(channel);

            return Task.FromResult(new DIDARegisterSchedulerReply { Ok = true });
        }



        public DIDARegisterStorageReply RegisterStorage(DIDARegisterStorageRequest request)
        {
            //http://localhost:2222
            foreach (var entry in request.Storages)
            {
                var parsed = entry.Value.Remove(0, 7).Split(":");

                storages.Add(entry.Key, new DIDAStorageNode
                {
                    host = parsed[0],
                    port = int.Parse(parsed[1]),
                    serverId = entry.Key.ToString()
                });
            }

            foreach (var entry in request.StorageHashes)
            {
                storageHashes.Add(entry.Key, entry.Value);
            }

            return new DIDARegisterStorageReply { Ok = true };
        }

        public DIDAReqReply ProcessRequest(DIDAReqRequest request)
        {
            Console.WriteLine("Processing request...");

            ExtendedMetarecord metaRecord = ConvertGrpcMetaRecord(request.Meta);
            SortedDictionary<int, int> copyStorageHashes;
            DIDAStorageNode[] nodes;

            lock (this)
            {
                foreach (var hash in metaRecord.deadStoragesHashes)
                {
                    if (storageHashes.ContainsKey(hash))
                    {
                        storages.Remove(storageHashes[hash]);
                        storageHashes.Remove(hash);
                    }
                }

                copyStorageHashes = new SortedDictionary<int, int>(storageHashes);
                nodes = storages.Values.ToArray();
            }

            string className = request.Chain[request.Next].Op.Classname;
            IDIDAOperator op = LoadByRefletion(className);
            string prevOutput = request.Next > 0 ? request.Chain[request.Next - 1].Output : "";
            StorageProxy proxy = new StorageProxy(nodes, copyStorageHashes, metaRecord);
            op.ConfigureStorage(proxy);
            string output = "";
            try
            {
                output = op.ProcessRecord(metaRecord, request.Input, prevOutput);
                if (debug)
                    Console.WriteLine($"Output: {output}");

            }
            catch (Exception)
            {
                // reading a record that's too old causes the application chain do stop
                Console.WriteLine("Terminating application");
                lock (this)
                {
                    scheduler.DIDAAddFreeWorker(new DIDAAddFreeWorkerRequest { Id = serverID, Last = true });
                }
                return new DIDAReqReply { Ok = false };
            }

            DIDAMetaRecordMessage grpcMetaRecord = new DIDAMetaRecordMessage
            {
                Id = proxy.Meta.Id,
                VersionNumber = proxy.Meta.VersionNumber,
                ReplicaId = proxy.Meta.ReplicaId,
            };

            grpcMetaRecord.DeadStoragesHashes.Add(proxy.Meta.deadStoragesHashes);
            request.Chain[request.Next].Output = output;
            request.Next++;

            DIDAReqRequest newRequest = new DIDAReqRequest
            {
                Input = request.Input,
                ChainSize = request.ChainSize,
                Next = request.Next,
                Meta = grpcMetaRecord,
            };

            newRequest.Chain.Add(request.Chain);

            System.Threading.Thread.Sleep(gossip_delay);
            if (request.Next != request.ChainSize)
            {
                ConnectToNextWorker(newRequest);
            }

            lock (this)
            {
                if (request.Next == request.ChainSize)
                    scheduler.DIDAAddFreeWorkerAsync(new DIDAAddFreeWorkerRequest { Id = serverID, Last = true });
                else
                    scheduler.DIDAAddFreeWorkerAsync(new DIDAAddFreeWorkerRequest { Id = serverID, Last = false });
            }

            return new DIDAReqReply { Ok = true };
        }



        private void ConnectToNextWorker(DIDAReqRequest request)
        {
            GrpcChannel channel = GrpcChannel.ForAddress("http://" + request.Chain[request.Next].Host + ":" + request.Chain[request.Next].Port);
            DIDAWorkerService.DIDAWorkerServiceClient worker = new DIDAWorkerService.DIDAWorkerServiceClient(channel);

            worker.DIDAProcessRequestAsync(request);

        }



        private IDIDAOperator LoadByRefletion(String className)
        {


            //string currWorkingDir = AppDomain.CurrentDomain.BaseDirectory;

            string currentWorkingDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName;

            foreach (string filename in Directory.EnumerateFiles(currentWorkingDir + @"\PCS"))
            {
                if (filename.EndsWith(".dll"))
                {
                    Assembly dll = Assembly.LoadFrom(filename);
                    Type[] typeList = dll.GetTypes();
                    foreach (Type type in typeList)
                    {
                        if (type.Name == className)
                        {
                            return (IDIDAOperator)Activator.CreateInstance(type);
                        }
                    }
                }
            }
            return null;
        }


        //-------
        // CONVERSIONS FROM GRPC TO LIB DATA STRUCTURES AND VICE-VERSA 
        //-------

        private ExtendedMetarecord ConvertGrpcMetaRecord(DIDAMetaRecordMessage m)
        {
            ExtendedMetarecord result = new ExtendedMetarecord
            {
                Id = m.Id,
                VersionNumber = m.VersionNumber,
                ReplicaId = m.ReplicaId,
            };

            List<int> dsh = new List<int>();
            foreach (var deadStorage in m.DeadStoragesHashes)
            {
                dsh.Add(deadStorage);
            }

            result.deadStoragesHashes = dsh;
            return result;
        }
    }
}
