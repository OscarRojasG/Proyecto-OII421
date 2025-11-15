using UnityEngine;
using UnityEngine.Audio;

public class MusicController : MonoBehaviour
{
    public AudioSource audioSource;
    public GameController gameController;
    public AudioResource[] audios = new AudioResource[3];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource.volume = 0.6f;
        gameController = GameController.Instance;
        switch (gameController.GetCurrentLevel())
        {
            case "1":
                audioSource.resource = audios[0];
                break;
            case "2":
                audioSource.resource = audios[1];
                break;
            case "3":
                audioSource.resource = audios[2];
                break;
            default:
                break;
        }
        audioSource.Play();
        audioSource.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
