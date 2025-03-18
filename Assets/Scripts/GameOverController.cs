using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    public Button retryButton;
    public Button exitButton;

    void Start()
    {
        retryButton.onClick.AddListener(() =>
        {
            SceneController.Instance.PreviousScene();
        });

        exitButton.onClick.AddListener(() =>
        {
            SceneController.Instance.ChangeScene("MainScene");
            SceneController.Instance.ClearHistory();
        });
    }
}
