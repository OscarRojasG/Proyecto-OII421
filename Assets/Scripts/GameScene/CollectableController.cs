using UnityEngine;

public class CollectableController : ObjectController
{
    private GameQuestion gameQuestion;

    public void SetGameQuestion(GameQuestion gameQuestion)
    {
        this.gameQuestion = gameQuestion;
    }

    public GameQuestion GetGameQuestion()
    {
        return gameQuestion;
    }
}
