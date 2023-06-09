using LockStepFrame;
using SimplePhysx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePhysx
{
    public class QuadTree
    {
        private Dictionary<string, QuadCell> cellDict = new Dictionary<string, QuadCell>();
        //单元格长度
        private FXInt cellLength;

        public QuadTree(int cellLength)
        {
            this.cellLength = cellLength;
        }
        public static string GetCellKey(int xIndex, int zIndex)
        {
            string key = xIndex + "," + zIndex;
            return key;
        }
        public void OnColPosChange(FixedPointCollider2DBase col)
        {
            //碰撞体划分的区域是否有变动
            var inRangeCells = GetInRangeCell(col);
            col.RefreshCells(inRangeCells);
        }
        public QuadCell GetCell(int xIndex, int zIndex)
        {
            var key = GetCellKey(xIndex, zIndex);
            if (!cellDict.ContainsKey(key))
            {
                var quadCell = new QuadCell(xIndex, zIndex, cellLength);
                cellDict[key] = quadCell;
            }
            return cellDict[key];
        }
        public QuadCell GetCell(string key)
        {
            if (cellDict.ContainsKey(key))
            {
                return cellDict[key];
            }
            return null;
        }
        //获取在碰撞体范围内的宫格
        private List<QuadCell> GetInRangeCell(FixedPointCollider2DBase col)
        {
            //获取碰撞体中心所在格
            var indexTuple = GetAreaIndex(col.Pos);
            int xIndex = indexTuple.Item1;
            int zIndex = indexTuple.Item2;
            var centerCell = GetCell(xIndex, zIndex);
            List<QuadCell> inRangeCells = new List<QuadCell>();
            Dictionary<string, bool> cacheDict = new Dictionary<string, bool>();
            inRangeCells.Add(centerCell);
            cacheDict.Add(centerCell.Key, true);
            DetectAroundCell(col, xIndex, zIndex, inRangeCells, cacheDict);
            return inRangeCells;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="col"></param>
        /// <param name="xIndex"></param>
        /// <param name="zIndex"></param>
        /// <param name="inRangeCell">接触到的区域</param>
        /// <param name="hasCheckCells">已经校验过的区域</param>
        private void DetectAroundCell(FixedPointCollider2DBase col, int xIndex, int zIndex, List<QuadCell> inRangeCell, Dictionary<string, bool> cacheDict)
        {
            //获取中心格周围的八个宫格
            //查找周围格是否有接触，如果有新的接触格，再以新的周围格为中心检测，以此循环
            List<string> newCells = new List<string>();
            #region 周围八个宫格检测
            //上
            var upCell = GetCell(xIndex, zIndex + 1);
            if (!cacheDict.ContainsKey(upCell.Key))
            {
                cacheDict[upCell.Key] = col.DetectBoxCollider(upCell);
                if (cacheDict[upCell.Key])
                {
                    inRangeCell.Add(upCell);
                    newCells.Add(upCell.Key);
                }
            }
            //右
            var rightCell = GetCell(xIndex + 1, zIndex);
            if (!cacheDict.ContainsKey(rightCell.Key))
            {
                cacheDict[rightCell.Key] = col.DetectBoxCollider(rightCell);
                if (cacheDict[rightCell.Key])
                {
                    inRangeCell.Add(rightCell);
                    newCells.Add(rightCell.Key);
                }
            }
            //下
            var downCell = GetCell(xIndex, zIndex - 1);
            if (!cacheDict.ContainsKey(downCell.Key))
            {
                cacheDict[downCell.Key] = col.DetectBoxCollider(downCell);
                if (cacheDict[downCell.Key])
                {
                    inRangeCell.Add(downCell);
                    newCells.Add(downCell.Key);
                }
            }
            //左
            var leftCell = GetCell(xIndex - 1, zIndex);
            if (!cacheDict.ContainsKey(leftCell.Key))
            {
                cacheDict[leftCell.Key] = col.DetectBoxCollider(leftCell);
                if (cacheDict[leftCell.Key])
                {
                    inRangeCell.Add(leftCell);
                    newCells.Add(leftCell.Key);
                }
            }
            //上右
            var upRightCell = GetCell(xIndex + 1, zIndex + 1);
            if (!cacheDict.ContainsKey(upRightCell.Key))
            {
                if (cacheDict[upCell.Key] || cacheDict[rightCell.Key])
                {
                    cacheDict[upRightCell.Key] = col.DetectBoxCollider(upRightCell);
                }
                else
                {
                    cacheDict[upRightCell.Key] = false;
                }
                if (cacheDict[upRightCell.Key])
                {
                    inRangeCell.Add(upRightCell);
                    newCells.Add(upRightCell.Key);
                }
            }
            //下右
            var downRightCell = GetCell(xIndex + 1, zIndex - 1);
            if (!cacheDict.ContainsKey(downRightCell.Key))
            {
                if (cacheDict[downCell.Key] || cacheDict[rightCell.Key])
                {
                    cacheDict[downRightCell.Key] = col.DetectBoxCollider(downRightCell);
                }
                else
                {
                    cacheDict[downRightCell.Key] = false;
                }
                if (cacheDict[downRightCell.Key])
                {
                    inRangeCell.Add(downRightCell);
                    newCells.Add(downRightCell.Key);
                }
            }
            //下左
            var downLeftCell = GetCell(xIndex - 1, zIndex - 1);
            if (!cacheDict.ContainsKey(downLeftCell.Key))
            {
                if (cacheDict[downCell.Key] || cacheDict[leftCell.Key])
                {
                    cacheDict[downLeftCell.Key] = col.DetectBoxCollider(downLeftCell);
                }
                else
                {
                    cacheDict[downLeftCell.Key] = false;
                }
                if (cacheDict[downLeftCell.Key])
                {
                    inRangeCell.Add(downLeftCell);
                    newCells.Add(downLeftCell.Key);
                }
            }
            //上左
            var upLeftCell = GetCell(xIndex - 1, zIndex + 1);
            if (!cacheDict.ContainsKey(upLeftCell.Key))
            {
                if (cacheDict[upCell.Key] || cacheDict[leftCell.Key])
                {
                    cacheDict[upLeftCell.Key] = col.DetectBoxCollider(upLeftCell);
                }
                else
                {
                    cacheDict[upLeftCell.Key] = false;
                }
                if (cacheDict[upLeftCell.Key])
                {
                    inRangeCell.Add(upLeftCell);
                    newCells.Add(upLeftCell.Key);
                }
            }
            #endregion
            for (int i = 0; i < newCells.Count; i++)
            {
                var cell = GetCell(newCells[i]);
                DetectAroundCell(col, cell.XIndex, cell.ZIndex, inRangeCell, cacheDict);
            }
        }
        private (int, int) GetAreaIndex(FXVector3 pos)
        {
            FXInt xIndex = pos.x / cellLength;
            if (xIndex >= 0)
            {
                xIndex = xIndex.Celling();
            }
            else
            {
                xIndex = -(-xIndex).Celling();
            }

            FXInt zIndex = pos.z / cellLength;
            if (zIndex >= 0)
            {
                zIndex = zIndex.Celling();
            }
            else
            {
                zIndex = -(-zIndex).Celling();
            }

            return (xIndex.RawInt, zIndex.RawInt);
        }
    }

}
