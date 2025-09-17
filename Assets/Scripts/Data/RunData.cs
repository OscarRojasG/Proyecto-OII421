using System.Collections.Generic;

[System.Serializable]
public class RunData
{

    public RunData()
    {
        questions = new List<OutQuestion>();
    }
    public int playerId;
    public string levelId;

    // summary
    public int distance;
    public int jumps;
    public int errors;
    public int correct;

    public int completed;

    // optional: session tracking
    // public string runId;     // unique UUID per run
    // public float duration;   // seconds played

    // answered questions
    public List<OutQuestion> questions;
}

