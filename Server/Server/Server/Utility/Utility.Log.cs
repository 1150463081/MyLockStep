using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public partial class Utility
    {
        public class Log
        {
            public static void Debug(string str)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(str);
            }
            public static void Warn(string str)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(str);
            }
            public static void Error(string str)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(str);
            }
            public static void ColorLog(ConsoleColor color,string str)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(str);
            }
        }
    }
}
