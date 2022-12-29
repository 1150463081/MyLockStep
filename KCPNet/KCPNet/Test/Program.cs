using System;
using System.Threading;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client();
            client.StartClient();
            while (true)
            {
                var input = Console.ReadLine();
                if (input == "quit")
                {
                    client.CloseClient();
                }
            }
        }
    }
    public class Client
    {
        private CancellationTokenSource cts;
        private CancellationToken ct;
        public Client()
        {
            cts = new CancellationTokenSource();
            ct = cts.Token;
        }
        public void StartClient()
        {
            Console.WriteLine($"Client Start");
            Task.Run(ShowTime,ct);
        }
        public void CloseClient()
        {
            Console.WriteLine($"Client Close");
            cts.Cancel();
        }
         void ShowTime()
        {
            while (true)
            {
                if (ct.IsCancellationRequested)
                {
                    Console.WriteLine($"Task is Canceled");
                    break;
                }
                Console.WriteLine($"ShowTime:{DateTime.Now}");
                Thread.Sleep(1000);
            }
        }
    }
}
