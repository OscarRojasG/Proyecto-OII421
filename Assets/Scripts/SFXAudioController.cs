using UnityEngine;
using UnityEngine.Audio;

public class SFXAudioController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip buttonClickAudio;
    public AudioClip choqueAudio;
    public AudioClip gameOverAudio;
    public AudioClip respuestaCorrectaAudio;
    public AudioClip tuboEnsayoAudio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playRespuestaCorrecta()
    {
        audioSource.PlayOneShot(respuestaCorrectaAudio);
    }

    public void playGameOver()
    {
        audioSource.PlayOneShot(gameOverAudio);
    }

    public void playChoque()
    {
        audioSource.PlayOneShot(choqueAudio);
    }  

    public void playButtonClick()
    {
        audioSource.PlayOneShot(buttonClickAudio);
    }

    public void playTuboEnsayo()
    {
        audioSource.PlayOneShot(tuboEnsayoAudio);
    }
}
