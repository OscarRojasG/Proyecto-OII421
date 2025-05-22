using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PauseScreenController : MonoBehaviour
{
    public Button continueButton;
    public Button exitButton;
    public GameObject pauseScreen;

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
            // No actualizamos datos, ya que no se completó el nivel o se perdió.

            Time.timeScale = 1f; // Reanudar el tiempo
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
        // Time.timeScale = 0f;
        StartCoroutine(ShowPause());
    }

    IEnumerator ShowPause() {
        yield return new WaitForEndOfFrame();
        pauseScreen.SetActive(true);
    }
    public void Hide()
    {
        // Time.timeScale = 1f;
        pauseScreen.SetActive(false);
        Canvas.ForceUpdateCanvases();
    }
}
