using UnityEngine;
using UnityEngine.UI;

public class MainSceneController : MonoBehaviour
{
    public Button playButton;

    private void Start()
    {
        playButton.onClick.AddListener(() =>
        {
            SceneController.Instance.ChangeScene("LevelSelection");
        });
    }

}
