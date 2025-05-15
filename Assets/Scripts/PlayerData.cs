using System.IO;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private string saveFilePath = "null";
    public bool DataExists() {
        return File.Exists(saveFilePath);
    }

    public bool GenerateUserData(string mail) {
        if (!DataExists()) {
            SaveData data = new SaveData() {
                playerName = mail,
                highScore = 0
            };
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(saveFilePath, json);
            return true;
        } else {
            return false;
        }
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

    // Example save data structure
    [System.Serializable]
    public class SaveData
    {
        public string playerName;
        public int highScore;
    }
}
