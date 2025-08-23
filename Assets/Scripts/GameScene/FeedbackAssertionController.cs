using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FeedbackAssertionController : AssertionController
{
    public Image backgroundPanel;
    public Button revisarButton;

    private UnityAction revisarAction;

    private Color colorCorrect = new Color(171f / 255f, 229f / 255f, 180f / 255f);
    private Color colorWrong = new Color(255f / 255f, 179f / 255f, 179f / 255f);

    void Start()
    {
        revisarButton.onClick.AddListener(() =>
        {
            revisarAction();
        });
    }

    public void SetCorrect(bool isCorrect)
    {
        if (isCorrect)
        {
            backgroundPanel.color = colorCorrect;
        }
        else
        {
            backgroundPanel.color = colorWrong;
        }
    }

    public void SetRevisarAction(UnityAction revisarAction)
    {
        this.revisarAction = revisarAction;
    }
}