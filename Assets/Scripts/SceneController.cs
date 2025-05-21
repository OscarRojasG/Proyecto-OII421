using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    private static List<string> sceneHistory = new List<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantiene el objeto al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // Si ya existe una instancia, destruye esta
        }
    }

    public void ChangeScene(string name)
    {
        // SceneManager.UnloadSceneAsync(name);
        SceneManager.LoadScene(name);
        sceneHistory.Add(name);
    }

    public void PreviousScene()
    {
        if (sceneHistory.Count >= 2)
        {
            string previousSceneIndex = sceneHistory[sceneHistory.Count - 1];
            sceneHistory.RemoveAt(sceneHistory.Count - 1);
            SceneManager.LoadScene(previousSceneIndex);
        }
        else if (sceneHistory.Count == 1)
        {
            sceneHistory.RemoveAt(sceneHistory.Count - 1);
            SceneManager.LoadScene("MainScene");
        }
        else
        {
            Debug.LogWarning("No hay una escena anterior en el historial.");
        }
    }

    public void ClearHistory()
    {
        sceneHistory.Clear();
    }
}