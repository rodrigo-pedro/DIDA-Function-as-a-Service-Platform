using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulerDIDA
{

    class Scheduler : DIDASchedulerService.DIDASchedulerServiceBase
    {
        private Dictionary<string, string> _workerUrls = new Dictionary<string, string>();
        private ConcurrentQueue<string> _occupiedWorkers = new ConcurrentQueue<string>();
        private ConcurrentQueue<string> _freeWorkers = new ConcurrentQueue<string>();

        public string ServerID { set; get; }
        private int _numMetaRecords = 0;


        public Scheduler(string server_id)
        {
            this.ServerID = server_id;
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
        }


        public DIDARegisterWorkersReply Register(DIDARegisterWorkersRequest request)
        {

            Console.WriteLine("Registering workers...");

            foreach (var entry in request.Workers)
            {
                _workerUrls.Add(entry.Key, entry.Value);
                _freeWorkers.Enqueue(entry.Key);
            }

            return new DIDARegisterWorkersReply { Ok = true };
        }


        public DIDARunReply Run(DIDARunRequest request)
        {
            Console.WriteLine("Starting chain...");
            List<DIDAAssignmentMessage> assignments = new List<DIDAAssignmentMessage>();

            lock (this)
            {
                string firstInLine = "";
                _ = _freeWorkers.Count != 0 ? _freeWorkers.TryPeek(out firstInLine) : _occupiedWorkers.TryPeek(out firstInLine);

                string[] operators = File.ReadAllLines(request.AppFile);


                string[] orderedOperators = new string[operators.Length];

                foreach (string s in operators)
                {
                    string[] split = s.Split(" ");
                    orderedOperators[int.Parse(split[2])] = s;
                }

                foreach (string s in orderedOperators)
                {
                    string[] split = s.Split(" ");

                    string id = "";
                    _ = _freeWorkers.Count != 0 ? _freeWorkers.TryDequeue(out id) : _occupiedWorkers.TryDequeue(out id);

                    //string id = _workersToWork.Dequeue();
                    string url = _workerUrls[id];

                    string parsedUrl = url.Remove(0, 7); //remover http://


                    DIDAOperatorIDMessage oper = new DIDAOperatorIDMessage
                    {
                        Classname = split[1],
                        Order = int.Parse(split[2])
                    };
                    DIDAAssignmentMessage ass = new DIDAAssignmentMessage
                    {
                        Op = oper,
                        Host = parsedUrl.Split(":")[0],
                        Port = int.Parse(parsedUrl.Split(":")[1])
                    };

                    assignments.Add(ass);
                    _occupiedWorkers.Enqueue(id);
                }

                DIDAMetaRecordMessage metaRecord = new DIDAMetaRecordMessage
                {
                    Id = _numMetaRecords,
                    VersionNumber = -1,
                    ReplicaId = -1
                };
                _numMetaRecords++;

                DIDAReqRequest req = new DIDAReqRequest
                {
                    Input = request.Input,
                    ChainSize = operators.Length,
                    Next = 0,
                    Meta = metaRecord
                };
                req.Chain.Add(assignments);

                GrpcChannel channel = GrpcChannel.ForAddress(_workerUrls[firstInLine]);
                DIDAWorkerService.DIDAWorkerServiceClient worker = new DIDAWorkerService.DIDAWorkerServiceClient(channel);

                worker.DIDAProcessRequestAsync(req);
            }

            return new DIDARunReply { Ok = true };
        }

        public DIDAAddFreeWorkerReply AddFreeWorker(DIDAAddFreeWorkerRequest req)
        {
            if (req.Last)
            {
                Console.WriteLine("Finished chain");
            }

            lock (this)
            {
                _freeWorkers.Enqueue(req.Id);
                _occupiedWorkers = new ConcurrentQueue<string>(_occupiedWorkers.Where(x => x != req.Id));
            }

            return new DIDAAddFreeWorkerReply { Ok = true };
        }


        public override Task<DIDARegisterWorkersReply> DIDARegisterWorkers(DIDARegisterWorkersRequest request, ServerCallContext context) =>
            Task.FromResult(Register(request));


        public override Task<DIDARunReply> DIDARun(DIDARunRequest request, ServerCallContext context) =>
            Task.FromResult(Run(request));

        public override Task<DIDAAddFreeWorkerReply> DIDAAddFreeWorker(DIDAAddFreeWorkerRequest request, ServerCallContext context) =>
            Task.FromResult(AddFreeWorker(request));

    }


    public class SchedulerServer
    {
        static void Main(string[] args)
        {

            string urlParsed = args[1].Remove(0, 7); //remover http://
            Server server = new Server()
            {

                Services = { DIDASchedulerService.BindService(new Scheduler(args[0])) },
                // assuming the url starts with http://
                Ports = { new ServerPort(
                    urlParsed.Split(":")[0],
                    int.Parse(urlParsed.Split(":")[1]), ServerCredentials.Insecure) }
            };

            server.Start();

            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }

    }
}
