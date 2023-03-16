using System;

namespace LockStepFrame
{
    [Serializable]
    public struct FXInt
    {
        private long scaledValue;
        public long ScaledValue
        {
            get
            {
                return scaledValue;
            }
            set
            {
                scaledValue = value;
            }
        }
        //移位计数
        const int BIT_MOVE_COUNT = 10;
        const long MULTIPLIER_FACTOR = 1 << BIT_MOVE_COUNT;

        public static readonly FXInt zero = new FXInt(0);
        public static readonly FXInt one = new FXInt(1);


        #region 构造函数
        //内部使用，已经缩放完成的数据
        private FXInt(long scaledValue)
        {
            this.scaledValue = scaledValue;
        }
        public FXInt(int val)
        {
            scaledValue = val * MULTIPLIER_FACTOR;
        }
        public FXInt(float val)
        {
            scaledValue = (long)Math.Round(val * MULTIPLIER_FACTOR);
        }
        //float损失精度，必须显式转换
        public static explicit operator FXInt(float f)
        {
            return new FXInt((long)Math.Round(f * MULTIPLIER_FACTOR));
        }
        //int不损失精度，可以隐式转换
        public static implicit operator FXInt(int i)
        {
            return new FXInt(i);
        }
        #endregion

        #region 运算符
        //加，减，乘，除，取反
        public static FXInt operator +(FXInt a, FXInt b)
        {
            return new FXInt(a.scaledValue + b.scaledValue);
        }
        public static FXInt operator -(FXInt a, FXInt b)
        {
            return new FXInt(a.scaledValue - b.scaledValue);
        }
        public static FXInt operator *(FXInt a, FXInt b)
        {
            long value = a.scaledValue * b.scaledValue;
            if (value >= 0)
            {
                value >>= BIT_MOVE_COUNT;
            }
            else
            {
                value = -(-value >> BIT_MOVE_COUNT);
            }
            return new FXInt(value);
        }
        public static FXInt operator /(FXInt a, FXInt b)
        {
            if (b.scaledValue == 0)
            {
                throw new Exception();
            }
            return new FXInt((a.scaledValue << BIT_MOVE_COUNT) / b.scaledValue);
        }
        public static FXInt operator -(FXInt value)
        {
            return new FXInt(-value.scaledValue);
        }
        public static bool operator ==(FXInt a, FXInt b)
        {
            return a.scaledValue == b.scaledValue;
        }
        public static bool operator !=(FXInt a, FXInt b)
        {
            return a.scaledValue != b.scaledValue;
        }
        public static bool operator >(FXInt a, FXInt b)
        {
            return a.scaledValue > b.scaledValue;
        }
        public static bool operator <(FXInt a, FXInt b)
        {
            return a.scaledValue < b.scaledValue;
        }
        public static bool operator >=(FXInt a, FXInt b)
        {
            return a.scaledValue >= b.scaledValue;
        }
        public static bool operator <=(FXInt a, FXInt b)
        {
            return a.scaledValue <= b.scaledValue;
        }

        public static FXInt operator >>(FXInt value, int moveCount)
        {
            if (value.scaledValue >= 0)
            {
                return new FXInt(value.scaledValue >> moveCount);
            }
            else
            {
                return new FXInt(-(-value.scaledValue >> moveCount));
            }
        }
        public static FXInt operator <<(FXInt value, int moveCount)
        {
            return new FXInt(value.scaledValue << moveCount);
        }
        #endregion

        /// <summary>
        /// 转换完成后，不可再参与逻辑运算
        /// </summary>
        public float RawFloat
        {
            get
            {
                return scaledValue * 1.0f / MULTIPLIER_FACTOR;
            }
        }

        public int RawInt
        {
            get
            {
                if (scaledValue >= 0)
                {
                    return (int)(scaledValue >> BIT_MOVE_COUNT);
                }
                else
                {
                    return -(int)(-scaledValue >> BIT_MOVE_COUNT);
                }
            }
        }
        public FXInt Abs
        {
            get
            {
                FXInt vInt = this;
                if (vInt >= 0)
                {
                    return vInt;
                }
                else
                {
                    return -vInt;
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            FXInt vInt = (FXInt)obj;
            return scaledValue == vInt.scaledValue;
        }

        public override int GetHashCode()
        {
            return scaledValue.GetHashCode();
        }

        public override string ToString()
        {
            return RawFloat.ToString();
        }
    }
}
