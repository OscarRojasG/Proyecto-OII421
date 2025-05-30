using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// using UnityEngine.UIElements;

public class MainSceneController : MonoBehaviour
{
    public Button playButton;
    public Button achievementsButton;
    public Button tutorialButton;

    private PlayerData pd;
    public Canvas MainScreenCanvas, FirstRunCanvas;
    public TMP_InputField mailInputField;
    public Button continueButton;
    private string mail = "null";

    private Button sendDataButton;
    private Canvas canvas;

    void Start()
    {
        canvas = GameObject.Find("PopupCanvas").GetComponent<Canvas>();
        sendDataButton = GameObject.Find("EnviarDatos").GetComponent<Button>();
        pd = GameController.Instance.GetComponent<PlayerData>();

        GameObject bg = GameObject.Find("CanvasBackground");
        Destroy(bg);
        bg = Instantiate(Resources.Load("Prefabs/CanvasBackground")) as GameObject;
        bg.name = "CanvasBackground"; 
        bg.GetComponentInChildren<MotionController>().speed = -100f;
        // If no data to load
        if (!pd.DataExists()) {
            print("Hiding Main Screen");
            MainScreenCanvas.gameObject.SetActive(false);
            FirstRunCanvas.gameObject.SetActive(true);
            // Trigger on every character change
            mailInputField.onValueChanged.AddListener(OnTextSubmitted);

            // Trigger when the user presses Enter or deselects the input
            mailInputField.onEndEdit.AddListener(OnTextSubmitted);

        } else {
            print("Showing Main Screen");
            FirstRunCanvas.gameObject.SetActive(false);
            MainScreenCanvas.gameObject.SetActive(true);
        }
        playButton.onClick.AddListener(() =>
        {
            SceneController.Instance.ChangeScene("LevelSelection");
        });

        achievementsButton.onClick.AddListener(() =>
        {
            SceneController.Instance.ChangeScene("AchievementsScene");
        });

        sendDataButton.onClick.AddListener(() =>
        {
            Debug.Log("Sending Data!!!!!");
            GameObject popup = Instantiate(Resources.Load("Prefabs/CargandoDatosPopup")) as GameObject;
            RectTransform popupRect = popup.GetComponent<RectTransform>();
            popup.transform.SetParent(canvas.transform, false);
            popupRect.anchoredPosition3D = Vector3.zero;
            pd.sendData();
            Destroy(popup, 3f);

            // pd.WriteFile();
        });

        continueButton.onClick.AddListener(() => {
            // Debug.Log("Creating File");
            pd.GenerateUserData(mail);

            Debug.Log("Showing Main Menu");
            FirstRunCanvas.gameObject.SetActive(false);
            MainScreenCanvas.gameObject.SetActive(true);
        });

        tutorialButton.onClick.AddListener(() =>
        {
            SceneController.Instance.ChangeScene("TutorialScene");
        });
    }

    void Update()
    {
        
    }
    void OnTextSubmitted(string text) {
        mail = text;
    }



}
