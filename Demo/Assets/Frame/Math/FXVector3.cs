
using System;
using System.Collections.Generic;
using System.Text;


using UnityEngine;

namespace LockStepFrame
{
    public struct FXVector3
    {
        public FXInt x;
        public FXInt y;
        public FXInt z;
        public FXVector3(FXInt x, FXInt y, FXInt z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }


        public FXVector3(Vector3 v) {
            this.x = (FXInt)v.x;
            this.y = (FXInt)v.y;
            this.z = (FXInt)v.z;
        }


        public FXInt this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return x;
                    case 1:
                        return y;
                    case 2:
                        return z;
                    default:
                        return 0;
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    case 2:
                        z = value;
                        break;
                }
            }
        }

        #region 定义常用向量
        public static FXVector3 zero
        {
            get
            {
                return new FXVector3(0, 0, 0);
            }
        }
        public static FXVector3 one
        {
            get
            {
                return new FXVector3(1, 1, 1);
            }
        }
        public static FXVector3 forward
        {
            get
            {
                return new FXVector3(0, 0, 1);
            }
        }
        public static FXVector3 back
        {
            get
            {
                return new FXVector3(0, 0, -1);
            }
        }
        public static FXVector3 left
        {
            get
            {
                return new FXVector3(-1, 0, 0);
            }
        }
        public static FXVector3 right
        {
            get
            {
                return new FXVector3(1, 0, 0);
            }
        }
        public static FXVector3 up
        {
            get
            {
                return new FXVector3(0, 1, 0);
            }
        }
        public static FXVector3 down
        {
            get
            {
                return new FXVector3(0, -1, 0);
            }
        }
        #endregion

        #region 运算符
        public static FXVector3 operator +(FXVector3 v1, FXVector3 v2)
        {
            FXInt x = v1.x + v2.x;
            FXInt y = v1.y + v2.y;
            FXInt z = v1.z + v2.z;
            return new FXVector3(x, y, z);
        }
        public static FXVector3 operator -(FXVector3 v1, FXVector3 v2)
        {
            FXInt x = v1.x - v2.x;
            FXInt y = v1.y - v2.y;
            FXInt z = v1.z - v2.z;
            return new FXVector3(x, y, z);
        }
        public static FXVector3 operator *(FXVector3 v, FXInt value)
        {
            FXInt x = v.x * value;
            FXInt y = v.y * value;
            FXInt z = v.z * value;
            return new FXVector3(x, y, z);
        }
        public static FXVector3 operator *(FXInt value, FXVector3 v)
        {
            FXInt x = v.x * value;
            FXInt y = v.y * value;
            FXInt z = v.z * value;
            return new FXVector3(x, y, z);
        }
        public static FXVector3 operator /(FXVector3 v, FXInt value)
        {
            FXInt x = v.x / value;
            FXInt y = v.y / value;
            FXInt z = v.z / value;
            return new FXVector3(x, y, z);
        }
        public static FXVector3 operator -(FXVector3 v)
        {
            FXInt x = -v.x;
            FXInt y = -v.y;
            FXInt z = -v.z;
            return new FXVector3(x, y, z);
        }

        public static bool operator ==(FXVector3 v1, FXVector3 v2)
        {
            return v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;
        }
        public static bool operator !=(FXVector3 v1, FXVector3 v2)
        {
            return v1.x != v2.x || v1.y != v2.y || v1.z != v2.z;
        }
        #endregion


        /// <summary>
        /// 当前向量长度平方
        /// </summary>
        public FXInt sqrMagnitude
        {
            get
            {
                return x * x + y * y + z * z;
            }
        }

        public static FXInt SqrMagnitude(FXVector3 v)
        {
            return v.x * v.x + v.y * v.y + v.z * v.z;
        }

        public FXInt magnitude
        {
            get
            {
                return FXCalc.Sqrt(this.sqrMagnitude);
            }
        }

        /// <summary>
        /// 返回当前定点向量的单位向量
        /// </summary>
        public FXVector3 normalized
        {
            get
            {
                if (magnitude > 0)
                {
                    FXInt rate = FXInt.one / magnitude;
                    return new FXVector3(x * rate, y * rate, z * rate);
                }
                else
                {
                    return zero;
                }
            }
        }

        /// <summary>
        /// 返回传入参数向量的单位向量
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static FXVector3 Normalize(FXVector3 v)
        {
            if (v.magnitude > 0)
            {
                FXInt rate = FXInt.one / v.magnitude;
                return new FXVector3(v.x * rate, v.y * rate, v.z * rate);
            }
            else
            {
                return zero;
            }
        }

        /// <summary>
        /// 规格化当前PE向量为单位向量
        /// </summary>
        public void Normalize()
        {
            FXInt rate = FXInt.one / magnitude;
            x *= rate;
            y *= rate;
            z *= rate;
        }

        /// <summary>
        /// 点乘
        /// </summary>
        public static FXInt Dot(FXVector3 a, FXVector3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        /// <summary>
        /// 叉乘
        /// </summary>
        public static FXVector3 Cross(FXVector3 a, FXVector3 b)
        {
            return new FXVector3(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
        }

        /// <summary>
        /// 向量夹角
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static FXArgs Angle(FXVector3 from, FXVector3 to)
        {
            FXInt dot = Dot(from, to);
            FXInt mod = from.magnitude * to.magnitude;
            if (mod == 0)
            {
                return FXArgs.Zero;
            }
            FXInt value = dot / mod;
            //反余弦函数计算
            return FXCalc.Acos(value);
        }

        /// <summary>
        /// 获取浮点数向量（注意：不可再进行逻辑运算）
        /// </summary>
        public Vector3 ConvertViewVector3() {
            return new Vector3(x.RawFloat, y.RawFloat, z.RawFloat);
        }

        public long[] CovertLongArray()
        {
            return new long[] { x.ScaledValue, y.ScaledValue, z.ScaledValue };
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            FXVector3 v = (FXVector3)obj;
            return v.x == x && v.y == y && v.z == z;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("x:{0} y:{1} z:{2}", x, y, z);
        }
    }
}
