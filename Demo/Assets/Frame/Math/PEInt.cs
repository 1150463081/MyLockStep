using System;

namespace PEMath
{
    [Serializable]
    public struct PEInt
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

        public static readonly PEInt zero = new PEInt(0);
        public static readonly PEInt one = new PEInt(1);


        #region ���캯��
        //�ڲ�ʹ�ã��Ѿ�������ɵ�����
        private PEInt(long scaledValue)
        {
            this.scaledValue = scaledValue;
        }
        public PEInt(int val)
        {
            scaledValue = val * MULTIPLIER_FACTOR;
        }
        public PEInt(float val)
        {
            scaledValue = (long)Math.Round(val * MULTIPLIER_FACTOR);
        }
        //float��ʧ���ȣ�������ʽת��
        public static explicit operator PEInt(float f)
        {
            return new PEInt((long)Math.Round(f * MULTIPLIER_FACTOR));
        }
        //int����ʧ���ȣ�������ʽת��
        public static implicit operator PEInt(int i)
        {
            return new PEInt(i);
        }
        #endregion

        #region �����
        //�ӣ������ˣ�����ȡ��
        public static PEInt operator +(PEInt a, PEInt b)
        {
            return new PEInt(a.scaledValue + b.scaledValue);
        }
        public static PEInt operator -(PEInt a, PEInt b)
        {
            return new PEInt(a.scaledValue - b.scaledValue);
        }
        public static PEInt operator *(PEInt a, PEInt b)
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
            return new PEInt(value);
        }
        public static PEInt operator /(PEInt a, PEInt b)
        {
            if (b.scaledValue == 0)
            {
                throw new Exception();
            }
            return new PEInt((a.scaledValue << BIT_MOVE_COUNT) / b.scaledValue);
        }
        public static PEInt operator -(PEInt value)
        {
            return new PEInt(-value.scaledValue);
        }
        public static bool operator ==(PEInt a, PEInt b)
        {
            return a.scaledValue == b.scaledValue;
        }
        public static bool operator !=(PEInt a, PEInt b)
        {
            return a.scaledValue != b.scaledValue;
        }
        public static bool operator >(PEInt a, PEInt b)
        {
            return a.scaledValue > b.scaledValue;
        }
        public static bool operator <(PEInt a, PEInt b)
        {
            return a.scaledValue < b.scaledValue;
        }
        public static bool operator >=(PEInt a, PEInt b)
        {
            return a.scaledValue >= b.scaledValue;
        }
        public static bool operator <=(PEInt a, PEInt b)
        {
            return a.scaledValue <= b.scaledValue;
        }

        public static PEInt operator >>(PEInt value, int moveCount)
        {
            if (value.scaledValue >= 0)
            {
                return new PEInt(value.scaledValue >> moveCount);
            }
            else
            {
                return new PEInt(-(-value.scaledValue >> moveCount));
            }
        }
        public static PEInt operator <<(PEInt value, int moveCount)
        {
            return new PEInt(value.scaledValue << moveCount);
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
        public PEInt Abs
        {
            get
            {
                PEInt vInt = this;
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
            PEInt vInt = (PEInt)obj;
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
