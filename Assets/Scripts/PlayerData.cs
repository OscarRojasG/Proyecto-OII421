using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerData : MonoBehaviour
{
    private string saveFilePath = "save.json";
    public SaveData data;
    public Stats lastGameStats { private set; get; }

    public static PlayerData Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            saveFilePath = Path.Combine(Application.persistentDataPath, saveFilePath);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void sendData()
    {
        if (data != null)
        {
            string json = JsonConvert.SerializeObject(data);
            Debug.Log("Sending data: " + json);
            // Send the JSON data to your server or API
            // Example: using UnityWebRequest
            StartCoroutine(SendDataToServer(json,
                onSuccess: (response) =>
                {
                    Debug.Log("Success! Server responded: " + response);
                },
                onError: (error) =>
                {
                    throw new Exception("Something went wrong: " + error);
                }
            ));
        }
        else
        {
            throw new Exception("PlayerData: No data to send.");
        }
        WriteFile(); 
    }

    public IEnumerator SendDataCoroutine()
    {
        if (data != null)
        {
            string json = JsonConvert.SerializeObject(data);
            Debug.Log("Sending data: " + json);

            bool isDone = false;
            string result = "";
            string error = "";

            yield return SendDataToServer(json,
                onSuccess: (response) =>
                {
                    result = response;
                    isDone = true;
                },
                onError: (err) =>
                {
                    error = err;
                    isDone = true;
                });


            yield return new WaitUntil(() => isDone);

            if (!string.IsNullOrEmpty(error))
            {
                Debug.LogError("Error in sendData: " + error);
            }
            else
            {
                Debug.Log("Success: " + result);
            }
        }
        else
        {
            Debug.LogWarning("PlayerData: No data to send.");
            yield break;
        }
    }

    public IEnumerator SendDataToServer(string json, System.Action<string> onSuccess, System.Action<string> onError)
    {
        string url = "https://bleapi.krr.cl/submit";
        string token = "my-secret-token"; // Replace with your actual token

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Authorization", token);
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error sending data: " + request.error);
                onError?.Invoke(request.error);
            }
            else
            {
                Debug.Log("Data sent successfully: " + request.downloadHandler.text);
                onSuccess?.Invoke(request.downloadHandler.text);
            }
        }

    }

    public void Start()
    {
        Debug.Log("PlayerData: Start");
        saveFilePath = Path.Combine(Application.persistentDataPath, saveFilePath);
        // if file exists
        if (!DataExists())
        {
            data = new SaveData();
            return;
        }
        else
        {
            string json = File.ReadAllText(saveFilePath);
            data = JsonConvert.DeserializeObject<SaveData>(json);
        }

    }

    public void OnLevelFinish(int collisionCount, int assertionCount, int errorCount, int distance, int completed, int collectedObjects)
    {
        if (data == null)
            throw new Exception("PlayerData: No data loaded.");

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

        if (completed == 1)
        {
            data.completedLevels[GameController.Instance.GetCurrentLevel()] = true;
        }

        WriteFile();


        lastGameStats = new Stats();
        lastGameStats.distance = distance;
        lastGameStats.collectedObjects = collectedObjects;
        lastGameStats.correctCount = assertionCount - errorCount;
        lastGameStats.errorCount = errorCount;

        // SendDataToServer(JsonConvert.SerializeObject(data), null, null);
    }

    // public void AssignScientist()
    // {
    //     if (data != null)
    //     {
    //         data.hasScientist = true;
    //         // WriteFile();
    //     }
    //     else
    //     {
    //         "PlayerData: No data to assign.");
    //     }
    // }

    public bool DataExists()
    {
        return File.Exists(saveFilePath);
    }

    public bool GenerateUserData(string mail)
    {
        if (!DataExists())
        {
            data.answeredQuestions = new Dictionary<string, List<OutQuestion>>
            {
                { "1", new List<OutQuestion>() },
                { "2", new List<OutQuestion>() },
                { "3", new List<OutQuestion>() }
            };
            data.completedLevels = new Dictionary<string, bool>();

            data.playerName = mail;
            WriteFile();
        }

        // Reload();
        return false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Must be called at level end or loss
    public void WriteFile()
    {

        if (data == null)
        {
            throw new Exception("PlayerData: No data to write.");
        }

        string json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
        File.WriteAllText(saveFilePath, json);
    }

    public int getJumps()
    {
        if (data != null) return data.jumpCount;
        else throw new Exception("PlayerData: No data to get jumps from.");
    }

    public int getDistance()
    {
        if (data != null) return data.traveledDistance;
        else throw new Exception("PlayerData: No data to get distance from.");
    }

    // public void increaseJump()
    // {
    //     if (data != null)
    //         data.jumpCount++;
    //     else
    //         throw new Exception("PlayerData: No data to increase jumps.");
    // }

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

        public Dictionary<string, List<OutQuestion>> answeredQuestions;
        public Dictionary<string, bool> completedLevels;
    }

    public class Stats
    {
        public int distance;
        public int correctCount;
        public int errorCount;
        public int collectedObjects;

        public int GetTotalAssertions()
        {
            return errorCount + correctCount;
        }

        public int GetCorrectPercentage()
        {
            if (GetTotalAssertions() != 0)
                return (int)(((float)correctCount / GetTotalAssertions()) * 100);
            return 0;
        }

        public int GetErrorPercentage()
        {
            if (GetTotalAssertions() != 0)
                return (int)(((float)errorCount / GetTotalAssertions()) * 100);
            return 0;
        }
    }
}
