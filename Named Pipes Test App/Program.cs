using System;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;

namespace Named_Pipes_Test_App
{
    class Program
    {
        static void Main(string[] args)
        {
            StartServer();

            Task.Delay(2000).Wait();

            StartClient();

            Console.WriteLine("Done!");
        }

        static void StartServer()
        {
            Task.Factory.StartNew(() =>
            {
                var server = new NamedPipeServerStream("TestPipes");
                server.WaitForConnection();
                StreamReader reader = new StreamReader(server);
                StreamWriter writer = new StreamWriter(server);
                while (true)
                {
                    var line = reader.ReadLine();
                    writer.WriteLine("Server: Your message was - " + String.Join("", line));
                    writer.Flush();
                }
            });
        }

        static void StartClient()
        {
            Console.WriteLine("Enter your message...");

            var client = new NamedPipeClientStream("TestPipes");
            client.Connect();
            StreamReader reader = new StreamReader(client);
            StreamWriter writer = new StreamWriter(client);
            
            while (true)
            {
                string input = Console.ReadLine();
                if (String.IsNullOrEmpty(input)) break;
                writer.WriteLine(input);
                writer.Flush();
                Console.WriteLine(reader.ReadLine());
            }
        }
    }
}
