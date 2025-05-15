using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.IO;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    private QuestionData questionData;
    private ProgressData progressData;

    private string currentLevel;

    private void Awake()
    {

        if (Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantiene el objeto al cambiar de escena

            questionData = LoadQuestions();

            /*
            try
            {
                progressData = ReadJson();
            }
            catch (Exception)
            {
                progressData = CreateJsonUserProgress();
            }
            */
        }
        else
        {
            Destroy(gameObject); // Si ya existe una instancia, destruye esta
        }
    }

    public string GetCurrentLevel()
    {
        return currentLevel;
    }

    public void SetCurrentLevel(string currentLevel)
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
    
    // /*
    // public ProgressData CreateJsonUserProgress()
    // {
    //     string path = Path.Combine(Application.persistentDataPath, "Usuario.json");
    //     ProgressData progressData = new ProgressData();
    //     progressData.answeredQuestions = new AnsweredQuestion[0];
    //     string progressJson = JsonUtility.ToJson(progressData, true);
    //     File.WriteAllText(path, progressJson);
    //     return progressData;
    // }

    public ProgressData ReadJson()
    {
        string path = Path.Combine(Application.persistentDataPath, "Usuario.json");
        string progressJson = File.ReadAllText(path);
        return JsonUtility.FromJson<ProgressData>(progressJson);
    }

    public ProgressData GetProgressData()
    {
        return progressData;
    }

    public void SaveProgressData()
    {
        string path = Path.Combine(Application.persistentDataPath, "Usuario.json");
        string progressJson = JsonUtility.ToJson(progressData, true);
        File.WriteAllText(path, progressJson);
        print(progressJson);
    }
    // */

}
