namespace eBoardAPI.Consts;

public static class ClassSettingConst
{
    public static readonly TimeOnly MorningStateTime = new TimeOnly(7, 0);
    public static readonly TimeOnly AfternoonStartTime = new TimeOnly(13, 0);
    
    public const int DEFAULT_MORNING_PERIOD_COUNT = 5;
    public const int DEFAULT_AFTERNOON_PERIOD_COUNT = 4;
    
    public const int PERIOD_DURATION_MINUTES = 45;
    public const int BREAK_DURATION_MINUTES = 5;
}