using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    private int currentLevel;

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

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void SetCurrentLevel(int currentLevel)
    {
        this.currentLevel = currentLevel;
    }

    
}
