using System.Collections.Generic;
using UnityEngine;

public class QuestionManager
{
    private Dictionary<AssertionForm, int> weights = new Dictionary<AssertionForm, int>();
    private List<Question> questionsQueue = new List<Question>();
    private List<Question> questions = new List<Question>();

    public QuestionManager(List<Question> questions)
    {
        Util.Shuffle(questions);
        foreach (Question question in questions)
        {
            questionsQueue.Add(question);
            this.questions.Add(question);

            // Inicialmente asignar peso 0 a las formas
            foreach (Assertion assertion in question.assertions)
            {
                foreach (AssertionForm assertionForm in assertion.forms)
                {
                    weights.Add(assertionForm, 0);
                }
            }
        }
    }

    public GameQuestion GetQuestion()
    {
        Question question = questionsQueue[0];
        questionsQueue.RemoveAt(0);
        questionsQueue.Add(question);

        List<GameAssertion> gameAssertions = new List<GameAssertion>();

        foreach (Assertion assertion in question.assertions)
        {
            // 1. Obtener menor peso
            int minWeight = 999999;
            foreach (AssertionForm assertionForm in assertion.forms)
            {
                if (weights[assertionForm] < minWeight)
                {
                    minWeight = weights[assertionForm];
                }
            }

            // 2. Seleccionar formas con menor peso
            List<AssertionForm> assertionForms = new List<AssertionForm>();
            foreach (AssertionForm assertionForm in assertion.forms)
            {
                if (weights[assertionForm] == minWeight)
                {
                    assertionForms.Add(assertionForm);
                }
            }

            // 3. Seleccionar aleatoriamente una de las formas con menor peso
            AssertionForm gameAssertionForm = assertionForms[Random.Range(0, assertionForms.Count)];

            // 4. Incrementamos peso de la forma seleccionada
            weights[gameAssertionForm] += 1;


            GameAssertion gameAssertion = new GameAssertion(assertion, gameAssertionForm);
            gameAssertions.Add(gameAssertion);
        }

        // Desordenamos orden de afirmaciones
        Util.Shuffle(gameAssertions);

        GameQuestion gameQuestion = new GameQuestion(question, gameAssertions);
        return gameQuestion;
    }

    public void MarkQuestionAsSolved(GameQuestion gameQuestion)
    {
        questionsQueue.Remove(gameQuestion.question);
        if (questionsQueue.Count == 0)
        {
            questionsQueue = new List<Question>(questions);
        }
    }
}
