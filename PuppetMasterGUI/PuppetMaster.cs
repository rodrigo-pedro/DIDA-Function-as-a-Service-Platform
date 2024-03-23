using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using StorageLocator;

namespace PuppetMasterGUI
{
    public class PuppetMaster
    {
        DIDASchedulerService.DIDASchedulerServiceClient scheduler;
        Dictionary<int, string> storageUrls;
        Dictionary<string, int> storageNames;
        SortedDictionary<int, int> storageHashes;
        Dictionary<string, string> workerUrls;
        private int storageCounter = 0;
        private bool debug = false;

        public PuppetMaster()
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            storageUrls = new Dictionary<int, string>();
            storageNames = new Dictionary<string, int>();
            storageHashes = new SortedDictionary<int, int>();
            workerUrls = new Dictionary<string, string>();
        }

        public PCSService.PCSServiceClient CreatePCSClient(string url)
        {
            string host = url.Split(":")[0] + ":" + url.Split(":")[1];

            GrpcChannel channel = GrpcChannel.ForAddress(host + ":10000");
            return new PCSService.PCSServiceClient(channel);
        }

        public void CreateScheduler(string serverId, string url)
        {

            CreateNodeRequest request = new CreateNodeRequest { ServerId = -1, Url = url, GossipDelay = -1, Name = serverId };

            var pcs = CreatePCSClient(url);
            pcs.CreateScheduler(request);

            GrpcChannel schedulerChannel = GrpcChannel.ForAddress(url);
            scheduler = new DIDASchedulerService.DIDASchedulerServiceClient(schedulerChannel);

            DIDARegisterWorkersRequest req = new DIDARegisterWorkersRequest();
            req.Workers.Add(workerUrls);
            scheduler.DIDARegisterWorkers(req);

            foreach (var workerUrl in workerUrls.Values)
            {
                GrpcChannel workerChannel = GrpcChannel.ForAddress(workerUrl);
                var worker = new DIDAWorkerService.DIDAWorkerServiceClient(workerChannel);
                worker.DIDARegisterScheduler(new DIDARegisterSchedulerRequest { Url = url });
            }

        }

        // assumindo que ja existe um scheduler
        public void CreateWorker(string serverId, string url, string gossipDelay)
        {

            CreateNodeRequest request = new CreateNodeRequest { ServerId = -1, Url = url, GossipDelay = int.Parse(gossipDelay), Name = serverId, Debug = debug };

            var pcs = CreatePCSClient(url);
            pcs.CreateWorker(request);


            workerUrls.Add(serverId, url);


            DIDARegisterStorageRequest storageRegisterRequest = new DIDARegisterStorageRequest();
            storageRegisterRequest.Storages.Add(storageUrls);
            storageRegisterRequest.StorageHashes.Add(storageHashes);

            GrpcChannel workerChannel = GrpcChannel.ForAddress(url);
            var worker = new DIDAWorkerService.DIDAWorkerServiceClient(workerChannel);
            worker.DIDARegisterStorage(storageRegisterRequest);
        }

