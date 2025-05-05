using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;

public class FeedbackAssertion
{
    public AssertionForm assertionForm;
    public string feedbackText;
    public Sprite feedbackImageSprite;
    public Sprite playerAnswerIconSprite;
    public Sprite playerAnswerStatusSprite;
}

public class FeedbackPanelController : MonoBehaviour
{
    public TextMeshProUGUI assertionText;

    public Image playerAnswerIcon;
    public Image playerAnswerStatus;

    public Image feedbackImage;
    public TextMeshProUGUI feedbackText;

    public Button prevButton;
    public Button nextButton;
    public Button continueButton;

    private List<FeedbackAssertion> feedbackAssertions = new List<FeedbackAssertion>();
    private UnityAction continueButtonAction;
    private int currentIndex = 0;

    void Start()
    {
        continueButton.onClick.AddListener(() =>
        {
            continueButtonAction();
            Destroy(gameObject);
        });

        nextButton.onClick.AddListener(() =>
        {
            currentIndex = (currentIndex + 1) % feedbackAssertions.Count;
            UpdateView();
        });

        prevButton.onClick.AddListener(() =>
        {
            currentIndex = (currentIndex + feedbackAssertions.Count - 1) % feedbackAssertions.Count;
            UpdateView();
        });
    }

    public void SetContinueAction(UnityAction action)
    {
        continueButtonAction = action;
    }

    public void AddAssertion(AssertionForm assertionForm, bool playerAnswer, string feedbackText, string feedbackImagePath)
    {
        FeedbackAssertion feedbackAssertion = new FeedbackAssertion();
        feedbackAssertion.assertionForm = assertionForm;
        feedbackAssertion.feedbackText = feedbackText;

        if (feedbackImagePath != null)
        {
            feedbackAssertion.feedbackImageSprite = Resources.Load<Sprite>("images/" + feedbackImagePath);
        }

        if (playerAnswer)
        {
            feedbackAssertion.playerAnswerIconSprite = Resources.Load<Sprite>("icons/test_tube_true");
        }
        else
        {
            feedbackAssertion.playerAnswerIconSprite = Resources.Load<Sprite>("icons/test_tube_false");
        }

        if (playerAnswer == assertionForm.answer)
        {
            feedbackAssertion.playerAnswerStatusSprite = Resources.Load<Sprite>("icons/check_mark");
        }
        else
        {
            feedbackAssertion.playerAnswerStatusSprite = Resources.Load<Sprite>("icons/cross_mark");
        }

        feedbackAssertions.Add(feedbackAssertion);
        UpdateView();
    }

    private void UpdateView()
    {
        FeedbackAssertion feedbackAssertion = feedbackAssertions[currentIndex];

        assertionText.text = feedbackAssertion.assertionForm.statement;
        feedbackText.text = feedbackAssertion.feedbackText;
        playerAnswerIcon.sprite = feedbackAssertion.playerAnswerIconSprite;
        playerAnswerStatus.sprite = feedbackAssertion.playerAnswerStatusSprite;

        if (feedbackAssertion.feedbackImageSprite == null)
        {
            feedbackImage.gameObject.SetActive(false);
        }
        else
        {
            feedbackImage.sprite = feedbackAssertion.feedbackImageSprite;
            feedbackImage.gameObject.SetActive(true);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }
}
