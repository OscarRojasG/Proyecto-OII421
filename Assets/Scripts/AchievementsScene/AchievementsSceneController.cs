using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsSceneController : MonoBehaviour
{
    PlayerData pd;
    public TextMeshProUGUI target1; 
    public TextMeshProUGUI target2; 
    public TextMeshProUGUI target3; 
    public TextMeshProUGUI target4; 
    public TextMeshProUGUI target5; 
    public Button backButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pd = PlayerData.Instance;
        Debug.Log("PlayerData loaded: " + pd.data.playerName);
        target1.text = "";
        target2.text = "";
        target3.text = "";
        target4.text = "";
        target5.text = "";

        if (pd.data.hasScientist)
        {
            target1.text += "Completado";
        }
        else
        {
            target1.text += "No completado";
        }

        if (pd.data.hasUndamaged)
        {
            target2.text += "Completado";
        }
        else
        {
            target2.text += "No completado";
        }

        if (pd.data.hasBunny)
        {
            target3.text += "Completado";
        }
        else
        {
            target3.text += "No completado";
        }

        target3.text += " - Saltos: " + pd.getJumps() + "/10";


        if (pd.data.hasRunner)
        {
            target4.text += "Completado";
        }
        else
        {
            target4.text += "No completado";
        }

        target4.text += " - Distancia: " + pd.getDistance() + "/500 m.";

        if (pd.data.hasRat)
        {
            target5.text += "Completado";
        }
        else
        {
            target5.text += "No completado";
        }
        target5.text += " - Errores: " + pd.data.assertionErrors + "/10";
        // Create a TextMeshProUGUI component in the scene and assign it to ach1Text
        backButton.onClick.AddListener(() =>
        {
            SceneController.Instance.ChangeScene("MainScene");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
