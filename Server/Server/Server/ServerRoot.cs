using System;
using System.Threading;

namespace Server
{
    class ServerRoot
    {
        static void Main(string[] args)
        {
            ModuleManager.Instance.Init();
            while (true)
            {
                ModuleManager.Instance.Update();
                Thread.Sleep(10);
            }   
        }
    }
}
