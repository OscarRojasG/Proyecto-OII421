using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FeedbackPanelController : MonoBehaviour
{
    public TextMeshProUGUI correctAnswerText;
    public TextMeshProUGUI playerAnswerText;
    public TextMeshProUGUI feedbackText;
    public Button continueButton;

    private UnityAction continueButtonAction;

    void Start()
    {
        continueButton.onClick.AddListener(() =>
        {
            continueButtonAction();
            Destroy(gameObject);
        });
    }

    public void SetContinueAction(UnityAction action)
    {
        continueButtonAction = action;
    }

    public void SetCorrectAnswer(string correctAnswer)
    {
        correctAnswerText.SetText(correctAnswer);
    }

    public void SetPlayerAnswer(string playerAnswer)
    {
        playerAnswerText.SetText(playerAnswer);
    }

    public void SetFeedback(string feedback)
    {
        feedbackText.SetText(feedback);
    }
}
