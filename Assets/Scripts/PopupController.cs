using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PopupController : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public Button okButton;
    public Button cancelButton;

    private Action onOk;
    private Action onCancel;

    public void Setup(string message, Action onOkCallback, Action onCancelCallback = null)
    {
        messageText.text = message;
        onOk = onOkCallback;
        onCancel = onCancelCallback;

        okButton.onClick.RemoveAllListeners();
        okButton.onClick.AddListener(() =>
        {
            onOk?.Invoke();
            Destroy(gameObject);
        });

        if (cancelButton != null)
        {
            cancelButton.gameObject.SetActive(onCancel != null);
            cancelButton.onClick.RemoveAllListeners();
            cancelButton.onClick.AddListener(() =>
            {
                onCancel?.Invoke();
                Destroy(gameObject);
            });
        }
    }


}
