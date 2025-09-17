using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Collections;
using System;
using Unity.VisualScripting;
using Newtonsoft.Json;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

[System.Serializable]
public class RegisterUserData
{
    public string name;
    public string email;

    public RegisterUserData(string name, string email)
    {
        this.name = name;
        this.email = email;
    }
}


public class ServerAPI : MonoBehaviour
{
    public void Start()
    {

    }
    // public static String serverUrl = "http://archlinux:3001";
    public static string serverUrl = "https://bleapi.krr.cl";
    public void RegisterOnServer(string name, string mail, Action onSuccess = null, Action<string> onError = null, Action onTimeout = null)
    {
        StartCoroutine(RegisterOnServerCoroutine(name, mail, onSuccess, onError, onTimeout));
    }

    private static IEnumerator RegisterOnServerCoroutine(string name, string mail, Action onSuccess, Action<string> onError, Action onTimeout)
    {
        string url = $"{serverUrl}/api/users/";
        RegisterUserData user = new RegisterUserData(name, mail);
        string json = JsonUtility.ToJson(user);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.timeout = 10;

            // Log the body and headers for debugging
            Debug.Log("Sending registration request");
            Debug.Log("Request Body: " + json);

            yield return request.SendWebRequest();
            switch (request.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    // This result is often due to a timeout or a network/DNS issue.
                    Debug.LogError("Connection Error: " + request.error);
                    onTimeout?.Invoke(); // Invoke the specific timeout callback
                    break;

                case UnityWebRequest.Result.ProtocolError:
                    // This indicates an error response from the server (e.g., 404, 500).
                    string serverError = $"Protocol Error: {request.error}\nResponse: {request.downloadHandler.text}";
                    Debug.LogError(serverError);
                    onError?.Invoke(request.downloadHandler.text); // Send server response to the error handler
                    break;

                case UnityWebRequest.Result.Success:
                    Debug.Log("Register Success: " + request.downloadHandler.text);
                    try
                    {
                        // Attempt to parse the JSON response from the server
                        var response = JsonUtility.FromJson<RegistrationResponse>(request.downloadHandler.text);

                        if (response != null && !string.IsNullOrEmpty(response.token))
                        {
                            Debug.Log("Token received: " + response.token);

                            // Store the received token and user ID
                            PlayerData.Instance.Data.token = response.token;
                            PlayerData.Instance.Data.playerId = response.user.id;
                            SaveSystem.Save(PlayerData.Instance.Data);

                            onSuccess?.Invoke(); // Signal that the registration was successful
                        }
                        else
                        {
                            const string errorMsg = "Token is missing or invalid in the server response.";
                            Debug.LogError(errorMsg);
                            onError?.Invoke(errorMsg);
                        }
                    }
                    catch (Exception ex)
                    {
                        string errorMsg = $"Failed to parse server response: {ex.Message}";
                        Debug.LogError(errorMsg);
                        Debug.Log(ex);
                        onError?.Invoke(errorMsg);
                    }
                    break;
            }

        }
    }
    // Define a response class to match the expected JSON structure
    [System.Serializable]
    public class RegistrationResponse
    {
        public string token;
        public User user;
    }

    [System.Serializable]
    public class User
    {
        public int id;
    }


    public void UploadRun(int userId, RunData runData,
        Action<string> onSuccess = null, Action<string> onError = null)
    {
        string json = JsonConvert.SerializeObject(RunDataConverter.ToMinimal(runData));
        StartCoroutine(UploadRunCoroutine(userId, json, onSuccess, onError));
    }

    private IEnumerator UploadRunCoroutine(int userId, string runDataJson,
        Action<string> onSuccess, Action<string> onError)
    {
        string url = $"{serverUrl}/api/users/{userId}/run";
        using (var request = new UnityWebRequest(url, "PUT"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(runDataJson);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("x-api-key", PlayerData.Instance.Data.token);
            request.timeout = 10; // <-- set timeout

            yield return request.SendWebRequest();
            try
            {

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("UploadRun Error: " + request.error);
                    onError?.Invoke(request.error);
                }
                else
                {
                    string response = request.downloadHandler.text;
                    Debug.Log("UploadRun Success: " + response);
                    onSuccess?.Invoke(response);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("UploadRun Exception: " + ex.Message);
                onError?.Invoke(ex.Message);
            }
        }
    }

}
