using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;

namespace Server
{
    /// <summary>
    /// 毫秒级计时器
    /// </summary>
    public class MsTickTimer
    {
        private DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        private int startId = 0;
        private string locker = "MsTickTimer_locker";
        private Dictionary<int, TickTask> taskDict = new Dictionary<int, TickTask>();

        public int AddTask(int delay, Action callEvt, Action cancelEvt, int loopCnt = 1)
        {
            var destTime = GetUTCNowMilliseconds() + delay;
            var tId = GenerateTaskId();
            var task = new TickTask(tId, delay, destTime, callEvt, cancelEvt, loopCnt);
            if (taskDict.TryAdd(tId, task))
            {
                return tId;
            }
            else
            {
                return -1;
            }
        }
        public void UpdateTask()
        {
            var nowTime = GetUTCNowMilliseconds();
            foreach (var task in taskDict.Values)
            {
                if (nowTime < task.DestTime)
                {
                    continue;
                }
                task.LoopIndex++;
                if (task.LoopIndex <= task.LoopCnt)
                {
                    task.CallEvt?.Invoke();
                    task.DestTime += task.Delay;
                }
                if (task.LoopIndex >= task.LoopCnt)
                {
                    //计时循环结束
                    DeleteTask(task.Id);
                }
            }
        }
        public void DeleteTask(int taskId)
        {
            if (taskDict.ContainsKey(taskId))
            {
                taskDict[taskId].CancelEvt?.Invoke();
                taskDict.Remove(taskId);
            }
            else
            {
                Utility.Log.Error($"Not Exist MsTickTask {taskId}");
            }
        }

        private double GetUTCNowMilliseconds()
        {
            var ts = DateTime.UtcNow - startTime;
            return ts.TotalMilliseconds;
        }
        private int GenerateTaskId()
        {
            lock (locker)
            {
                return startId++;
            }
        }

        private class TickTask
        {
            public int Id;
            public int Delay;
            public double DestTime;
            public Action CallEvt;
            public Action CancelEvt;
            public int LoopCnt;
            public int LoopIndex;

            public TickTask(int id, int delay, double destTime, Action callEvt, Action cancelEvt, int loopCnt)
            {
                Id = id;
                Delay = delay;
                DestTime = destTime;
                CallEvt = callEvt;
                CancelEvt = cancelEvt;
                LoopCnt = loopCnt;
                LoopIndex = 0;
            }
        }
    }
}
