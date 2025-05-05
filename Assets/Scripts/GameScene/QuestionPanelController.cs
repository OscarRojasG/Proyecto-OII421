using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestionPanelController : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public Image questionImage;
    public Button continueButton;

    public AssertionController[] assertionControllers = new AssertionController[4];

    private UnityAction<AssertionController[]> continueAction;

    void Start()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        continueButton.onClick.AddListener(() =>
        {
            continueAction(assertionControllers);
            Destroy(gameObject);
        });
    }

    public void SetQuestion(QuestionNew question, int formIndex)
    {
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
            assertionControllers[i].SetAssertionForm(question.assertions[i].forms[formIndex]);
        }
    }

    public void SetContinueAction(UnityAction<AssertionController[]> action)
    {
        continueAction = action; 
    }
}
