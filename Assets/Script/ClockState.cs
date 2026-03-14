public static class ClockState
{
    public static bool HasValue { get; private set; } = false;

    public static int Hour { get; private set; } = 0;
    public static int Minute { get; private set; } = 0;

    // ✅ czy puzzle zostało rozwiązane
    public static bool IsSolved { get; private set; } = false;

    public static void Set(int hour, int minute)
    {
        if (hour < 0) hour = 0;
        if (hour > 23) hour = 23;
        if (minute < 0) minute = 0;
        if (minute > 59) minute = 59;

        Hour = hour;
        Minute = minute;
        HasValue = true;
    }

    public static void MarkSolved()
    {
        IsSolved = true;
    }
}