using System;
using System.Collections.Generic;


// /*
[Serializable]
public class ProgressData
{
    public string playerID;
    public GameQuestion[] answeredQuestions;
}
// */


[Serializable]
public class QuestionData
{
    public Dictionary<string, Question[]> data;
}

[Serializable]
public class Question
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

public class GameQuestion
{
    public Question question { get; }
    public List<GameAssertion> assertions { get; }

    public GameQuestion(Question question, List<GameAssertion> assertions)
    {
        this.question = question;
        this.assertions = assertions;
    }
}

public class GameAssertion
{
    public Assertion assertion { get; }
    public AssertionForm assertionForm { get; }

    public GameAssertion(Assertion assertion, AssertionForm assertionForm)
    {
        this.assertion = assertion;
        this.assertionForm = assertionForm;
    }
}