using UnityEngine;
using UnityEngine.Audio;

public class MusicController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioMixer mixer;
    public AudioResource[] audios = new AudioResource[3];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameController gameController = GameController.Instance;
        float dB = Mathf.Log10(gameController.GetVolume()) * 20f;
        mixer.SetFloat("MasterVolume", dB);

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
