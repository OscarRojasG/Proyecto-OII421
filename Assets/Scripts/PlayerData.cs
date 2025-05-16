using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private string saveFilePath = "null";
    public SaveData data;

    public List<OutQuestion> outQuestions;

    public bool DataExists()
    {
        return File.Exists(saveFilePath);
    }

    public bool GenerateUserData(string mail)
    {
        if (!DataExists())
        {
            data = new SaveData()
            {
                playerName = mail,
                highScore = 0
            };
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(saveFilePath, json);
            return true;
        }
        return false;
    }

    void Awake()
    {
        saveFilePath =
        Path.Combine(Application.persistentDataPath, "save.json");

        // Set up file path
        // saveFilePath = Path.Combine(Application.persistentDataPath, "save.json");

        if (!DataExists())
        {
            Debug.Log("First run detected. Creating default save file...");

            // // Example default data
            // SaveData defaultData = new SaveData()
            // {
            //     playerName = "Player",
            //     highScore = 0
            // };

            // // Serialize and write to file
            // string json = JsonUtility.ToJson(defaultData, true);
            // File.WriteAllText(saveFilePath, json);

            // Debug.Log("Default save created at: " + saveFilePath);
        }
        else
        {

            Debug.Log("Save file found. Loading existing data...");
            string json = File.ReadAllText(saveFilePath);
            data = JsonUtility.FromJson<SaveData>(json);
            if (data.answeredQuestions == null)
            {
                data.answeredQuestions = new OutQuestion[0];
            }
            outQuestions = new List<OutQuestion>(data.answeredQuestions);
            // Debug.Log("Save file found. Loading existing data...");
            // // Optionally, you can load and deserialize here
        }
    }




    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void WriteFile()
    {
        if (data != null)
        {
            data.answeredQuestions = outQuestions.ToArray();
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(saveFilePath, json);
        }
        else
        {
            Debug.LogWarning("PlayerData: No data to write.");
        }
    }

    // Example save data structure
    [System.Serializable]
    public class SaveData
    {
        public string playerName;
        public int highScore;
        public OutQuestion[] answeredQuestions;
    }
}
