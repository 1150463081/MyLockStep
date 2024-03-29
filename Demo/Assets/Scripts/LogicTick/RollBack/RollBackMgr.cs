﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LockStepFrame;
using UnityEngine;
using System.IO;

namespace GameCore
{
    [Module]
    public class RollBackMgr:Module
    {
        private List<IRollBack> members=new List<IRollBack>();
        private Dictionary<int, MemoryStream> snapShotDict = new Dictionary<int, MemoryStream>();
        private Queue<MemoryStream> streamPool = new Queue<MemoryStream>();
        private SnapShotWriter snapShotWriter;
        private SnapShotReader snapShotReader;

        public override void OnInit()
        {
            base.OnInit();
            snapShotWriter = new SnapShotWriter();
            snapShotReader = new SnapShotReader();
        }

        /// <summary>
        /// 每次逻辑帧结束时统一进行快照
        /// </summary>
        public void TakeSnapShot(int frameId)
        {
            var stream = AccrueStream();
            snapShotDict[frameId]=stream;
            snapShotWriter.Init(stream);
            for (int i = 0; i < members.Count; i++)
            {
                members[i].TakeSnapShot(snapShotWriter);
            }
            stream.Position = 0;
            snapShotWriter.Flush();
        }
        public void RollBackTo(int frame)
        {
            if(!snapShotDict.ContainsKey(frame))
            {
                Debug.LogError($"Roll Back To {frame} Failed!!");
                return;
            }
            var stream = snapShotDict[frame];
            stream.Position = 0;
            snapShotReader.Init(stream);
            for (int i = 0; i < members.Count; i++)
            {
                members[i].RollBackTo(snapShotReader);
            }
        }
        public void Register(IRollBack r)
        {
            if (members.Contains(r))
            {
                Debug.LogError("RollBack Register Failed!!!");
                return;
            }
            members.Add(r);
        }

        public void DeRegister(IRollBack r)
        {
            if (!members.Contains(r))
            {
                Debug.LogError("RollBack DeRegister Failed!!!");
                return;
            }
            members.Remove(r);
        }

        private MemoryStream AccrueStream()
        {
            if (streamPool.Count > 0)
            {
                return streamPool.Dequeue();
            }
            else
            {
                return new MemoryStream();
            }
        }
        private void RecyleStream(MemoryStream stream)
        {
            stream.Position = 0;
            streamPool.Enqueue(stream);
        }
    }
}
