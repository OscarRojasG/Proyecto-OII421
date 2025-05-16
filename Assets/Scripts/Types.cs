using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;


public class QuestionT : Question
{
    private int Nivel { get; set; }
    public int Id { get; private set; }
    private List<AssertionT> assertionsT;
    public QuestionT(int Nivel, int Id, Question q)
    {
        this.question = q.question; this.concept = q.concept; this.image = q.image; this.Id = Id;
        this.Nivel = Nivel; this.Id = Id;

        assertionsT = new List<AssertionT>();
        int a_id = 0;
        foreach (Assertion a in q.assertions)
        {
            assertionsT.Add(new AssertionT(a_id, a));
            a_id++;
        }
        this.assertions = assertionsT.ToArray();
    }
    static public List<QuestionT> FromQuestionList(Question[] qs, int level)
    {
        List<QuestionT> d = new List<QuestionT>();
        int q_id = 0;
        foreach (Question q in qs)
        {
            d.Add(new QuestionT(level, q_id, q));
            q_id++;
        }
        return d;
    }
}

public class AssertionFormT : AssertionForm
{
    public int Id { get; set; }
    public AssertionFormT(int id, AssertionForm af) 
    {
        this.Id = id; this.statement = af.statement; this.answer = af.answer;
    }
} 

public class AssertionT : Assertion
{
    public int Id;
    public AssertionT(int id, Assertion a)
    {
        this.Id = id;
        this.subconcept = a.subconcept;
        this.feedbackImage = a.feedbackImage;
        this.feedbackText = a.feedbackText;
        var formsT = new List<AssertionFormT>();

        int af_id = 0;
        foreach (AssertionForm af in a.forms) {
            formsT.Add(new AssertionFormT(af_id, af));
            af_id++;
        }

        this.forms = formsT.ToArray();
    }
}


[Serializable]
public class OutAssertion
{
    public int assertionId;
    public int formId;
    public bool correct;
}

public class OutQuestionT {
    public static OutQuestion FromGameQuestion(GameQuestion q)
    {
        OutQuestion oq = new OutQuestion
        {
            questionId = q.question.Id
        };
        var oas = new List<OutAssertion>();
        foreach (GameAssertion ga in q.assertions)
        {
            OutAssertion a = new OutAssertion();
            a.assertionId = ga.assertion.Id;
            a.formId = ga.assertionForm.Id;
            oas.Add(a);
        }
        oq.assertions = oas.ToArray();
        return oq;
    }
    
}

[Serializable]
public class OutQuestion
{
    public int questionId;
    public float answerTime;
    public OutAssertion[] assertions;
}

// /*
[Serializable]
public class ProgressData
{
    public string playerID;
    public OutQuestion[] answeredQuestions;
}
// */


[Serializable]
public class PlayerResponse
{
    public int levelID;
    public int questionID;
    public int[] assertionsID;
}

// Recorre los niveles
[Serializable]
public class QuestionData
{
    public Dictionary<string, Question[]> data;
}

// Tiene la pregunta y las afirmaciones (y formas)
[Serializable]
public class Question
{
    public string question;
    public string concept;
    public string image;
    public Assertion[] assertions;

    public static explicit operator Question(List<QuestionT> v)
    {
        throw new NotImplementedException();
    }
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

// pregunta que le salio al usuario en el nivel
public class GameQuestion
{
    public QuestionT question { get; }
    public List<GameAssertion> assertions { get; }

    public GameQuestion(QuestionT question, List<GameAssertion> assertions)
    {
        this.question = question;
        this.assertions = assertions;
    }
}

public class GameAssertion
{
    public AssertionT assertion { get; }
    public AssertionFormT assertionForm { get; }

    public GameAssertion(AssertionT assertion, AssertionFormT assertionForm)
    {
        this.assertion = assertion;
        this.assertionForm = assertionForm;
    }
}