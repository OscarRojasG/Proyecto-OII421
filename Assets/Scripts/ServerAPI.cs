using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Collections;
using System;
using Unity.VisualScripting;

public class ServerAPI : MonoBehaviour
{
    public void Start()
    {

    }
    public static String serverUrl = "http://archlinux:3001";
    // public static String serverUrl = "https://bleapi.local.krr.cl";
    public void RegisterOnServer(string name, string mail)
    {
        StartCoroutine(RegisterCoroutine(name, mail));
    }

    private static  IEnumerator RegisterCoroutine(string name, string mail)
    {
        string url = $"{serverUrl}/api/users/";
        var json = JsonUtility.ToJson(new { name = name, email = mail });
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Register Error: " + request.error);
        }
        else
        {
            Debug.Log("Register Success: " + request.downloadHandler.text);

            // Parse the response body to get the token
            var response = JsonUtility.FromJson<Response>(request.downloadHandler.text);

            if (response != null && !string.IsNullOrEmpty(response.token))
            {
                Debug.Log("Token received: " + response.token);
                // Store token in PlayerData
                PlayerData.Instance.data.token = response.token;
            }
            else
            {
                Debug.LogError("Token is missing or invalid in the response.");
            }
        }
    }

    // Define a response class to match the expected JSON structure
    [System.Serializable]
    public class Response
    {
        public string token;
    }


    public void UploadRun(int userId, string runDataJson)
    {
        StartCoroutine(UploadRunCoroutine(userId, runDataJson));
    }

    private IEnumerator UploadRunCoroutine(int userId, string runDataJson)
    {
        string url = $"http://archlinux:3001/api/users/{userId}/run";
        var request = new UnityWebRequest(url, "PUT");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(runDataJson);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
            Debug.LogError("UploadRun Error: " + request.error);
        else
            Debug.Log("UploadRun Success: " + request.downloadHandler.text);
    }
}
