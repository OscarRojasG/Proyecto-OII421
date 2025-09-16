using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionController : MonoBehaviour
{
    public Button buttonLevelOne;
    public Button buttonLevelTwo;
    public Button buttonLevelThree;

    public Image iconLevelOne;
    public Image iconLevelTwo;
    public Image iconLevelThree;

    public Sprite completedLevel;
    public Sprite lockedLevel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Button[] buttons = { buttonLevelOne, buttonLevelTwo, buttonLevelThree };
        Image[] icons = { iconLevelOne, iconLevelTwo, iconLevelThree };
        bool prevCompleted = true;

        for (int i = 0; i < buttons.Length; i++)
        {
            Button button = buttons[i];
            string level = (i + 1).ToString();

            Image icon = icons[i];
            icon.enabled = true;

            print(PlayerData.Instance.Data.playerName);
            PlayerData.Instance.Data.completedLevels.TryGetValue(level, out bool completed);

            if (completed)
            {
                icon.sprite = completedLevel;
                button.onClick.AddListener(() =>
                {
                    SceneController.Instance.ChangeScene("GameScene");
                    GameController.Instance.SetCurrentLevel(level);
                });
            }
            else
            {
                if (prevCompleted == false)
                {
                    icon.sprite = lockedLevel;
                    continue;
                }

                prevCompleted = false;
                icon.enabled = false;
                button.onClick.AddListener(() =>
                {
                    SceneController.Instance.ChangeScene("GameScene");
                    GameController.Instance.SetCurrentLevel(level);
                });
            }
        }
    }
}
