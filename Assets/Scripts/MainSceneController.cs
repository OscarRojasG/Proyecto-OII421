using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// using UnityEngine.UIElements;

public class MainSceneController : MonoBehaviour
{
    public Button playButton;
    public Button achievementsButton;
    public PlayerData pd;
    public Canvas MainScreenCanvas, FirstRunCanvas;
    public TMP_InputField mailInputField;
    public Button continueButton;
    private string mail = "null";

    void Start()
    {
        GameObject bg = GameObject.Find("CanvasBackground");
        Destroy(bg);
        bg = Instantiate(Resources.Load("Prefabs/CanvasBackground")) as GameObject;
        bg.name = "CanvasBackground"; 
        bg.GetComponentInChildren<MotionController>().speed = -100f;
        if (!pd.DataExists()) {
            pd.Reload();
            print("Hiding Main Screen");
            MainScreenCanvas.gameObject.SetActive(false);
            FirstRunCanvas.gameObject.SetActive(true);
            //         // Trigger on every character change
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

        continueButton.onClick.AddListener(() => {
            Debug.Log("Creating File");
            pd.GenerateUserData(mail);

            Debug.Log("Showing Main Menu");
            FirstRunCanvas.gameObject.SetActive(false);
            MainScreenCanvas.gameObject.SetActive(true);
        });
    }

    void Update()
    {
        
    }
    void OnTextSubmitted(string text) {
        mail = text;
    }



}
