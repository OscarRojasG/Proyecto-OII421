using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/* Enlazado a MainScene::FirstRunCanvas
 * Se encarga de manejar la entrada del usuario y solicitar el correo 
 * electrónico.
 */
public class FirstRunController : MonoBehaviour
{

    public delegate void OnSubmit();
    /* Datos de usuario */
    private PlayerData pd;

    /* Guarda la entrada de usuario */
    private String email = "null";

    /* Guarda la entrada de usuario */
    private String nombre = "null";

    /* Raíz de la ui */
    public Canvas canvas;

    /* Enviar datos */
    public Button submit;

    /* Campo de entrada mail */
    public TMP_InputField mail;
    public TMP_InputField nombreInput;

    public OnSubmit onSubmit;
    private ServerAPI serverAPI;

    private bool popupActive;

    void Start()
    {
        canvas = GameObject.Find("FirstRunCanvas").GetComponent<Canvas>();
        submit = GameObject.Find("Submit").GetComponent<Button>();
        mail = GameObject.Find("MailInputText").GetComponent<TMP_InputField>();
        nombreInput = GameObject.Find("NombreInputText").GetComponent<TMP_InputField>();
        serverAPI = gameObject.AddComponent<ServerAPI>();

        submit.onClick.AddListener(_onSubmit);

        mail.onValueChanged.AddListener(onEmailChange);
        nombreInput.onValueChanged.AddListener(onNameChange);
    }

    public void hide()
    {
        canvas.gameObject.SetActive(false);
    }

    public void show()
    {
        canvas.gameObject.SetActive(true);
        // We are requested to do something, so we must init the playerdata.
    }

    void onEmailChange(string text)
    {
        email = text;
    }

    void onNameChange(string text)
    {
        nombre = text;
    }

    public void _onSubmit()
    {
        if (popupActive) return;
        StartCoroutine(_onSubmitRoutine());
    }

    private IEnumerator _onSubmitRoutine()
    {
        Debug.Log("_onSubmit");

        if (!isValidEmail(email))
        {
            popupActive = true;
            PopupManager.Show("Correo electrónico inválido. Debe ser @mail.pucv.cl.", () =>
            {
                popupActive = false;
            });
            yield break;
        }

        PopupManager.LoadingShow("Contactando al servidor. Por favor espere.");

        bool done = false;
        bool errorOccurred = false;



        SaveData data = SaveSystem.GenerateUserData(nombre, email);
        PlayerData.Instance.Load(data);

        PopupManager.LoadingShow();
        serverAPI.RegisterOnServer(
            nombre,
            email,
            () => { done = true; },  // onSuccess
            (errorMsg) =>
            {            // onError
                errorOccurred = true;
                PopupManager.LoadingHide();
                // PopupManager.Show("No se pudo contactar al servidor, inténtelo de nuevo más tarde.", null);
            },
            () =>
            {
                errorOccurred = true;
                PopupManager.LoadingHide();
                // PopupManager.Show("No se pudo contactar al servidor, inténtelo de nuevo más tarde.", null);
            }
        );

        yield return new WaitUntil(() => done || errorOccurred);

        if (errorOccurred)
        {
            popupActive = true;
            PopupManager.Show("No se pudo contactar al servidor, inténtelo de nuevo más tarde.", () =>
            {
                popupActive = false;
            });
            yield break;
        }


        PopupManager.LoadingHide();


        popupActive = true;
        PopupManager.Show("Usuario registrado exitosamente", () =>
        {
            SaveSystem.Save(PlayerData.Instance.Data);
            hide();
            onSubmit?.Invoke();
            popupActive = false;
        });
    }

    // Método para validar el correo electrónico
    private bool isValidEmail(string email)
    {
        // Verificar que tenga el dominio correcto y que sea un correo electrónico básico válido
        string emailPattern = @"^[a-zA-Z0-9._%+-]+@mail\.pucv\.cl$";

        // Usamos Regex para hacer la validación
        return Regex.IsMatch(email, emailPattern);
    }

}