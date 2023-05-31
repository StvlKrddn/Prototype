using System;
using System.Collections;
using System.Collections.Generic;
using EventSystem;
using EventHandler = EventSystem.EventHandler;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    
    [SerializeField] private bool showIntroduction;
    [SerializeField] private GameObject objectivesContainer;
    [SerializeField] private PlayerInput playerInput;

    public Queue<GameObject> ObjectivesQueue { get; private set; }
    public GameObject CurrentObjective { get; private set; }
    public bool GameIsActive { get; private set; }
    public int Points { get; private set; }
    public bool ShowIntroduction { get; private set; }

    private EventHandler eventHandler;

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
        GameIsActive = true;

        foreach (Transform objective in objectivesContainer.transform)
        {
            ObjectivesQueue.Enqueue(objective.gameObject);
            objective.gameObject.SetActive(false);
        }

        CurrentObjective = ObjectivesQueue.Peek();
        CurrentObjective.SetActive(true);

        ShowIntroduction = showIntroduction;
        
        LockPlayerMovement(true);
    }
    
    private void OnEnable()
    {
        eventHandler.RegisterListener<ObjectiveCompleteEvent>(OnObjectiveComplete);
        eventHandler.RegisterListener<GamePausedEvent>(OnGamePaused);
        eventHandler.RegisterListener<IntroSequenceCompleteEvent>(OnIntroComplete);
    }

    private void OnDestroy()
    {
        eventHandler.UnregisterListener<ObjectiveCompleteEvent>(OnObjectiveComplete);
        eventHandler.UnregisterListener<GamePausedEvent>(OnGamePaused);
        eventHandler.UnregisterListener<IntroSequenceCompleteEvent>(OnIntroComplete);
    }

    private void OnObjectiveComplete(ObjectiveCompleteEvent eventInfo)
    {
        print(eventInfo.Description);

        Points++;

        ObjectivesQueue.Dequeue().SetActive(false);
        
        if (ObjectivesQueue.Count == 0)
        {
            EndGame();
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
        GameIsActive = !eventInfo.gamePaused;

        LockPlayerMovement(!GameIsActive);
        Cursor.lockState = GameIsActive ? CursorLockMode.Locked : CursorLockMode.None;
    }

    private void OnIntroComplete(IntroSequenceCompleteEvent eventInfo)
    {
        LockPlayerMovement(false);
    }
    
    private IEnumerator IntroductionSequence()
    {
        yield return new WaitForSeconds(2 + 5 * 4);
        LockPlayerMovement(false);
    }
    
    private void LockPlayerMovement(bool state)
    {
        // If state is true
        if (state)
        {
            // Deactivate player inputs
            playerInput.DeactivateInput();
        }
        else
        {
            // Activate player input
            playerInput.ActivateInput();
        }
        
    }
    
    private void EndGame()
    {
        GameIsActive = false;
        eventHandler.InvokeEvent(new GameEndEvent(
            description: "All objectives have been found"
            ));
    }
}
