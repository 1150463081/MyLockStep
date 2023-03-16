using System;
using Server;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace TestExample
{
    class Program
    {
        static void Main(string[] args)
        {
            MemoryStream ms = new MemoryStream();
            long a = 1;
            long b = 2;
            long c = 3;
            BinaryWriter writer = new BinaryWriter(ms);
            writer.Write(a);
            writer.Write(b);
            writer.Write(c);

            ms.Position = 0;
            BinaryReader reader = new BinaryReader(ms);
            long a1 = reader.ReadInt64();
            long b1 = reader.ReadInt64();
            long c1 = reader.ReadInt64();
            Console.WriteLine($"a:{a1},b:{b1},c:{c1}");

            Console.ReadKey();
        }
    }
}
