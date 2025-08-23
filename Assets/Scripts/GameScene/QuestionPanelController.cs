using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestionPanelController : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public Image questionImage;
    public Button continueButton;

    public DynamicAssertionController[] assertionControllers = new DynamicAssertionController[4];

    private UnityAction<AssertionController[]> continueAction;

    void Start()
    {
        continueButton.onClick.AddListener(() =>
        {
            continueAction(assertionControllers);
            gameObject.SetActive(false);
        });
    }

    public void SetQuestion(GameQuestion gameQuestion)
    {
        Question question = gameQuestion.question;
        questionText.SetText(question.question);

        if (question.image != null)
        {
            questionImage.sprite = Resources.Load<Sprite>("images/" + question.image);
            questionImage.gameObject.SetActive(true);
        } 
        else
        {
            questionImage.gameObject.SetActive(false);
        }

        for (int i = 0; i < assertionControllers.Length; i++)
        {
            assertionControllers[i].SetAssertionForm(gameQuestion.assertions[i].assertionForm);
        }
    }

    public void SetContinueAction(UnityAction<AssertionController[]> action)
    {
        continueAction = action; 
    }
}
