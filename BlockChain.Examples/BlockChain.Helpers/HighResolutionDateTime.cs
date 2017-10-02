using System;
using System.Diagnostics;

public static class HighResolutionDateTime
{   
    public static DateTime UtcNow
    {
        get
        {   
            long timeStamp = Stopwatch.GetTimestamp();

            return DateTime.FromFileTimeUtc(timeStamp);
        }
    }
}