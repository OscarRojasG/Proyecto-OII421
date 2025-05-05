using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AssertionController : MonoBehaviour
{
    public Image iconImage;
    public Button iconButton;
    public TextMeshProUGUI statement;

    private bool playerAnswer = false;
    private AssertionForm assertionForm;

    private Sprite spriteTrue;
    private Sprite spriteFalse;

    void Start()
    {
        spriteTrue = Resources.Load<Sprite>("icons/test_tube_true");
        spriteFalse = Resources.Load<Sprite>("icons/test_tube_false");


        iconButton.onClick.AddListener(() =>
        {
            playerAnswer = !playerAnswer;
            changeIcon();
        });

        changeIcon();
    }

    private void changeIcon()
    {
        if (playerAnswer)
        {
            iconImage.sprite = spriteTrue;
        }
        else
        {
            iconImage.sprite = spriteFalse;
        }
    }

    public void SetAssertionForm(AssertionForm assertionForm)
    {
        this.assertionForm = assertionForm;
        statement.SetText(assertionForm.statement);
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
