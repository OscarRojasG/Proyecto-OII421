using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    private QuestionData questionData;

    private int currentLevel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantiene el objeto al cambiar de escena

            questionData = LoadQuestions();
        }
        else
        {
            Destroy(gameObject); // Si ya existe una instancia, destruye esta
        }
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void SetCurrentLevel(int currentLevel)
    {
        this.currentLevel = currentLevel;
    }

    private QuestionData LoadQuestions()
    {
        TextAsset asset = Resources.Load<TextAsset>("questions");
        return JsonConvert.DeserializeObject<QuestionData>(asset.text);
    }

    public List<Question> GetQuestions()
    {
        return new List<Question>(questionData.data[currentLevel]);
    }
    
}
