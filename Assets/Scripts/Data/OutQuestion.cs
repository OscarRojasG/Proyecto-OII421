
using System;

[Serializable]
// Equivalente al QuestionRun
public class OutQuestion
{
    public int questionId;
    public float answerTime;
    public OutAssertion[] assertions;
}