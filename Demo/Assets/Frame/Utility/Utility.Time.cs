using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockStepFrame
{
    public partial class Utility
    {
        public class Time
        {
            readonly static DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            readonly static long epochTicks = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).Ticks;
            readonly static long secDivisor = (long)Math.Pow(10, 7);
            readonly static long msecDivisor = (long)Math.Pow(10, 4);
            /// <summary>
            /// 获取秒级别UTC时间戳
            /// </summary>
            /// <returns>秒级别时间戳</returns>
            public static long SecondTimeStamp()
            {
                return (DateTime.UtcNow.Ticks - epochTicks) / secDivisor;
            }
            /// <summary>
            /// 获取毫秒级别的UTC时间戳
            /// </summary>
            /// <returns>毫秒级别时间戳</returns>
            public static long MillisecondTimeStamp()
            {
                return (DateTime.UtcNow.Ticks - epochTicks) / msecDivisor;
            }
            /// <summary>
            /// 获取微秒级别UTC时间戳
            /// </summary>
            /// <returns>微秒级别UTC时间戳</returns>
            public static long MicrosecondTimeStamp()
            {
                return DateTime.UtcNow.Ticks - epochTicks;
            }
            /// <summary>
            ///秒级别UTC时间
            /// </summary>
            /// <returns>long时间</returns>
            public static long SecondNow()
            {
                return DateTime.UtcNow.Ticks / secDivisor;
            }
            /// <summary>
            ///毫秒别UTC时间
            /// </summary>
            /// <returns>long时间</returns>
            public static long MillisecondNow()
            {
                return DateTime.UtcNow.Ticks / msecDivisor;
            }
            /// <summary>
            ///微秒级别UTC时间
            /// </summary>
            /// <returns>long时间</returns>
            public static long MicrosecondNow()
            {
                return DateTime.UtcNow.Ticks;
            }
            /// <summary>
            /// 时间戳转UTC DateTime
            /// </summary>
            /// <returns>UTC DateTime</returns>
            public static DateTime TimestampToDateTime(long ts)
            {
                int length = (int)Math.Floor(Math.Log10(ts));
                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                if (length == 9)
                    return dateTime.AddSeconds(ts);
                if (length == 12)
                    return dateTime.AddMilliseconds(ts);
                if (length == 16)
                    return dateTime.AddTicks(ts);
                return DateTime.MinValue;
            }
        }
    }
}
