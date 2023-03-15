using System;
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
        private List<MemoryStream> snapShotLst = new List<MemoryStream>();
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
        public void TakeSnapShot()
        {
            int frame = GetModule<BattleMgr>().LocalFrameId;
            var stream = AccrueStream();
            snapShotLst.Add(stream);
            snapShotWriter.Init(stream);
            for (int i = 0; i < members.Count; i++)
            {
                members[i].TakeSnapShot(snapShotWriter);
            }
            var buffer = stream.GetBuffer();
            var stringbuilder = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                stringbuilder.Append(buffer[i]);
            }
            Debug.LogError($"SnapShot:{stringbuilder.ToString()}");
            snapShotWriter.Flush();
        }
        public void RollBackTo(int Frame)
        {

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
