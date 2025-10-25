using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    public Button retryButton;
    public Button exitButton;
    public Image skullIcon;
    public TextMeshProUGUI textAciertos;
    public TextMeshProUGUI textFallos;
    public TextMeshProUGUI textDistancia;
    public TextMeshProUGUI textObjetos;

    private PlayerData playerData;
    private ServerAPI serverAPI;

    private bool popupActive;

    private IEnumerator SendData()
    {
        bool done = false;
        string error = null;
        PopupManager.LoadingShow();
        serverAPI.UploadRun(
            playerData.Data.playerId,
            playerData.RunData,
            onSuccess: _ =>
            {
                PopupManager.LoadingHide();

                popupActive = true;
                PopupManager.Show(
                    "Nivel cargado con éxito al servidor", () =>
                    {
                        done = true;
                        popupActive = false;
                    });
            },
            onError: err =>
            {
                error = err;
                done = true;

                // Store RunData in persistent queue TODO
                UploadQueue.Enqueue(playerData.RunData);
            }
        );

        // Wait until serverAPI finishes
        while (!done) yield return null;

        // If there was an error, show popup and wait until accepted
        if (error != null)
        {
            bool accepted = false;

            popupActive = true;
            PopupManager.Show(
                "Could not reach server. Your progress will be uploaded later.",
                onOk: () => {
                    accepted = true;
                    popupActive = false;
                }
            );

            while (!accepted) yield return null; // wait until OK is clicked
        }
    }

    private IEnumerator Retry()
    {
        yield return SendData();
        SceneController.Instance.PreviousScene();
    }

    private IEnumerator ExitGame()
    {
        yield return SendData();
        SceneController.Instance.ChangeScene("MainScene");
        SceneController.Instance.ClearHistory();
    }

    private void Start()
    {
        serverAPI = gameObject.AddComponent<ServerAPI>();
        retryButton.onClick.AddListener(() => {
            if (popupActive) return;
            StartCoroutine(Retry());
        });
        exitButton.onClick.AddListener(() =>
        {
            if (popupActive) return;
            StartCoroutine(ExitGame());
        });

        playerData = PlayerData.Instance;
        Stats stats = playerData.lastGameStats;

        textAciertos.SetText($"{stats.correctCount}/{stats.GetTotalAssertions()} ({stats.GetCorrectPercentage()}%)");
        textFallos.SetText($"{stats.errorCount}/{stats.GetTotalAssertions()} ({stats.GetErrorPercentage()}%)");
        textDistancia.SetText(stats.distance + " m.");
        textObjetos.SetText(stats.collectedObjects + "/" + GameController.Instance.GetQuestions().Count);

        playerData.RunData.distance = stats.distance;
        playerData.RunData.errors = stats.errorCount;
        playerData.RunData.correct = stats.correctCount;

        if (stats.collectedObjects == 3)
            skullIcon.enabled = false;
    }
}
