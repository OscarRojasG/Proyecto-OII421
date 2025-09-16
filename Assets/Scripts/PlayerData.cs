using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

using AOT;
using NUnit.Framework.Constraints;


public class PlayerData : MonoBehaviour
{
    private static int MAX_JUMP_COUNT = 10;
    private static int MAX_TRAVELED_DISTANCE = 500;
    private static int MAX_ASSERTION_ERRORS = 10;


    private string saveFilePath = "save.json";
    public SaveData Data;
    public RunData RunData;
    public Stats lastGameStats { private set; get; }

    public static PlayerData Instance { get; private set; }
    private ServerAPI serverAPI;

    public void Syncfs()
    {
        WebGL.FileSystem.Syncfs((err) =>
        {
            if (string.IsNullOrEmpty(err))
            {
                Debug.Log("OK!!");
            }
            else
            {
                Debug.Log("NG!!");
            }
        });
    }

    /*#if UNITY_WEBGL && !UNITY_EDITOR*/
    /*    [DllImport("_Internal")]*/
    /*    private static extern void SyncFSToIDB();*/
    /*#endif*/

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

    public IEnumerator SendDataCoroutine(Func<bool> abortSend, GameObject popup)
    {
        if (Data != null)
        {
            string json = JsonConvert.SerializeObject(RunDataConverter.ToMinimal(RunData));
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
                if (popup != null)
                {
                    popup.SetActive(false);
                    GameObject text = popup.transform.Find("Panel/Text (TMP)").gameObject;
                    var textComponent = text.GetComponentInChildren<TMPro.TextMeshProUGUI>();



                    popup.GetComponentInChildren<UnityEngine.UI.Text>().text = "Error: " + error;
                    yield return new WaitForSeconds(2f);
                }
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
        int userId = PlayerData.Instance.Data.playerId;


        string url = $"{ServerAPI.serverUrl}/api/users/{userId}/runs/";
        string token = PlayerData.Instance.Data.token;

        using (UnityWebRequest request = new UnityWebRequest(url, "PUT"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("x-api-key", token);
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
        serverAPI = gameObject.AddComponent<ServerAPI>();
        Debug.Log("PlayerData: Start");
        saveFilePath = Path.Combine(Application.persistentDataPath, saveFilePath);
        // if file exists
        if (!DataExists())
        {
            Data = new SaveData();
            return;
        }
        else
        {
            string json = File.ReadAllText(saveFilePath);
            Data = JsonConvert.DeserializeObject<SaveData>(json);
        }

    }

    void UpdateAchievements(
        int collisionCount, int errorCount, int completed, int distance
    ) {
        if (Data.jumpCount >= MAX_JUMP_COUNT)
            Data.hasBunny = true;

        if (Data.traveledDistance >= MAX_TRAVELED_DISTANCE)
        {
            Data.hasRunner = true;
        }

        if (Data.assertionErrors >= MAX_ASSERTION_ERRORS)
            Data.hasRat = true;

        if (collisionCount == 0 && completed == 1)
            Data.hasUndamaged = true;

        if (errorCount == 0 && completed == 1)
            Data.hasScientist = true;

        if (Data.highScore < distance)
            Data.highScore = distance;

        if (!Data.hasRunner)
        {
            Data.traveledDistance += distance;
            if (Data.traveledDistance >= MAX_TRAVELED_DISTANCE)
            {
                Data.hasRunner = true;
                Data.traveledDistance = MAX_TRAVELED_DISTANCE;
            }
        }
    }

    /* Se ejecuta al finalizar el nivel. Su objetivo es actualizar los datos
     * del jugador (data) en base al efecto de los eventos del nivel en el 
     * estado de jugador final. Se desbloquean logros, se guarda el progreso y
     * se registran las estadísticas de la partida finalizada.

     * Genera stats del Run para mostrarlos en la pantalla de GameOver.
     */

    public void OnLevelFinish(int collisionCount, int assertionCount, int errorCount, int distance, int completed, int collectedObjects)
    {
        if (Data == null)
            throw new Exception("PlayerData: No data loaded.");

        UpdateAchievements(collisionCount, errorCount, completed, distance);


        if (completed == 1)
        {
            Data.completedLevels[GameController.Instance.GetCurrentLevel()] = true;
        }

        RunData.completed = completed;

        WriteFile();

        UpdateLastGameStats(distance, collectedObjects, assertionCount, errorCount);


        SendDataToServer(JsonConvert.SerializeObject(RunDataConverter.ToMinimal(PlayerData.Instance.RunData)), null, null);
    }

    private void UpdateLastGameStats(int distance, int collectedObjects, int assertionCount, int errorCount)
    {
        lastGameStats = new Stats();
        lastGameStats.distance = distance;
        lastGameStats.collectedObjects = collectedObjects;
        lastGameStats.correctCount = assertionCount - errorCount;
        lastGameStats.errorCount = errorCount;
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

    public bool GenerateUserData(string name, string mail)
    {
        if (!DataExists())
        {
            Data.answeredQuestions = new Dictionary<string, List<OutQuestion>>
            {
                { "1", new List<OutQuestion>() },
                { "2", new List<OutQuestion>() },
                { "3", new List<OutQuestion>() },
                { "4", new List<OutQuestion>() }
            };
            Data.completedLevels = new Dictionary<string, bool>();

            Data.playerName = mail;
            Data.playerNombre = name;
            WriteFile();

            // Register on server

            registerOnServer(name, mail);



        }

        // Reload();
        return false;
    }


    private void registerOnServer(string name, string mail)
    {
        serverAPI.RegisterOnServer(name, mail);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Must be called at level end or loss
    public void WriteFile()
    {

        if (Data == null)
        {
            throw new Exception("PlayerData: No data to write.");
        }

        string json = Newtonsoft.Json.JsonConvert.SerializeObject(Data);
        File.WriteAllText(saveFilePath, json);
#if UNITY_WEBGL && !UNITY_EDITOR
        Syncfs();
#endif
    }

    public int getJumps()
    {
        if (Data != null) return Data.jumpCount;
        else throw new Exception("PlayerData: No data to get jumps from.");
    }

    public int getDistance()
    {
        if (Data != null) return Data.traveledDistance;
        else throw new Exception("PlayerData: No data to get distance from.");
    }

    public IEnumerator SendDataCoroutine()
    {
        if (Data != null)
        {
            string json = JsonConvert.SerializeObject(Data);
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

    // public void increaseJump()
    // {
    //     if (data != null)
    //         data.jumpCount++;
    //     else
    //         throw new Exception("PlayerData: No data to increase jumps.");
    // }


    public void RegisterAnsweredQuestion(string levelId, OutQuestion q)
    {
        // if (!RunData.questions.ContainsKey(levelId))
        //     RunData.questions[levelId] = new List<OutQuestion>();
        RunData.levelId = levelId;
        RunData.questions.Add(q);
        // WriteFile();
    }
}


[System.Serializable]
public class SaveData
{
    // Nombre
    public string playerNombre;

    // Correo
    public string playerName;

    /* DATOS PROVISTOS POR EL SERVIDOR */

    public int playerId;


    /* Token: Se utiliza para autenticarse al servidor y autorizar el 
     * envío de runs.
     */
    public string token;

    public int highScore;


    /* LOGROS */
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



// Magia negra para habilitar la memoria persistente en WebGL
//

namespace WebGL
{
    public static class FileSystem
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        static extern void FileSystemSyncfsAddEvent(Action<int, string> action);
        [DllImport("__Internal")]
        static extern void FileSystemSyncfs(int id);
#else

        static Action<int, string> _callback;
        static void FileSystemSyncfsAddEvent(Action<int, string> action) { _callback = action; }
        static void FileSystemSyncfs(int id) { _callback?.Invoke(id, ""); }
#endif


        static Dictionary<int, Action<string>> callbacks = new Dictionary<int, Action<string>>();

        /// <summary>
        /// 静的コンストラクタ
        /// 同期完了時に呼び出し内部コールバックを登録する
        /// </summary>
        static FileSystem()
        {
            FileSystemSyncfsAddEvent(Callback);
        }

        /// <summary>
        /// 同期完了時の内部コールバック
        /// </summary>
        /// <param name="id"></param>
        [MonoPInvokeCallback(typeof(Action<int, string>))]
        static void Callback(int id, string error)
        {
            var cb = callbacks[id];
            callbacks.Remove(id);
            cb?.Invoke(string.IsNullOrEmpty(error) ? null : error);
        }

        /// <summary>
        /// IndexedDBを同期させる
        /// </summary>
        /// <param name="cb">string : error</param>
        public static void Syncfs(Action<string> cb)
        {
            var id = callbacks.Count + 1;
            callbacks.Add(id, cb);
            FileSystemSyncfs(id);
        }
    }
}



[System.Serializable]
public class MinimalAnswer
{
    public object[] a = new object[5];
}

[System.Serializable]
public class MinimalRunData
{
    public int p;
    public string l;


    public int d, j, e, c, cc;
    public List<object[]> a;
}



[System.Serializable]
public class RunData
{

    public RunData()
    {
        questions = new List<OutQuestion>();
    }
    public int playerId;
    public string levelId;

    // summary
    public int distance;
    public int jumps;
    public int errors;
    public int correct;

    public int completed;

    // optional: session tracking
    // public string runId;     // unique UUID per run
    // public float duration;   // seconds played

    // answered questions
    public List<OutQuestion> questions;
}




/* Virtually equivalent to OutQuestion */

// [System.Serializable]
// public class AnsweredQuestion
// {
//     public int questionId;
//     public float answerTime;
//     public List<AssertionAnswer> assertions;
// }

// Virtually equivalent to OutAssertion
// [System.Serializable]
// public class AssertionAnswer
// {
//     public int assertionId;
//     public int formId;
//     public bool correct;
// }
