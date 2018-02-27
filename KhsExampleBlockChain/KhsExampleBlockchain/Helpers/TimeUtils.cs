using System;
using System.Collections.Generic;
using System.Text;

namespace KhsExampleBlockchain.Helpers
{

    public class TimeUtils
    {
        //Another overload to help the Java and C# implementations match more closely.
        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }
    }
}
