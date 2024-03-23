using Grpc.Core;
using System;


namespace DIDAWorker
{
    public class WorkerServer
    {

        static void Main(string[] args)
        {
            string urlParsed = args[1].Remove(0, 7); //remover http://
            Server server = new Server()
            {
                Services = { DIDAWorkerService.BindService(new Worker(args[0], int.Parse(args[2]), bool.Parse(args[3]))) },
                //assuming the url starts with http://
                Ports = { new ServerPort(
                    urlParsed.Split(":")[0],
                    int.Parse(urlParsed.Split(":")[1]), ServerCredentials.Insecure) }
            };

            Console.WriteLine($"Worker {args[0]} starting at {args[1]}");
            server.Start();


            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }

    }
}
