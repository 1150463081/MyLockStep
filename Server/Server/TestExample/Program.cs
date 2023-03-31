using System;
using Server;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace TestExample
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> a = new List<int>() { 1, 2, 3, 4 };
            List<int> b = new List<int>() { 3, 4, 5, 6 };

            var c = a.Except(b);
            var d = b.Except(a);

            foreach (var item in c)
            {
                Console.WriteLine($"{item}");
            }
            foreach (var item in d)
            {
                Console.WriteLine($"{item}");
            }

            Console.WriteLine($"{string.IsNullOrEmpty(" ")},{string.IsNullOrWhiteSpace("")},,{string.IsNullOrWhiteSpace(" ")}");

            Console.ReadKey();
        }
    }
}
