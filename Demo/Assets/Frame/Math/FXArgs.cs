
using System;

namespace LockStepFrame
{
    public struct FXArgs
    {
        public int value;
        public uint multipler;

        public FXArgs(int value, uint multipler)
        {
            this.value = value;
            this.multipler = multipler;
        }

        public static FXArgs Zero = new FXArgs(0, 10000);
        public static FXArgs HALFPI = new FXArgs(15708, 10000);
        public static FXArgs PI = new FXArgs(31416, 10000);
        public static FXArgs TWOPI = new FXArgs(62832, 10000);

        public static bool operator >(FXArgs a, FXArgs b)
        {
            if (a.multipler == b.multipler)
            {
                return a.value > b.value;
            }
            else
            {
                throw new System.Exception("multipler is unequal.");
            }
        }
        public static bool operator <(FXArgs a, FXArgs b)
        {
            if (a.multipler == b.multipler)
            {
                return a.value < b.value;
            }
            else
            {
                throw new System.Exception("multipler is unequal.");
            }
        }
        public static bool operator >=(FXArgs a, FXArgs b)
        {
            if (a.multipler == b.multipler)
            {
                return a.value >= b.value;
            }
            else
            {
                throw new System.Exception("multipler is unequal.");
            }
        }
        public static bool operator <=(FXArgs a, FXArgs b)
        {
            if (a.multipler == b.multipler)
            {
                return a.value <= b.value;
            }
            else
            {
                throw new System.Exception("multipler is unequal.");
            }
        }
        public static bool operator ==(FXArgs a, FXArgs b)
        {
            if (a.multipler == b.multipler)
            {
                return a.value == b.value;
            }
            else
            {
                throw new System.Exception("multipler is unequal.");
            }
        }
        public static bool operator !=(FXArgs a, FXArgs b)
        {
            if (a.multipler == b.multipler)
            {
                return a.value != b.value;
            }
            else
            {
                throw new System.Exception("multipler is unequal.");
            }
        }


        /// <summary>
        /// 转化为视图角度，不可再用于逻辑运算
        /// </summary>
        /// <returns></returns>
        public int ConvertViewAngle()
        {
            float radians = ConvertToFloat();
            return (int)Math.Round(radians / Math.PI * 180);
        }

        /// <summary>
        /// 转化为视图弧度，不可再用于逻辑运算
        /// </summary>
        public float ConvertToFloat()
        {
            return value * 1.0f / multipler;
        }

        public override bool Equals(object obj)
        {
            return obj is FXArgs args &&
                value == args.value &&
                multipler == args.multipler;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public override string ToString()
        {
            return $"value:{value} multipler:{multipler}";
        }
    }
}
