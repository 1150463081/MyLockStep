using System;
using LockStepFrame;
using System.Collections.Generic;
using UnityEngine;

namespace SimplePhysx
{
    /// <summary>
    /// 基于定点数的碰撞检测基类
    /// </summary>
    public abstract class FixedPointCollider2DBase
    {
        public string Name { get; protected set; }
        public FXVector3 Pos { get; protected set; }

        List<(FXVector3 normal, FXVector3 adjust)> collisionInfos = new List<(FXVector3 normal, FXVector3 adjust)>();
        public void ClacCollison(List<FixedPointCollider2DBase> colliders, ref FXVector3 velocity, ref FXVector3 adjust)
        { 
            if (velocity == FXVector3.zero)
            {
                return;
            }
            FXVector3 normal = FXVector3.zero;
            FXVector3 adj = FXVector3.zero;
            collisionInfos.Clear();
            for (int i = 0; i < colliders.Count; i++)
            {
                if (DetectCollider(colliders[i], ref normal, ref adj))
                {
                    collisionInfos.Add((normal, adj));
                }
            }
            if (collisionInfos.Count == 1)
            {
                velocity = CorrectVelocity(velocity, collisionInfos[0].normal);
                adjust = collisionInfos[0].adjust;
            }
            else if (collisionInfos.Count > 1)//多个物体碰撞
            {
                //根据所有碰撞法线向量计算中心向量，并计算中心向量与法线向量最大夹角
                FXVector3 centerNormal = FXVector3.zero;
                FXVector3 correctNormal = FXVector3.zero;
                var nomalAngle = CalcMaxCenterNormalAngle(collisionInfos, velocity, ref centerNormal, ref correctNormal);
                //计算速度的反向量与法线的夹角
                var v2nAngle = FXVector3.Angle(-velocity, centerNormal);
                //可移动，计算偏移，修正速度
                if (v2nAngle > nomalAngle)
                {

                    velocity = CorrectVelocity(velocity, correctNormal);
                    var adjustSum = FXVector3.zero;
                    for (int i = 0; i < collisionInfos.Count; i++)
                    {
                        adjustSum += collisionInfos[i].adjust;
                    }
                    adjust = adjustSum;
                }
                else//不可移动
                {
                    velocity = FXVector3.zero;
                }
            }

        }

        /// <param name="col">碰撞盒</param>
        /// <param name="normal">碰撞面法线向量</param>
        /// <param name="adjust">位置修正向量</param>
        public bool DetectCollider(FixedPointCollider2DBase col, ref FXVector3 normal, ref FXVector3 adjust)
        {
            if (col is FixedPointSphereCollider2D sphereCol)
            {
                return DetectSphereCollider(sphereCol, ref normal, ref adjust);
            }
            else if (col is FixedPointBoxCollider2D boxCol)
            {
                return DetectBoxCollider(boxCol, ref normal, ref adjust);
            }
            return false;
        }

        /// <param name="sphereCol">球形碰撞盒</param>
        /// <param name="normal">碰撞面法线向量</param>
        /// <param name="adjust">位置修正向量</param>
        public abstract bool DetectSphereCollider(FixedPointSphereCollider2D sphereCol, ref FXVector3 normal, ref FXVector3 adjust);

        /// <param name="boxCol">盒形碰撞盒</param>
        /// <param name="normal">碰撞面法线向量</param>
        /// <param name="adjust">位置修正向量</param>
        public abstract bool DetectBoxCollider(FixedPointBoxCollider2D boxCol, ref FXVector3 normal, ref FXVector3 adjust);

        public void SetPos(FXVector3 pos)
        {
            if(pos!=Pos)
            Debug.LogError($"SetPos:{pos}");
            Pos = pos;
        }
        private FXVector3 CorrectVelocity(FXVector3 velocity, FXVector3 normal)
        {
            if (normal == FXVector3.zero)
            {
                return velocity;
            }
            //如果速度与法线夹角大于90度，表示正在靠近，需要修正速度
            if (FXVector3.Angle(velocity, normal) > FXArgs.HALFPI)
            {
                //算出速度在法线上的投影
                var a = FXVector3.Dot(velocity, normal);
                velocity = -a * normal + velocity;
            }
            return velocity;
        }
        /// <summary>
        /// 计算中心法线向量与最边缘法线向量夹角
        /// </summary>
        /// <param name="collisionInfos"></param>
        /// <returns></returns>
        private FXArgs CalcMaxCenterNormalAngle(List<(FXVector3 normal, FXVector3 adjust)> collisionInfos, FXVector3 velocity, ref FXVector3 centerNormal, ref FXVector3 correctNormal)
        {
            for (int i = 0; i < collisionInfos.Count; i++)
            {
                centerNormal += collisionInfos[i].normal;
            }
            if (collisionInfos.Count > 0)
            {
                centerNormal /= collisionInfos.Count;
            }

            FXArgs maxAngle = FXArgs.Zero;
            FXArgs maxCorrectAngle = FXArgs.Zero;
            for (int i = 0; i < collisionInfos.Count; i++)
            {
                var tempAngle = FXVector3.Angle(collisionInfos[i].normal, centerNormal);
                if (tempAngle > maxAngle)
                {
                    maxAngle = tempAngle;
                }
                //找出速度方向与法线方向夹角最大的碰撞法线，速度校正由这个法线来决定
                var tempAngle_2 = FXVector3.Angle(velocity, collisionInfos[i].normal);
                if (tempAngle_2 > maxCorrectAngle)
                {
                    maxCorrectAngle = tempAngle_2;
                    correctNormal = collisionInfos[i].normal;
                }
            }


            return maxAngle;
        }
        #region 分离轴算法相关
        /// <summary>
        /// 获取顶点在投影向量上的最两端投影点
        /// </summary>
        /// <param name="normal"></param>
        /// <param name="vertexs"></param>
        /// <returns></returns>
        protected FXVector3[] GetProjectedPoints(FXVector3 normal, FXVector3[] vertexs)
        {
            //计算每个顶点在轴上的投影点
            FXVector3[] projections = new FXVector3[2];
            FXInt min = int.MaxValue;
            FXInt max = int.MinValue;
            for (int i = 0; i < vertexs.Length; i++)
            {
                var project = FXVector3.Dot(vertexs[i], normal);
                if (project < min)
                {
                    min = project;
                    projections[0] = project * normal;
                }
                if (project > max)
                {
                    max = project;
                    projections[1] = project * normal;
                }
            }
            return projections;
        }
        protected FXInt GetMaxDis(FXVector3[] arr1, FXVector3[] arr2, ref FXVector3 normal)
        {
            FXInt max = int.MinValue;
            for (int i = 0; i < arr1.Length; i++)
            {
                for (int j = 0; j < arr2.Length; j++)
                {
                    var dis = (arr1[i] - arr2[j]).magnitude;
                    if (dis > max)
                    {
                        max = dis;
                        normal = (arr1[i] - arr2[j]).normalized;
                    }
                }
            }
            return max;
        }
        #endregion
    }
}
