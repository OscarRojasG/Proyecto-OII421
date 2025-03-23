using System;
using System.Collections.Generic;
using NUnit.Framework;

[Serializable]
public class QuestionData
{
    public Dictionary<string, Question[]> data;
}

[Serializable]
public class Question
{
    public string question;
    public string correctAnswer;
    public string[] wrongAnswers;
}

[Serializable]
public class ProgressData
{
    public string playerID;
    public AnsweredQuestion[] answeredQuestions;
}

[Serializable]
public class AnsweredQuestion
{
    public Question question;
    public string playerAnswer;
    public bool isCorrect;
    public double responseTime;
    public string level;
}