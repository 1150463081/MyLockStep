using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LockStepFrame
{
    [Module]
    public class TimerManager:Module
    {
        private class Timer
        {
            public bool IsActive = false;
            public bool IsCompleted = false;
            public int TimerId;

            private float nowTime;
            private float nowLoopCount;
            private float targetTime;

            private float delayTime;
            private float waitTime;
            private int maxLoopCount;
            private Action loopEndEvt;//每次循环结束触发的事件
            private Action timerEndEvt;//计时全部结束时触发的事件


            public int InitTimer(float delayTime, float waitTime, int maxLoopCount, Action loopEndEvt, Action timerEndEvt)
            {
                this.delayTime = delayTime;
                this.waitTime = waitTime;
                this.maxLoopCount = maxLoopCount;
                this.loopEndEvt = loopEndEvt;
                this.timerEndEvt = timerEndEvt;
                TimerId = Utility.Common.GenerateId<Timer>();
                return TimerId;
            }

            public void StartTimer()
            {
                IsActive = true;
                targetTime = delayTime + (nowLoopCount + 1) * waitTime;
            }
            public void PlayTimer()
            {
                IsActive = true;
            }
            public void PauseTimer()
            {
                IsActive = false;
            }
            public void StopTimer()
            {
                IsCompleted = true;
            }
            public void TickTimer(float delta)
            {
                if (!IsActive || IsCompleted)
                {
                    return;
                }
                nowTime += delta;
                if (nowTime > targetTime)
                {
                    loopEndEvt?.Invoke();
                    nowLoopCount++;
                    if (nowLoopCount >= maxLoopCount)//循环结束
                    {
                        timerEndEvt?.Invoke();
                        //todo 回收计时
                        IsCompleted = true;
                    }
                    else//开始下一次循环
                    {
                        targetTime = delayTime + (nowLoopCount + 1) * waitTime;
                    }
                }
            }
        }

        private Dictionary<int, Timer> timerDict = new Dictionary<int, Timer>();
        private Queue<Timer> stopQueue = new Queue<Timer>();

        public void Tick(float delta)
        {
            stopQueue.Clear();
            foreach (var timer in timerDict.Values)
            {
                timer.TickTimer(delta);
                if (timer.IsCompleted)
                {
                    stopQueue.Enqueue(timer);
                }
            }
            while (stopQueue.Count > 0)
            {
                timerDict.Remove(stopQueue.Dequeue().TimerId);
            }
        }

        public int StartTimer(float delayTime, float waitTime, int maxLoopCount, Action loopEndEvt, Action timerEndEvt)
        {
            Timer timer = new Timer();
            int timerId = timer.InitTimer(delayTime, waitTime, maxLoopCount, loopEndEvt, timerEndEvt);
            timer.StartTimer();
            timerDict[timerId] = timer;
            return timerId;
        }
        public void PlayTimer()
        {
        }
        public void PauseTimer()
        {
        }
        public void StopTimer()
        {
        }


        public override void OnUpdate()
        {
            Tick(Time.deltaTime);
        }

    }
}
