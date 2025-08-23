using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AssertionController : MonoBehaviour
{
    public Image iconImage;
    public Button iconButton;
    public TextMeshProUGUI statement;

    private bool playerAnswer = false;
    private AssertionForm assertionForm;

    public void SetPlayerAnswer(bool playerAnswer)
    {
        this.playerAnswer = playerAnswer;
        if (playerAnswer)
        {
            iconImage.sprite = Resources.Load<Sprite>("icons/test_tube_true");
        }
        else
        {
            iconImage.sprite = Resources.Load<Sprite>("icons/test_tube_false");
        }
    }

    public void SetAssertionForm(AssertionForm assertionForm)
    {
        this.assertionForm = assertionForm;
        statement.SetText(assertionForm.statement);
        SetPlayerAnswer(false);
    }

    public AssertionForm GetAssertion()
    {
        return assertionForm;
    }

    public bool GetPlayerAnswer()
    {
        return playerAnswer;
    }
}
