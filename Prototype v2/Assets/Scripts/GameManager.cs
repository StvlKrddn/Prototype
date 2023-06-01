using System;
using System.Collections;
using System.Collections.Generic;
using EventSystem;
using StarterAssets;
using EventHandler = EventSystem.EventHandler;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    
    [SerializeField] private bool showIntroduction;
    [SerializeField] private GameObject objectivesContainer;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject resetPoint;

    public Queue<GameObject> ObjectivesQueue { get; private set; }
    public GameObject CurrentObjective { get; private set; }
    public int Points { get; private set; }
    public bool ShowIntroduction { get; private set; }

    private EventHandler eventHandler;
    private bool isTutorialPlaying = true;
    private FirstPersonController controller;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one GameManager in the scene!");
        }
        else
        {
            instance = this;
        }
        
        eventHandler = EventHandler.instance;
        ObjectivesQueue = new Queue<GameObject>();

        foreach (Transform objective in objectivesContainer.transform)
        {
            ObjectivesQueue.Enqueue(objective.gameObject);
            objective.gameObject.SetActive(false);
        }

        CurrentObjective = ObjectivesQueue.Peek();
        CurrentObjective.SetActive(true);
        
        controller = player.GetComponent<FirstPersonController>();

        ShowIntroduction = showIntroduction;

        LockPlayerMovement(ShowIntroduction);
        isTutorialPlaying = ShowIntroduction;
    }
    
    private void OnEnable()
    {
        eventHandler.RegisterListener<ObjectiveCompleteEvent>(OnObjectiveComplete);
        eventHandler.RegisterListener<GamePausedEvent>(OnGamePaused);
        eventHandler.RegisterListener<TextSequenceCompleteEvent>(OnTextSequenceComplete);
    }

    private void OnDestroy()
    {
        eventHandler.UnregisterListener<ObjectiveCompleteEvent>(OnObjectiveComplete);
        eventHandler.UnregisterListener<GamePausedEvent>(OnGamePaused);
        eventHandler.UnregisterListener<TextSequenceCompleteEvent>(OnTextSequenceComplete);
    }

    private void OnObjectiveComplete(ObjectiveCompleteEvent eventInfo)
    {
        print(eventInfo.Description);

        Points++;

        ObjectivesQueue.Dequeue().SetActive(false);
        
        if (ObjectivesQueue.Count == 0)
        {
            StartEndSequence();
            return;
        }
        
        GameObject nextObjective = ObjectivesQueue.Peek();
        nextObjective.SetActive(true);
        
        eventHandler.InvokeEvent(new NewObjectiveEvent(
            newObjective: nextObjective,
            description: nextObjective + " is the new objective"
            ));
    }
    
    private void OnGamePaused(GamePausedEvent eventInfo)
    {
        bool isGamePaused = eventInfo.gamePaused;
        
        if (showIntroduction)
        {
            // Game is paused
            if (isGamePaused)
            {
                LockPlayerMovement(true);
                Cursor.lockState = CursorLockMode.None;
                return;
            }
            
            // Game is unpaused but tutorial is still playing
            if (!isGamePaused && isTutorialPlaying)
            {
                LockPlayerMovement(true);
                Cursor.lockState = CursorLockMode.None;
                return;
            }
            
            // Game is unpaused and tutorial is not playing
            if (!isGamePaused && !isTutorialPlaying)
            {
                LockPlayerMovement(false);
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        else
        {
            LockPlayerMovement(isGamePaused);
            
            Cursor.lockState = isGamePaused ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    private void OnTextSequenceComplete(TextSequenceCompleteEvent eventInfo)
    {
        if (eventInfo.CompletedSequenceName.Equals("Tutorial"))
        {
            isTutorialPlaying = false;
            LockPlayerMovement(false);
        }
        else
        {
            StartCoroutine(ShutDownTimer());
        }
        
        IEnumerator ShutDownTimer()
        {
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(0);
        }
    }

    private void LockPlayerMovement(bool state)
    {
        controller.enabled = !state;
    }
    
    private void StartEndSequence()
    {
        LockPlayerMovement(true);
        
        eventHandler.InvokeEvent(new GameEndEvent(
            description: "All objectives have been found"
            ));
    }
}
