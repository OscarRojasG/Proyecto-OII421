using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DynamicAssertionController : AssertionController
{
    void Start()
    {
        iconButton.onClick.AddListener(() =>
        {
            GameController.Instance.sfx.playButtonClick();
            SetPlayerAnswer(!GetPlayerAnswer());
        });
    }
}
