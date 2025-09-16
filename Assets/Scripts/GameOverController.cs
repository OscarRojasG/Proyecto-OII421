using System;
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

    private bool abortSend = false; // Flag to indicate if the send operation should be aborted
    private GameObject _currentPopup = null; // Reference to the current popup
public IEnumerator SendData()
    {
        abortSend = false; // Reset the flag
        Canvas canvas = GameObject.Find("PopupCanvas").GetComponent<Canvas>();

        // Instantiate the popup
        _currentPopup = Instantiate(Resources.Load("Prefabs/CargandoDatosPopup")) as GameObject;

        if (_currentPopup == null)
        {
            Debug.LogError("Failed to load Prefabs/CargandoDatosPopup.");
            yield break; // Exit if popup couldn't be loaded
        }

        _currentPopup.transform.SetParent(canvas.transform, false);
        RectTransform popupRect = _currentPopup.GetComponent<RectTransform>();
        popupRect.anchoredPosition3D = Vector3.zero;

        yield return StartCoroutine(playerData.SendDataCoroutine(() => abortSend, _currentPopup)); // Assuming pd.SendDataCoroutine doesn't take _isSendAborted as a param, but checks a shared flag in pd script.

        // Important: Only destroy the popup here if it hasn't already been destroyed by the cancel button.
        if (_currentPopup != null)
        {
            Debug.Log("SendDataCoroutine finished, destroying popup.");
            Destroy(_currentPopup);
            _currentPopup = null;
        }
        else
        {
            Debug.Log("Popup already destroyed by cancel button.");
        }
    }

    IEnumerator Retry()
    {
        yield return SendData();

        SceneController.Instance.PreviousScene();
    }

    IEnumerator ExitGame()
    {
        Debug.Log("Exiting game...");
        yield return SendData();

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

        Stats stats = playerData.lastGameStats;
        textAciertos.SetText(stats.correctCount + "/" + stats.GetTotalAssertions() + " (" + stats.GetCorrectPercentage() + "%)");
        textFallos.SetText(stats.errorCount + "/" + stats.GetTotalAssertions() + " (" + stats.GetErrorPercentage() + "%)");
        textDistancia.SetText(stats.distance.ToString() + " m.");
        textObjetos.SetText(stats.collectedObjects.ToString() + "/3");

        // Fill run end data

        playerData.RunData.distance = stats.distance;
        playerData.RunData.errors = stats.errorCount;
        playerData.RunData.correct = stats.correctCount;


        if (stats.collectedObjects == 3)
        {
            skullIcon.enabled = false;
        }
    }

}
