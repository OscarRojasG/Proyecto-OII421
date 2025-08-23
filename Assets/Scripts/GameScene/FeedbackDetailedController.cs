using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;

public class FeedbackDetailedController : MonoBehaviour
{
    public TextMeshProUGUI assertionText;
    public TextMeshProUGUI feedbackText;
    public Image feedbackImage;

    public Button backButton;

    public TextMeshProUGUI correctAnswerText;
    public Image correctAnswerFrame;

    private UnityAction backButtonAction;

    private Color colorTrue = new Color(171f / 255f, 229f / 255f, 180f / 255f);
    private Color colorFalse = new Color(255f / 255f, 179f / 255f, 179f / 255f);

    void Start()
    {
        backButton.onClick.AddListener(() =>
        {
            backButtonAction();
            gameObject.SetActive(false);
        });
    }

    public void SetFeedback(FeedbackAssertion feedbackAssertion)
    {
        assertionText.SetText(feedbackAssertion.assertionForm.statement);
        feedbackText.SetText(feedbackAssertion.feedbackText);

        if (feedbackAssertion.feedbackImage != null)
        {
            feedbackImage.gameObject.SetActive(true);
            feedbackImage.sprite = Resources.Load<Sprite>("images/" + feedbackAssertion.feedbackImage);
        }
        else
        {
            feedbackImage.gameObject.SetActive(false);
        }

        if (feedbackAssertion.assertionForm.answer)
        {
            correctAnswerFrame.color = colorTrue;
            correctAnswerText.SetText("Verdadero");
        }
        else
        {
            correctAnswerFrame.color = colorFalse;
            correctAnswerText.SetText("Falso");
        }

    }

    public void SetBackButtonAction(UnityAction action)
    {
        backButtonAction = action;
    }
}
