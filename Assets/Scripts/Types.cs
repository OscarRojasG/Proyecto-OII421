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
    public string image;
    public string feedback;
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

[Serializable]
public class QuestionDataNew
{
    public Dictionary<string, QuestionNew[]> data;
}

[Serializable]
public class QuestionNew
{
    public string question;
    public string concept;
    public string image;
    public Assertion[] assertions;
}

[Serializable]
public class Assertion
{
    public string subconcept;
    public string feedbackText;
    public string feedbackImage;
    public AssertionForm[] forms;
}

[Serializable]
public class AssertionForm
{
    public string statement;
    public bool answer;
}