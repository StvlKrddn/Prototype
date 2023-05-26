using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using EventHandler = EventSystem.EventHandler;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject hud;

    private EventHandler eventHandler;
    private InputAction pauseAction;
    
    private void Awake()
    {
        pauseScreen.SetActive(false);
        hud.SetActive(true);
        
        eventHandler = EventHandler.instance;
    }

    private void OnEnable()
    {
        pauseAction = new InputAction("pauseAction", binding: "keyboard/escape");
        pauseAction.performed += OnPausePerformed;
        pauseAction.Enable();
    }

    private void OnDestroy()
    {
        pauseAction.performed -= OnPausePerformed;
        pauseAction.Disable();
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        PauseToggle();
    }
    
    public void PauseToggle()
    {
        ToggleScreen();
        
        bool isPaused = pauseScreen.activeSelf;

        eventHandler.InvokeEvent(new GamePausedEvent(
            gamePaused: isPaused,
            description: "Escape was pressed"
        ));
        
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
    }
    
    public void QuitGame()
    {
        StartCoroutine(LoadScene());
        
        IEnumerator LoadScene()
        {
            AsyncOperation asyncLooad = SceneManager.LoadSceneAsync(0);

            while (!asyncLooad.isDone)
            {
                yield return null;
            }
        }
    }

    private void ToggleScreen()
    {
        hud.SetActive(pauseScreen.activeSelf);
        pauseScreen.SetActive(!pauseScreen.activeSelf);
    }
}
