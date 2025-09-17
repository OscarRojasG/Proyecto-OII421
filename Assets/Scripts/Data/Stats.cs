public class Stats
{
    public int distance;
    public int correctCount;
    public int errorCount;
    public int collectedObjects;

    public int GetTotalAssertions()
    {
        return errorCount + correctCount;
    }

    public int GetCorrectPercentage()
    {
        if (GetTotalAssertions() != 0)
            return (int)(((float)correctCount / GetTotalAssertions()) * 100);
        return 0;
    }

    public int GetErrorPercentage()
    {
        if (GetTotalAssertions() != 0)
            return (int)(((float)errorCount / GetTotalAssertions()) * 100);
        return 0;
    }
}