        public void CreateStorage(string serverId, string url, string gossipDelay)
        {
            storageCounter++;
            CreateNodeRequest request = new CreateNodeRequest { ServerId = storageCounter, Name = serverId, Url = url, GossipDelay = int.Parse(gossipDelay) };
            var pcs = CreatePCSClient(url);
            pcs.CreateStorage(request);

            storageUrls.Add(request.ServerId, url);
            storageNames.Add(serverId, request.ServerId);

            byte[] encoded = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(request.Name));
            int hashedStorage = BitConverter.ToInt32(encoded, 0);
            storageHashes.Add(hashedStorage, request.ServerId);

        }

        public void RunApplication(string pathToFile, string input)
        {

            //mandamos ja o path completo
            DIDARunRequest request = new DIDARunRequest
            {
                AppFile = pathToFile,
                Input = input
            };

            scheduler.DIDARunAsync(request);
        }



        public void PopulateDatabase(string pathToFile)
        {
            string[] populateLines = File.ReadAllLines(pathToFile);

            foreach (string line in populateLines)
            {
                string[] splitLine = line.Split(",");
                List<int> cluster = Locator.LocateStorages(splitLine[0], storageHashes);

                foreach (int clusterId in cluster)
                {
                    GrpcChannel storageChannel = GrpcChannel.ForAddress(storageUrls[clusterId]);
                    var storage = new DIDAStorageService.DIDAStorageServiceClient(storageChannel);
                    try
                    {
                        storage.write(new DIDAWriteRequest
                        {
                            Id = splitLine[0],
                            Val = splitLine[1]
                        });

                    } catch (Exception)
                    {
                        DeleteDeadStorage(clusterId);
                    }
                }
            }

        }

        public string ListServer(string serverId)
        {
            GrpcChannel storageChannel = GrpcChannel.ForAddress(storageUrls[storageNames[serverId]]);
            var storage = new DIDAStorageService.DIDAStorageServiceClient(storageChannel);
            DIDAListServerReply reply = new DIDAListServerReply();
            string strOut = "";
            try
            {
                reply = storage.listServer(new DIDAListServerRequest());
            } catch (Exception)
            {
                DeleteDeadStorage(storageNames[serverId]);
                strOut = "Storage " + serverId + " is down\r\n";
                Console.WriteLine(strOut);
                return "";
            }

            strOut += $"Storage {serverId}:\r\n";

            foreach (var obj in reply.Objects)
            {
                strOut +=
                    $"Object: {obj.Id}, Val: {obj.Val}, ReplicaId: {obj.Version.ReplicaId}, VersionNumber: {obj.Version.VersionNumber}\r\n";
            }

            Console.WriteLine(strOut);

            return strOut;

        }

        public string ListGlobal()
        {
            string strOut = "";
            foreach (var entry in storageNames)
            {
                strOut += ListServer(entry.Key);
            }

            return strOut;
        }

        public void RegisterStorages()
        {
            DIDARegisterStoragesRequest request = new DIDARegisterStoragesRequest();
            request.Urls.Add(storageUrls);
            request.Hashes.Add(storageHashes);
            foreach (string url in storageUrls.Values)
            {
                GrpcChannel channel = GrpcChannel.ForAddress(url);
                DIDAStorageService.DIDAStorageServiceClient storageClient = new DIDAStorageService.DIDAStorageServiceClient(channel);
                storageClient.registerStoragesAsync(request);
            }
        }

        public void KillNode(string name)
        {
            var pcs = CreatePCSClient(storageUrls[storageNames[name]]);
            pcs.KillNodeAsync(new KillNodeRequest { Name = name });
        }

        public void RunScript(string pathToFile)
        {
            string[] operators = File.ReadAllLines(pathToFile);

            foreach (string s in operators)
            {
                RunLine(s);
            }
        }

        public void DeleteDeadStorage(int serverID)
        {
            storageUrls.Remove(serverID);
            storageHashes.Remove(storageHashes.FirstOrDefault(x => x.Value == serverID).Key);
            storageNames.Remove(storageNames.FirstOrDefault(x => x.Value == serverID).Key);
        }

        public string Status()
        {
            string outStr = "";
            foreach (var entry in storageUrls)
            {
                GrpcChannel channel = GrpcChannel.ForAddress(entry.Value);
                DIDAStorageService.DIDAStorageServiceClient storageClient = new DIDAStorageService.DIDAStorageServiceClient(channel);

                try
                {
                    outStr += storageClient.status(new DIDAStatusRequest()).Status + "\r\n";

                }
                catch (Exception)
                {
                    outStr += $"Node: { storageNames.FirstOrDefault(x => x.Value == entry.Key).Key } with Status: DOWN\r\n";

                }

            }


            // TODO: resto dos nos

            Console.WriteLine(outStr);
            return outStr;
        }

        public void RunLine(string line)
        {
            string[] split = line.Split(" ");

            switch (split[0])
            {
                case "scheduler":
                    CreateScheduler(split[1], split[2]);
                    RegisterStorages();  //register storages to other storages
                    break;
                case "storage":
                    CreateStorage(split[1], split[2], split[3]);
                    break;
                case "worker":
                    CreateWorker(split[1], split[2], split[3]);
                    break;
                case "populate":
                    PopulateDatabase(split[1]);
                    break;
                case "client":
                    RunApplication(split[2], split[1]);
                    break;
                case "status":
                    Status();
                    break;
                case "listServer":
                    ListServer(split[1]);
                    break;
                case "listGlobal":
                    ListGlobal();
                    break;
                case "debug":
                    debug = true;
                    break;
                case "crash":
                    KillNode(split[1]);
                    break;
                case "wait":
                    System.Threading.Thread.Sleep(int.Parse(split[1]));
                    break;
            }
        }
    }
}

