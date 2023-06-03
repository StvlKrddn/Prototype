using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;
using FMODUnity;
using UnityEngine.Serialization;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private EventReference menuMusic;
    [SerializeField] private float fadeDuration;
    
    private EventInstance menuMusicEventInstance;

    private float fadeTimer = 0f;
    private bool isFading;
    
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        
        // Create an FMOD event instance that Unity can manipulate
        menuMusicEventInstance = RuntimeManager.CreateInstance(menuMusic);
            
        // Get the current playback state of the event instance
        menuMusicEventInstance.getPlaybackState(out PLAYBACK_STATE playbackState);
        
        // If the event instance is not currently playing...
        if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
        {
            // Play the event instance
            menuMusicEventInstance.start();
        }

        menuMusicEventInstance.setVolume(0);
        fadeTimer = 0f;
        isFading = true;
    }

    private void OnDestroy()
    {
        menuMusicEventInstance.stop(STOP_MODE.IMMEDIATE);
    }

    private void Update()
    {
        // menuMusicEventInstance.getVolume(out float musicVolume);
        if (isFading)
        {
            MusicFader();
        }
    }
    
    private void MusicFader()
    {
        fadeTimer += Time.deltaTime;
        
        // Calculate the current volume based on the fade progress
        float currentVolume = fadeTimer / fadeDuration;
            
        // Set the volume of the event instance
        menuMusicEventInstance.setVolume(currentVolume);
            
        // Check if the fade-in is complete
        if (fadeTimer >= fadeDuration)
        {
            isFading = false;
        }
    }

    public void PlayGame()
    {
        StartCoroutine(LoadScene());
        
        IEnumerator LoadScene()
        {
            AsyncOperation asyncLooad = SceneManager.LoadSceneAsync(1);

            while (!asyncLooad.isDone)
            {
                yield return null;
            }
        }
    }
    
    public void QuitGame()
    {
        // For editor
        //UnityEditor.EditorApplication.isPlaying = false;

        // For builds
        Application.Quit();
        
    }
}
