using System;
using System.Collections;
using System.Collections.Generic;
using EventSystem;
using EventHandler = EventSystem.EventHandler;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    
    [SerializeField] private GameObject objectivesContainer;

    public Queue<GameObject> ObjectivesQueue { get; private set; }
    public GameObject CurrentObjective { get; private set; }

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

        foreach (GameObject objective in objectivesContainer.transform)
        {
            ObjectivesQueue.Enqueue(objective);
        }

        CurrentObjective = ObjectivesQueue.Peek();
    }
    
    private void OnEnable()
    {
        eventHandler.RegisterListener<ObjectiveCompleteEvent>();
    }


    void Update()
    {
        
    }
}
