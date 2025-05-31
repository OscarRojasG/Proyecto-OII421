using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    public Button retryButton;
    public Button exitButton;
    private PlayerData playerData;

    public Image skullIcon;
    public TextMeshProUGUI textAciertos;
    public TextMeshProUGUI textFallos;
    public TextMeshProUGUI textDistancia;
    public TextMeshProUGUI textObjetos;

    IEnumerator SendData()
    {
        Debug.Log("Exiting game...");

        Canvas canvas = GameObject.Find("PopupCanvas").GetComponent<Canvas>();
        GameObject popup = Instantiate(Resources.Load("Prefabs/CargandoDatosPopup")) as GameObject;

        popup.transform.SetParent(canvas.transform, false);
        RectTransform popupRect = popup.GetComponent<RectTransform>();
        popupRect.anchoredPosition3D = Vector3.zero;

        Destroy(popup, 3f);

        // ⏳ Wait until data is actually sent
        yield return StartCoroutine(playerData.SendDataCoroutine());
    }

    IEnumerator Retry()
    {
        yield return SendData();

        // ⌛ Optional short delay to ensure popup shows
        yield return new WaitForSeconds(0.2f);

        SceneController.Instance.PreviousScene();
    }

    IEnumerator ExitGame()
    {
        Debug.Log("Exiting game...");
        yield return SendData();

        // ⌛ Optional short delay to ensure popup shows
        yield return new WaitForSeconds(0.2f);

        SceneController.Instance.ChangeScene("MainScene");
        SceneController.Instance.ClearHistory();
    }

    void Start()
    {
        retryButton.onClick.AddListener(() =>
        {
            StartCoroutine(Retry());
        });

        exitButton.onClick.AddListener(() =>
        {
            StartCoroutine(ExitGame());
        });

        playerData = PlayerData.Instance;

        PlayerData.Stats stats = playerData.lastGameStats;
        textAciertos.SetText(stats.correctCount + "/" + stats.GetTotalAssertions() + " (" + stats.GetCorrectPercentage() + "%)");
        textFallos.SetText(stats.errorCount + "/" + stats.GetTotalAssertions() + " (" + stats.GetErrorPercentage() + "%)");
        textDistancia.SetText(stats.distance.ToString() + " m.");
        textObjetos.SetText(stats.collectedObjects.ToString() + "/3");

        if (stats.collectedObjects == 3)
        {
            skullIcon.enabled = false;
        }
    }
}
