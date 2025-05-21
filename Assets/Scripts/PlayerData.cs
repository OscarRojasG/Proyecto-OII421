using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private string saveFilePath = "null";
    public SaveData data;

    public List<OutQuestion> outQuestions;

    public void Reload()
    {
        // if file exists
        if (!File.Exists(saveFilePath))
        {
            data = new SaveData();
            return;
        }

        string json = File.ReadAllText(saveFilePath);
        data = JsonUtility.FromJson<SaveData>(json);
        if (data.answeredQuestions == null)
        {
            data.answeredQuestions = new OutQuestion[0];
        }
        outQuestions = new List<OutQuestion>(data.answeredQuestions);
    }

    public void OnLevelFinish(int collisionCount, int errorCount, int distance, int completed)
    {
        if (data == null)
            return;

        if (data.jumpCount >= 10)
        {
            data.hasBunny = true;
        }

        if (data.traveledDistance >= 500)
        {
            data.hasRunner = true;
        }

        if (data.assertionErrors >= 10)
        {
            data.hasRat = true;
        }

        if (collisionCount == 0 && completed == 1)
        {
            data.hasUndamaged = true;
        }

        if (errorCount == 0 && completed == 1)
        {
            data.hasScientist = true;
        }

        if (data.highScore < distance)
        {
            data.highScore = distance;
        }

        if (!data.hasRunner)
        {
            data.traveledDistance += distance;
            if (data.traveledDistance >= 500)
            {
                data.hasRunner = true;
                data.traveledDistance = 500;
            }
        }

        WriteFile();
    }

    public void AssignScientist()
    {
        if (data != null)
        {
            data.hasScientist = true;
            // WriteFile();
        }
        else
        {
            Debug.LogWarning("PlayerData: No data to assign.");
        }
    }

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

    // Must be called at Game start
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

    // Must be called at level end or loss
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
            Debug.LogWarning("PlayerData: No player data loaded.");
        }
    }

    public int getJumps()
    {
        if (data != null)
        {
            return data.jumpCount;
        }
        else
        {
            Debug.LogWarning("PlayerData: No data to get jumps from.");
            return -1;
        }
    }

    public int getDistance()
    {
        if (data != null)
        {
            return data.traveledDistance;
        }
        else
        {
            Debug.LogWarning("PlayerData: No data to get distance from.");
            return -1;
        }
    }

    public void increaseJump()
    {
        if (data != null)
        {
            data.jumpCount++;
            // WriteFile();
        }
        else
        {
            Debug.LogWarning("PlayerData: No data to increase jumps.");
        }
    }

    // Example save data structure
    [System.Serializable]
    public class SaveData
    {
        public string playerName;
        public int highScore;

        // runner
        public bool hasRunner;
        public int traveledDistance;

        // rat
        public bool hasRat;
        public int assertionErrors;

        // bunny
        public bool hasBunny;
        public int jumpCount;

        // undamaged
        public bool hasUndamaged;

        // scientist (Doesn't have a specific stat)
        public bool hasScientist;

        public OutQuestion[] answeredQuestions;
    }
}
