using DIDAStorageClient;
using StorageLocator;
using System;
using System.Collections.Generic;
using System.Linq;


namespace DIDAWorker
{
    class StorageProxy : IDIDAStorage
    {
        Dictionary<int, DIDAStorageService.DIDAStorageServiceClient> _clients = new Dictionary<int, DIDAStorageService.DIDAStorageServiceClient>();
        Dictionary<int, Grpc.Net.Client.GrpcChannel> _channels = new Dictionary<int, Grpc.Net.Client.GrpcChannel>();
        SortedDictionary<int, int> _storageHashes = new SortedDictionary<int, int>();

        // metarecord for the request that this storage proxy is handling
        public ExtendedMetarecord Meta
        {
            get; set; 
        }

        // The constructor of a storage proxy.
        // The storageNodes parameter lists the nodes that this storage proxy needs to be aware of to perform
        // read, write and updateIfValueIs operations.
        // The metaRecord identifies the request being processed by this storage proxy object
        // and allows the storage proxy to request data versions previously accessed by the request
        // and to inform operators running on the following (downstream) workers of the versions it accessed.
        public StorageProxy(DIDAStorageNode[] storageNodes, SortedDictionary<int, int> storageHashes, ExtendedMetarecord metaRecord)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            foreach (DIDAStorageNode n in storageNodes)
            {
                int serverdId = int.Parse(n.serverId);
                _channels[serverdId] = Grpc.Net.Client.GrpcChannel.ForAddress("http://" + n.host + ":" + n.port);
                _clients[serverdId] = new DIDAStorageService.DIDAStorageServiceClient(_channels[serverdId]);

            }
            _storageHashes = storageHashes;
            Meta = metaRecord;
        }


        public virtual DIDARecordReply read(DIDAReadRequest r)
        {

            List<int> cluster = Locator.LocateStorages(r.Id, _storageHashes);

            int versionNumber, replicaId;

            if (Meta.VersionNumber == -1)
            {
                versionNumber = r.Version.VersionNumber;
                replicaId = r.Version.ReplicaId;
            }
            else
            {
                versionNumber = Meta.VersionNumber;
                replicaId = Meta.ReplicaId;
            }


            // Randomly access one replica to perform the read
            while (cluster.Count > 0)
            {
                int randomIndex = new Random().Next(0, cluster.Count);

                try
                {
                    
                    var result = _clients[cluster[randomIndex]].read(new DIDAStorageClient.DIDAReadRequest { Id = r.Id, Version = new DIDAStorageClient.DIDAVersion { VersionNumber = versionNumber, ReplicaId = replicaId } });

                    if (Meta.VersionNumber == -1)
                    {
                        Meta.VersionNumber = result.Version.VersionNumber;
                        Meta.ReplicaId = result.Version.ReplicaId;
                    }

                    if (result.Version.VersionNumber == -1)
                    {
                        cluster.RemoveAt(randomIndex);
                        continue;
                    }

                   
                    return new DIDARecordReply { Id = result.Id, Val = result.Val, Version = { VersionNumber = result.Version.VersionNumber, ReplicaId = result.Version.ReplicaId } };
                }
                catch (Exception)
                {
                    Console.WriteLine("Server with id: " + cluster[randomIndex] + " !failed, contacting next server");
                    Meta.deadStoragesHashes.Add(_storageHashes.FirstOrDefault(x => x.Value == cluster[randomIndex]).Key);
                    cluster.RemoveAt(randomIndex);
                }

            }

            throw new Exception("Error: Couldn't find version");

        }


        public virtual DIDAVersion write(DIDAWriteRequest r)
        {

            List<int> cluster = Locator.LocateStorages(r.Id, _storageHashes);

            while (cluster.Count > 0)
            {
                int randomIndex = new Random().Next(0, cluster.Count);

                try
                {
                    var result = _clients[cluster[randomIndex]].write(new DIDAStorageClient.DIDAWriteRequest { Id = r.Id, Val = r.Val });
                    Meta.VersionNumber = result.VersionNumber;
                    Meta.ReplicaId = result.ReplicaId;
                    return new DIDAVersion { VersionNumber = result.VersionNumber, ReplicaId = result.ReplicaId };
                }
                catch (Exception)
                {
                    Console.WriteLine("Server with id: " + cluster[randomIndex] + " ?failed, contacting next server");
                    Meta.deadStoragesHashes.Add(_storageHashes.FirstOrDefault(x => x.Value == cluster[randomIndex]).Key);
                    cluster.RemoveAt(randomIndex);
                }
            }

            throw new Exception("Error: No servers to write to.");
        }

        public virtual DIDAVersion updateIfValueIs(DIDAUpdateIfRequest r)
        {

            List<int> cluster = Locator.LocateStorages(r.Id, _storageHashes);

            foreach (int id in cluster)
            {
                try
                {
                    var res = _clients[id].updateIfValueIs(new DIDAStorageClient.DIDAUpdateIfRequest { Id = r.Id, Newvalue = r.Newvalue, Oldvalue = r.Oldvalue });
                    return new DIDAVersion { VersionNumber = res.VersionNumber, ReplicaId = res.ReplicaId };

                }
                catch (Exception)
                {
                    Console.WriteLine("Server with id: " + id + " failed, contacting next server");
                    Meta.deadStoragesHashes.Add(_storageHashes.FirstOrDefault(x => x.Value == id).Key);
                    cluster.Remove(id);
                }

            }
            throw new Exception("Error: no servers found");
        }
    }
}
