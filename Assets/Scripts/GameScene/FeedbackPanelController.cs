using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;


public class FeedbackPanelController : MonoBehaviour
{
    public TextMeshProUGUI assertionText;
    public Image panelImage;

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

    private Sprite playerAnswerTrueSprite;
    private Sprite playerAnswerFalseSprite;
    private Sprite playerAnswerCorrectSprite;
    private Sprite playerAnswerWrongSprite;

    public Image imageButton;

    private Color panelColorCorrect = new Color(204f / 255f, 250f / 255f, 209f / 255f);
    private Color panelColorWrong = new Color(255f / 255f, 191f / 255f, 196f / 255f);

    private Color buttonColorCorrect = new Color(122f / 255f, 236f / 255f, 115f / 255f);
    private Color buttonColorWrong = new Color(248f / 255f, 144f / 255f, 151f / 255f);

    void Awake()
    {
        playerAnswerTrueSprite = Resources.Load<Sprite>("icons/test_tube_true");
        playerAnswerFalseSprite = Resources.Load<Sprite>("icons/test_tube_false");
        playerAnswerCorrectSprite = Resources.Load<Sprite>("icons/check_mark");
        playerAnswerWrongSprite = Resources.Load<Sprite>("icons/cross_mark");
    }

    void Start()
    {
        imageButton = continueButton.gameObject.GetComponent<Image>();

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
        feedbackAssertion.playerAnswer = playerAnswer;
        feedbackAssertion.feedbackText = feedbackText;

        if (feedbackImagePath != null)
        {
            feedbackAssertion.feedbackImage = null;
        }

        feedbackAssertions.Add(feedbackAssertion);
        UpdateView();
    }

    private void UpdateView()
    {
        FeedbackAssertion feedbackAssertion = feedbackAssertions[currentIndex];

        assertionText.text = feedbackAssertion.assertionForm.statement;
        feedbackText.text = feedbackAssertion.feedbackText;

        if (feedbackAssertion.playerAnswer)
        {
            playerAnswerIcon.sprite = playerAnswerTrueSprite;
        }
        else
        {
            playerAnswerIcon.sprite = playerAnswerFalseSprite;
        }

        if (feedbackAssertion.playerAnswer == feedbackAssertion.assertionForm.answer)
        {
            playerAnswerStatus.sprite = playerAnswerCorrectSprite;
            panelImage.color = panelColorCorrect;
            imageButton.color = buttonColorCorrect;
        }
        else
        {
            playerAnswerStatus.sprite = playerAnswerWrongSprite;
            panelImage.color = panelColorWrong;
            imageButton.color = buttonColorWrong;
        }

        if (feedbackAssertion.feedbackImage == null)
        {
            feedbackImage.gameObject.SetActive(false);
        }
        else
        {
            feedbackImage.gameObject.SetActive(true);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }
}
