using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace PCS
{
    class PCS : PCSService.PCSServiceBase
    {
        private Dictionary<string, Process> runningProcs = new Dictionary<string, Process>();

        public override Task<CreateNodeReply> CreateWorker(CreateNodeRequest request, ServerCallContext context) => Task.FromResult(CreateNewWorker(request));

        public override Task<CreateNodeReply> CreateScheduler(CreateNodeRequest request, ServerCallContext context) => Task.FromResult(CreateNewScheduler(request));

        public override Task<CreateNodeReply> CreateStorage(CreateNodeRequest request, ServerCallContext context) => Task.FromResult(CreateNewStorage(request));

        public override Task<KillNodeReply> KillNode(KillNodeRequest request, ServerCallContext context) => Task.FromResult(KillNode(request));



        public CreateNodeReply CreateNewWorker(CreateNodeRequest request)
        {
            CreateProcess(@"DIDAWorker\bin\Debug\netcoreapp3.1\DIDAWorker.exe", $"{request.Name} {request.Url} {request.GossipDelay} {request.Debug}", request.Name);
            return new CreateNodeReply { Ok = true };
        }

        public CreateNodeReply CreateNewScheduler(CreateNodeRequest request)
        {
            CreateProcess(@"DIDAScheduler\bin\Debug\netcoreapp3.1\DIDAScheduler.exe", $"{request.Name} {request.Url}", request.Name);
            return new CreateNodeReply { Ok = true };
        }


        public CreateNodeReply CreateNewStorage(CreateNodeRequest request)
        {
            Console.WriteLine($"{ request.ServerId} { request.Name} { request.Url} { request.GossipDelay}");
            CreateProcess(@"DIDAStorage\bin\Debug\netcoreapp3.1\DIDAStorage.exe", $"{request.ServerId} {request.Name} {request.Url} {request.GossipDelay}", request.Name);
            return new CreateNodeReply { Ok = true };
        }

        public KillNodeReply KillNode(KillNodeRequest request)
        {
            KillProcess(request.Name);
            return new KillNodeReply { Ok = true };
        }


        private void CreateProcess(String path, String args, String name)
        {
            string base_dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;


            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = base_dir + "\\" + path,
                    Arguments = args,
                    UseShellExecute = true,
                }
            };
            proc.Start();

            runningProcs.Add(name, proc);
        }

        private void KillProcess(String name)
        {
            if (runningProcs.ContainsKey(name))
            {
                runningProcs[name].Kill();
                runningProcs.Remove(name);
            }
        }


        static void Main(string[] args)
        {

            Server server = new Server()
            {
                Services = { PCSService.BindService(new PCS()) },
                Ports = { new ServerPort("localhost", 10000, ServerCredentials.Insecure) }
            };

            server.Start();

            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
    }
}
