using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// using UnityEngine.UIElements;

public class MainSceneController : MonoBehaviour
{
    public Button playButton;
    public PlayerData pd;
    public Canvas MainScreenCanvas, FirstRunCanvas;
    public TMP_InputField mailInputField;
    public Button continueButton;
    private string mail = "null";

    void Start()
    {

        if (!pd.DataExists()) {
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
        }
        playButton.onClick.AddListener(() =>
        {
            SceneController.Instance.ChangeScene("LevelSelection");
        });

        continueButton.onClick.AddListener(() => {
            pd.GenerateUserData(mail);
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
