using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{
    private UnityAction<OptionController> onClickAction;
    public TextMeshProUGUI buttonText;

    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            onClickAction(this);
        });
    }

    public void SetText(string text)
    {
        buttonText.SetText(text);
    }

    public string GetText()
    {
        return buttonText.text;
    }

    public void SetOnClickAction(UnityAction<OptionController> action)
    {
        onClickAction = action;
    }
}
