using System;
using System.Collections;
using System.Collections.Generic;
using EventSystem;
using TMPro;
using UnityEngine;
using EventHandler = EventSystem.EventHandler;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI collectedObjectivesText;

    private GameManager gameManager;
    private EventHandler eventHandler;
    
    private int totalObjectives;
    private int collectedObjectives;
    
    private void Awake()
    {
        eventHandler = EventHandler.instance;
        gameManager = GameManager.instance;

        totalObjectives = gameManager.ObjectivesQueue.Count;
    }

    private void OnEnable()
    {
        eventHandler.RegisterListener<ObjectiveCompleteEvent>(OnObjectiveCompleted);
    }

    private void OnDisable()
    {
        eventHandler.UnregisterListener<ObjectiveCompleteEvent>(OnObjectiveCompleted);
    }

    private void OnObjectiveCompleted(ObjectiveCompleteEvent eventInfo)
    {
        collectedObjectives++;
    }
    
    // Update is called once per frame
    void Update()
    {
        collectedObjectivesText.text = "Flutes collected: " + collectedObjectives + "/" + totalObjectives;
    }
}
