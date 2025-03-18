using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PauseScreenController : MonoBehaviour
{
    public Button continueButton;
    public Button exitButton;

    private UnityAction continueAction;

    void Start()
    {
        Hide();

        continueButton.onClick.AddListener(() =>
        {
            continueAction();
        });

        exitButton.onClick.AddListener(() =>
        {
            SceneController.Instance.ChangeScene("MainScene");
            SceneController.Instance.ClearHistory();
        });
    }

    public void SetContinueAction(UnityAction action)
    {
        continueAction = action;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
