using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneController.Instance.PreviousScene();
        });
    }
}
