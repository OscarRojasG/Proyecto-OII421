using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    public Button retryButton;
    public Button exitButton;
    private PlayerData playerData;

    IEnumerator ExitGame()
    {
        Debug.Log("Exiting game...");
        playerData = GameController.Instance.GetComponent<PlayerData>();

        Canvas canvas = GameObject.Find("PopupCanvas").GetComponent<Canvas>();
        GameObject popup = Instantiate(Resources.Load("Prefabs/CargandoDatosPopup")) as GameObject;

        popup.transform.SetParent(canvas.transform, false);
        RectTransform popupRect = popup.GetComponent<RectTransform>();
        popupRect.anchoredPosition3D = Vector3.zero;

        Destroy(popup, 3f);

        // ⏳ Wait until data is actually sent
        yield return StartCoroutine(playerData.SendDataCoroutine());

        // ⌛ Optional short delay to ensure popup shows
        yield return new WaitForSeconds(0.2f);

        SceneController.Instance.ChangeScene("MainScene");
        SceneController.Instance.ClearHistory();
    }

    void Start()
    {
        retryButton.onClick.AddListener(() =>
        {
            SceneController.Instance.PreviousScene();
        });

        exitButton.onClick.AddListener(() =>
        {
            StartCoroutine(ExitGame());
        });
    }
}
