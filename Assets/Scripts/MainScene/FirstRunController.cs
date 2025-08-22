using System;
using System.Runtime.CompilerServices;
using TMPro;
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

    /* Raíz de la ui */
    public Canvas canvas;

    /* Enviar datos */
    public Button submit;

    /* Campo de entrada mail */
    public TMP_InputField mail;

    public OnSubmit onSubmit;

    void Start()
    {
        canvas = GameObject.Find("FirstRunCanvas").GetComponent<Canvas>();
        submit = GameObject.Find("Submit").GetComponent<Button>();
        mail = GameObject.Find("MailInputText").GetComponent<TMP_InputField>();

        submit.onClick.AddListener(_onSubmit);
        mail.onValueChanged.AddListener(onChange);
    }

    public void hide()
    {
        canvas.gameObject.SetActive(false);
    }

    public void show()
    {
        canvas.gameObject.SetActive(true);
    }

    void onChange(string text)
    {
        email = text;
    }

    void _onSubmit()
    {
        Debug.Log("_onSubmit");
        PlayerData.Instance.GenerateUserData(email);
        hide();
        onSubmit();
    }



}