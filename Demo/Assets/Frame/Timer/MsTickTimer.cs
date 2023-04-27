using System;
using System.Collections.Generic;
using UnityEngine;

namespace LockStepFrame
{
    /// <summary>
    /// 毫秒级计时器
    /// </summary>
    public class MsTickTimer
    {
        private int startId = 0;
        private string locker = "MsTickTimer_locker";
        private Dictionary<int, TickTask> taskDict = new Dictionary<int, TickTask>();

        public int AddTask(int delay, Action callEvt, Action cancelEvt, int loopCnt = 1, long startTime = -1)
        {
            var tId = GenerateTaskId();
            TickTask task;
            if (startTime < 0)
            {
                var destTime = Utility.Time.MillisecondNow() + delay;
                task = new TickTask(tId, delay, destTime, callEvt, cancelEvt, loopCnt);
            }
            else
            {
                var nowTime = Utility.Time.MillisecondNow();
                long destTime;
                if (nowTime > startTime)
                {
                    destTime = startTime + delay;
                    task = new TickTask(tId, delay, destTime, callEvt, cancelEvt, loopCnt);
                    while (task.DestTime < nowTime)
                    {
                        task.Tick(nowTime);
                    }
                }
                else
                {
                    destTime = startTime + delay;
                    task = new TickTask(tId, delay, destTime, callEvt, cancelEvt, loopCnt);
                }

            }
            taskDict[tId] = task;
            return tId;
        }
        public void UpdateTask()
        {
            var nowTime = Utility.Time.MillisecondNow();
            foreach (var task in taskDict.Values)
            {
                if (task.IsOver)
                {
                    DeleteTask(task.Id);
                }
                task.Tick(nowTime);
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
                Debug.LogError($"Not Exist MsTickTask {taskId}");
            }
        }
        public void ChangeInterval(int taskId, int interval)
        {
            if (taskDict.ContainsKey(taskId))
            {
                taskDict[taskId].ChangeInterval(interval);
            }
            else
            {
                Debug.LogError($"Not Exist MsTickTask {taskId}");
            }
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
            public int Interval;
            public double DestTime;
            public Action CallEvt;
            public Action CancelEvt;
            public int LoopCnt;
            public int LoopIndex;
            public bool IsLoop => LoopCnt <= 0;
            public bool IsOver;

            public TickTask(int id, int interval, double destTime, Action callEvt, Action cancelEvt, int loopCnt)
            {
                Id = id;
                Interval = interval;
                DestTime = destTime;
                CallEvt = callEvt;
                CancelEvt = cancelEvt;
                LoopCnt = loopCnt;
                LoopIndex = 0;
                IsOver = false;
            }
            public void Tick(long nowTime)
            {
                if (IsOver || nowTime < DestTime)
                {
                    return;
                }
                LoopIndex++;
                if (LoopIndex <= LoopCnt || IsLoop)
                {
                    CallEvt?.Invoke();
                    DestTime += Interval;
                }
                if (LoopIndex >= LoopCnt && !IsLoop)
                {
                    //计时循环结束
                    IsOver = true;
                }
            }
            public void ChangeInterval(int interval)
            {
                if (Interval == interval)
                {
                    return;
                }
                DestTime -= Interval;
                Interval = interval;
                DestTime += Interval;
            }
        }
    }
}
