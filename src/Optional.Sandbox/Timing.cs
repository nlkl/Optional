namespace Optional.Sandbox
{
    using System;
    using System.Diagnostics;

    public static class Timing
    {
        public static double RunningTimeInMs<TResult>(Func<TResult> action, int count)
        {
            return RunningTimeInMs(() => { var x = action(); }, count);
        }

        public static double RunningTimeInMs(Action action, int count)
        {
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
            {
                action();
            }
            var ticks = sw.ElapsedTicks;

            return 1000.0 * ticks / ((double)Stopwatch.Frequency * count);
        }
    }
}
