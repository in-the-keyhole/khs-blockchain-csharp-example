﻿using System;
using System.Diagnostics;

/// <summary>
/// Provides an implementation to get a highe rresolution timestamp
/// DateTime.UtcNow is accurate to 15.625
/// </summary>
/// <summary>
/// This class provides a high resolution clock by using the new API available in <c>Windows 8</c>/ 
/// <c>Windows Server 2012</c> and higher. In all other operating systems it returns time by using 
/// a manually tuned and compensated <c>DateTime</c> which takes advantage of the high resolution
/// available in <see cref="Stopwatch"/>.
/// </summary>
public sealed class Clock : IDisposable
{
    private readonly long _maxIdleTime = TimeSpan.FromSeconds(10).Ticks;
    private const long TicksMultiplier = 1000 * TimeSpan.TicksPerMillisecond;

    private readonly ThreadLocal<DateTime> _startTime =
        new ThreadLocal<DateTime>(() => DateTime.UtcNow, false);

    private readonly ThreadLocal<double> _startTimestamp =
        new ThreadLocal<double>(() => Stopwatch.GetTimestamp(), false);

    private static long _max;
    private static object _locker = new object();

    [DllImport("Kernel32.dll", CallingConvention = CallingConvention.Winapi)]
    private static extern void GetSystemTimePreciseAsFileTime(out long filetime);

    /// <summary>
    /// Creates an instance of the <see cref="Clock"/>.
    /// </summary>
    public Clock()
    {
        try
        {
            long preciseTime;
            GetSystemTimePreciseAsFileTime(out preciseTime);
            IsPrecise = true;
        }
        catch (EntryPointNotFoundException)
        {
            IsPrecise = false;
        }
    }

    /// <summary>
    /// Gets the flag indicating whether the instance of <see cref="Clock"/> provides high resolution time.
    /// <remarks>
    /// <para>
    /// This only returns <c>True</c> on <c>Windows 8</c>/<c>Windows Server 2012</c> and higher.
    /// </para>
    /// </remarks>
    /// </summary>
    public bool IsPrecise { get; }

    /// <summary>
    /// Gets the date and time in <c>UTC</c>.
    /// </summary>
    public DateTime UtcNow
    {
        get
        {
            if (IsPrecise)
            {
                long preciseTime = Stopwatch.GetTimestamp();

                lock (_locker)
                {
                    if (preciseTime <= _max)
                    {
                        _max++;
                        preciseTime = _max;
                    }
                    else
                    {
                        _max = preciseTime;
                    }
                }

                return DateTime.FromFileTimeUtc(preciseTime);
            }

            double endTimestamp = Stopwatch.GetTimestamp();

            var durationInTicks = (endTimestamp - _startTimestamp.Value) / Stopwatch.Frequency * TicksMultiplier;


            if (durationInTicks >= _maxIdleTime)
            {
                _startTimestamp.Value = Stopwatch.GetTimestamp();
                _startTime.Value = DateTime.UtcNow;
                return _startTime.Value;
            }

            return _startTime.Value.AddTicks((long)durationInTicks);
        }
    }

    /// <summary>
    /// Releases all resources used by the instance of <see cref="Clock"/>.
    /// </summary>
    public void Dispose()
    {
        _startTime.Dispose();
        _startTimestamp.Dispose();
    }
}