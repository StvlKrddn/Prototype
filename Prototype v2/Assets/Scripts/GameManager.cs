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
    
    [SerializeField] private GameObject objectivesContainer;
    [SerializeField] private GameObject player;

    public Queue<GameObject> ObjectivesQueue { get; private set; }
    public GameObject CurrentObjective { get; private set; }
    public bool GameIsActive { get; private set; }
    public int Points { get; private set; }

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
    }
    
    private void OnEnable()
    {
        eventHandler.RegisterListener<ObjectiveCompleteEvent>(OnObjectiveComplete);
    }

    private void OnDisable()
    {
        eventHandler.UnregisterListener<ObjectiveCompleteEvent>(OnObjectiveComplete);
    }
    
    public void LockPlayerMovement(bool state)
    {
        PlayerInput playerInput = player.GetComponent<PlayerInput>();
        
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
    
    private void EndGame()
    {
        GameIsActive = false;
        eventHandler.InvokeEvent(new GameEndEvent(
            description: "All objectives have been found"
            ));
    }
}
