using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionController : MonoBehaviour
{
    public Button buttonLevelOne;
    public Button buttonLevelTwo;
    public Button buttonLevelThree;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonLevelOne.onClick.AddListener(() =>
        {
            SceneController.Instance.ChangeScene("GameScene");
            GameController.Instance.SetCurrentLevel(1);
        });

        buttonLevelTwo.onClick.AddListener(() =>
        {
            SceneController.Instance.ChangeScene("GameScene");
            GameController.Instance.SetCurrentLevel(2);
        });

        buttonLevelThree.onClick.AddListener(() =>
        {
            SceneController.Instance.ChangeScene("GameScene");
            GameController.Instance.SetCurrentLevel(3);
        });
        
    }
}
