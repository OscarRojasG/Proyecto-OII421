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
    // Constants
    private static int MAX_JUMP_COUNT = 10;
    private static int MAX_TRAVELED_DISTANCE = 500;
    private static int MAX_ASSERTION_ERRORS = 10;


    // Child data
    public SaveData Data;
    public RunData RunData;
    public Stats lastGameStats { private set; get; }

    // Singleton 
    public static PlayerData Instance { get; private set; }

    // Server interaction
    private ServerAPI serverAPI;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void Start()
    {
        serverAPI = gameObject.AddComponent<ServerAPI>();
        Debug.Log("PlayerData: Start");
        Data = SaveSystem.Load();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateAchievements(
        int collisionCount, int errorCount, int completed, int distance
    )
    {
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

        SaveSystem.Save(Data);

        UpdateLastGameStats(distance, collectedObjects, assertionCount, errorCount);
        // serverAPI.UploadRun(Data.playerId, PlayerData.Instance.RunData);
    }

    public void Load(SaveData data)
    {
        Data = data;
    }


    private void UpdateLastGameStats(int distance, int collectedObjects, int assertionCount, int errorCount)
    {
        lastGameStats = new Stats();
        lastGameStats.distance = distance;
        lastGameStats.collectedObjects = collectedObjects;
        lastGameStats.correctCount = assertionCount - errorCount;
        lastGameStats.errorCount = errorCount;
    }




    public void registerOnServer(string name, string mail)
    {
        serverAPI.RegisterOnServer(name, mail);
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
