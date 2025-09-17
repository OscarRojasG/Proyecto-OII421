using System.Collections.Generic;

public static class UploadQueue
{
    private static List<RunData> queue = new List<RunData>();

    public static void Enqueue(RunData run) => queue.Add(run);

    public static RunData Dequeue()
    {
        if (queue.Count == 0) return null;
        RunData r = queue[0];
        queue.RemoveAt(0);
        return r;
    }

    public static IEnumerable<RunData> GetAll() => queue;
}