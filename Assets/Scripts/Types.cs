using System.Collections.Generic;

public class QuestionData
{
    public Dictionary<int, Question[]> data;
}

public class Question
{
    public string question;
    public string correctAnswer;
    public string[] wrongAnswers;
}
