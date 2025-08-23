using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FeedbackAssertion
{
    public AssertionForm assertionForm;
    public bool playerAnswer;
    public string feedbackText;
    public string feedbackImage;
}

public class FeedbackMainController : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public Image questionImage;
    public Button continueButton;
    public TextMeshProUGUI guessCount;

    public FeedbackDetailedController feedbackDetailedController;
    public FeedbackAssertionController[] assertionControllers = new FeedbackAssertionController[4];

    private UnityAction continueButtonAction;

    void Start()
    {
        continueButton.onClick.AddListener(() =>
        {
            continueButtonAction();
            gameObject.SetActive(false);
        });

        feedbackDetailedController.SetBackButtonAction(() =>
        {
            gameObject.SetActive(true);
        });
    }

    public void SetContinueAction(UnityAction action)
    {
        continueButtonAction = action;
    }

    public void SetAssertions(FeedbackAssertion[] feedbackAssertions)
    {
        int correctCount = 0;

        for (int i = 0; i < assertionControllers.Length; i++)
        {
            assertionControllers[i].SetAssertionForm(feedbackAssertions[i].assertionForm);

            FeedbackAssertion feedbackAssertion = feedbackAssertions[i];
            assertionControllers[i].SetRevisarAction(() => {
                feedbackDetailedController.SetFeedback(feedbackAssertion);
                gameObject.SetActive(false);
                feedbackDetailedController.gameObject.SetActive(true);
            });
            assertionControllers[i].SetPlayerAnswer(feedbackAssertions[i].playerAnswer);

            if (feedbackAssertions[i].assertionForm.answer == feedbackAssertions[i].playerAnswer)
            {
                correctCount++;
                assertionControllers[i].SetCorrect(true);
            }
            else
            {
                assertionControllers[i].SetCorrect(false);
            }
        }

        guessCount.SetText(correctCount + "/4");
    }

    public void SetQuestionText(string questionText)
    {
        this.questionText.SetText(questionText);
    }

    public void SetQuestionImage(string questionImage)
    {
        if (questionImage != null)
        {
            this.questionImage.gameObject.SetActive(true);
            this.questionImage.sprite = Resources.Load<Sprite>("images/" + questionImage);
        }
        else
        {
            this.questionImage.gameObject.SetActive(false);
        }
    }
}
