

using System;
using System.Collections.Generic;
using System.Text;

namespace LockStepFrame
{
    public class PECalc
    {

        public static PEInt Sqrt(PEInt value, int interatorCount = 8)
        {
            if (value == PEInt.zero)
            {
                return 0;
            }
            if (value < PEInt.zero)
            {
                throw new Exception();
            }

            PEInt result = value;
            PEInt history;
            int count = 0;
            do
            {
                history = result;
                result = (result + value / result) >> 1;
                ++count;
            } while (result != history && count < interatorCount);
            return result;
        }

        public static PEArgs Acos(PEInt value)
        {
            PEInt rate = (value * AcosTable.HalfIndexCount) + AcosTable.HalfIndexCount;
            rate = Clamp(rate, PEInt.zero, AcosTable.IndexCount);
            return new PEArgs(AcosTable.table[rate.RawInt], AcosTable.Multipler);
        }


        public static PEInt Clamp(PEInt input, PEInt min, PEInt max)
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
