using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestionPanelController : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public Image questionImage;

    public OptionController optionA;
    public OptionController optionB;
    public OptionController optionC;
    public OptionController optionD;

    private UnityAction<OptionController> correctOptionAction;
    private UnityAction<OptionController> wrongOptionAction;
    private UnityAction dismissAction = (() => {});

    public void SetQuestion(Question question)
    {
        questionText.SetText(question.question);

        if (question.image != null)
        {
            questionImage.sprite = Resources.Load<Sprite>("images/" + question.image);
            questionImage.gameObject.SetActive(true);
        }

        OptionController[] options = new OptionController[] { optionA, optionB, optionC, optionD };
        int correctOptionIndex = Random.Range(0, 4);

        string[] wrongAnswers = (string[]) question.wrongAnswers.Clone();
        Util.Shuffle(question.wrongAnswers);
        int currentWrongAnswer = 0;

        for (int i = 0; i < options.Length; i++)
        {
            if (i == correctOptionIndex)
            {
                options[i].SetOnClickAction((OptionController optionController) =>
                {
                    correctOptionAction(optionController);
                    Dismiss();
                });

                options[i].SetText(question.correctAnswer);
            }
            else
            {
                options[i].SetOnClickAction((OptionController optionController) =>
                {
                    wrongOptionAction(optionController);
                    Dismiss();
                });

                options[i].SetText(question.wrongAnswers[currentWrongAnswer]);
                currentWrongAnswer++;
            }
        }
    }

    public void SetCorrectOptionAction(UnityAction<OptionController> action)
    {
        correctOptionAction = action;
    }

    public void SetWrongOptionAction(UnityAction<OptionController> action)
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
