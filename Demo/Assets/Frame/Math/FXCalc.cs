

using System;
using System.Collections.Generic;
using System.Text;

namespace LockStepFrame
{
    public class FXCalc
    {

        public static FXInt Sqrt(FXInt value, int interatorCount = 8)
        {
            if (value == FXInt.zero)
            {
                return 0;
            }
            if (value < FXInt.zero)
            {
                throw new Exception();
            }

            FXInt result = value;
            FXInt history;
            int count = 0;
            do
            {
                history = result;
                result = (result + value / result) >> 1;
                ++count;
            } while (result != history && count < interatorCount);
            return result;
        }

        public static FXArgs Acos(FXInt value)
        {
            FXInt rate = (value * AcosTable.HalfIndexCount) + AcosTable.HalfIndexCount;
            rate = Clamp(rate, FXInt.zero, AcosTable.IndexCount);
            return new FXArgs(AcosTable.table[rate.RawInt], AcosTable.Multipler);
        }


        public static FXInt Clamp(FXInt input, FXInt min, FXInt max)
        {
            if (input < min)
            {
                return min;
            }
            if (input > max)
            {
                return max;
            }
            return input;
        }
    }
}
