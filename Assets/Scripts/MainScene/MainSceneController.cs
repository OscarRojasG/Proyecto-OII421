using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// using UnityEngine.UIElements;


/* Enlazado a MainScene::MainSceneController 
 * Se encarga de: asignar los botones
 * JUGAR, TUTORIAL, LOGROS, ENVIAR DATOS
 *
 */
public class MainSceneController : MonoBehaviour
{
    private PlayerData pd;

    public FirstRunController firstRun;

    public Canvas MainScreenCanvas;

    public Button playButton;
    public Button achievementsButton;
    public Button tutorialButton;
    private Button sendDataButton;



    public TMP_InputField mailInputField;

    private Canvas canvas;

    void show()
    {
        canvas.gameObject.SetActive(true);
    }

    void hide()
    {
        canvas.gameObject.SetActive(false);
    }

    void Start()
    {
        firstRun = GameObject.Find("FirstRunCanvas").GetComponent<FirstRunController>();
        firstRun.hide();
        firstRun.onSubmit = () =>
        {
            Debug.Log("Showing Main Screen");
            show();
        };

        canvas = GameObject.Find("PopupCanvas").GetComponent<Canvas>();
        sendDataButton = GameObject.Find("EnviarDatos").GetComponent<Button>();

        pd = PlayerData.Instance;

        GameObject bg = GameObject.Find("CanvasBackground");
        Destroy(bg);
        bg = Instantiate(Resources.Load("Prefabs/CanvasBackground")) as GameObject;
        bg.name = "CanvasBackground"; 
        bg.GetComponentInChildren<MotionController>().speed = -100f;
        // If no data to load
        if (!SaveSystem.DataExists()) {
            print("Hiding Main Screen");

            // Esconder el canvas del menú principal
            // hide();

            // Mostrar solicitud correo electrónico
            firstRun.show();


        } else {
            print("Showing Main Screen");
            firstRun.hide();
            show();
        }
        playButton.onClick.AddListener(() =>
        {
            PopupManager.Show("Start Level Selection?", () =>
            {
                SceneController.Instance.ChangeScene("LevelSelection");
            }, () => { Debug.Log("cancel"); });
        });

        achievementsButton.onClick.AddListener(() =>
        {
            SceneController.Instance.ChangeScene("AchievementsScene");
        });

        tutorialButton.onClick.AddListener(() =>
        {
            SceneController.Instance.ChangeScene("TutorialScene");
        });
    }

    void Update()
    {
        
    }




}
