using System;
using System.Linq;
public static class RunDataConverter
{
    public static MinimalRunData ToMinimal(RunData run)
    {
        return new MinimalRunData
        {
            p = run.playerId,
            l = run.levelId,
            d = run.distance,
            j = run.jumps,
            e = run.errors,
            c = run.correct,
            cc = run.completed,
            a = run.questions?
                .SelectMany(q => q.assertions.Select(a => new object[]
                {
                    q.questionId,
                    a.assertionId,
                    a.formId,
                    q.answerTime,
                    a.correct ? 1 : 0
                }))
                .ToList()
        };
    }
}

