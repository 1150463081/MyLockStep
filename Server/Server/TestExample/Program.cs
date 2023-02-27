using System;
using Server;
using System.Threading;
using System.Threading.Tasks;

namespace TestExample
{
    class Program
    {
        static void Main(string[] args)
        {
            MsTickTimer timer = new MsTickTimer();
            int interval = 2000;
            int loopCount = 200;
            Task.Run(() =>
            {
                Task.Delay(2000);
                DateTime histroyTime = DateTime.UtcNow;
                int i = 0;
                double sum = 0;
                try
                {
                    timer.AddTask(interval, () =>
                    {
                        TimeSpan ts = DateTime.UtcNow - histroyTime;
                        histroyTime = DateTime.UtcNow;
                        double delta = ts.TotalMilliseconds - interval;
                        i++;
                        sum += Math.Abs(delta);
                        Utility.Log.Debug($"第{i}次计时结束,误差{delta},{DateTime.UtcNow}");
                    }, ()=> {
                        Utility.Log.Debug($"平均误差:{sum / (loopCount * 1f)}");
                    }, loopCount);
                }
                catch(Exception e)
                {
                    Utility.Log.Error(e.ToString());
                }
            });
            Task.Run(() =>
            {
                while (true)
                {
                    timer.UpdateTask();
                    Task.Delay(5);
                }
            });
            Console.ReadKey();
        }
    }
}
