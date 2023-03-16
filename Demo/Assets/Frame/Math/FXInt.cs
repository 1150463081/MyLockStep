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
        //��λ����
        const int BIT_MOVE_COUNT = 10;
        const long MULTIPLIER_FACTOR = 1 << BIT_MOVE_COUNT;

        public static readonly FXInt zero = new FXInt(0);
        public static readonly FXInt one = new FXInt(1);


        #region ���캯��
        //�ڲ�ʹ�ã��Ѿ�������ɵ�����
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
        //float��ʧ���ȣ�������ʽת��
        public static explicit operator FXInt(float f)
        {
            return new FXInt((long)Math.Round(f * MULTIPLIER_FACTOR));
        }
        //int����ʧ���ȣ�������ʽת��
        public static implicit operator FXInt(int i)
        {
            return new FXInt(i);
        }
        #endregion

        #region �����
        //�ӣ������ˣ�����ȡ��
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
        /// ת����ɺ󣬲����ٲ����߼�����
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
