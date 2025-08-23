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
            SetPlayerAnswer(!GetPlayerAnswer());
        });
    }
}
