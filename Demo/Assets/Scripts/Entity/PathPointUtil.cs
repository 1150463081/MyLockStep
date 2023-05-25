using LockStepFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameCore
{
    public class PathPointUtil
    {
        private class PathPoint
        {
            public FXVector3 Pos;
            public PathPoint Next;
            private GameObject go;
            private LineRenderer line;
            public PathPoint(FXVector3 pos)
            {
                Pos = pos;
                go = new GameObject();
                go.transform.position = pos.ConvertViewVector3();
                line = go.AddComponent<LineRenderer>();
                line.startWidth = 0.1f;
                line.endWidth = 0.1f;
            }
            public void SetNext(PathPoint point)
            {
                Next = point;
                line.SetPosition(0, Pos.ConvertViewVector3());
                line.SetPosition(1, Next.Pos.ConvertViewVector3());
            }
        }

        private List<PathPoint> pathPointList = new List<PathPoint>();
        public void AddPathPoint(FXVector3 pos)
        {
            if (pathPointList.Count != 0 && pathPointList.Last().Pos == pos)
            {
                //和最后一个路径点相同，不记录
                return;
            }
            var pathPoint = new PathPoint(pos);
            if (pathPointList.Count != 0)
            {
                pathPointList.Last().SetNext(pathPoint);
            }
            pathPointList.Add(pathPoint);
        }
    }
}
