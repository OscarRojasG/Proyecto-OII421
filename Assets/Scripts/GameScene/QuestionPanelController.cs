using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestionPanelController : MonoBehaviour
{
    public TextMeshProUGUI questionText;

    public Button optionA;
    public Button optionB;
    public Button optionC;
    public Button optionD;

    private UnityAction correctOptionAction = (() => {});
    private UnityAction wrongOptionAction = (() => {});
    private UnityAction dismissAction = (() => {});

    public void SetQuestion(Question question)
    {
        questionText.SetText(question.question);

        Button[] options = new Button[] { optionA, optionB, optionC, optionD };
        int correctOptionIndex = Random.Range(0, 4);

        string[] wrongAnswers = (string[]) question.wrongAnswers.Clone();
        Util.Shuffle(question.wrongAnswers);
        int nextWrongAnswer = 0;

        for (int i = 0; i < options.Length; i++)
        {
            TextMeshProUGUI optionText = options[i].GetComponentInChildren<TextMeshProUGUI>();

            if (i == correctOptionIndex)
            {
                options[i].onClick.AddListener(() =>
                {
                    correctOptionAction();
                    Dismiss();
                });

                optionText.SetText(question.correctAnswer);
            }
            else
            {
                options[i].onClick.AddListener(() =>
                {
                    wrongOptionAction();
                    Dismiss();
                });

                optionText.SetText(wrongAnswers[nextWrongAnswer]);
                nextWrongAnswer++;
            }
        }
    }

    public void SetCorrectOptionAction(UnityAction action)
    {
        correctOptionAction = action;
    }

    public void SetWrongOptionAction(UnityAction action)
    {
        wrongOptionAction = action;
    }

    public void SetDismissAction(UnityAction action)
    {
        dismissAction = action;
    }

    public void Dismiss()
    {
        dismissAction();
        Destroy(gameObject);
    }
}
