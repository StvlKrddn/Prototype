using System;
using System.Collections;
using System.Collections.Generic;
using EventSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using EventHandler = EventSystem.EventHandler;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI collectedObjectivesText;

    [SerializeField] private GameObject instructionObject;
    [SerializeField] private GameObject collectedObject;
    [SerializeField] private GameObject introText;
    [SerializeField] private GameObject endText;
    [FormerlySerializedAs("instructionPanel")] [SerializeField] private GameObject tutorialPanel;

    private GameManager gameManager;
    private EventHandler eventHandler;
    
    private int totalObjectives;
    private int collectedObjectives;
    
    private void Awake()
    {
        eventHandler = EventHandler.instance;
        gameManager = GameManager.instance;

        totalObjectives = gameManager.ObjectivesQueue.Count;
        collectedObjectivesText.text = "Flutes collected: " + collectedObjectives + "/" + totalObjectives;

        if (gameManager.ShowIntroduction)
        {
            Cursor.lockState = CursorLockMode.None;
            tutorialPanel.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            introText.SetActive(true);
        }
    }
    
    public void Continue()
    {
        tutorialPanel.SetActive(false);
        instructionObject.SetActive(true);
        collectedObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        eventHandler.InvokeEvent(new TextSequenceCompleteEvent(
            completedSequenceName: "Tutorial",
            description: "Tutorial finished"
            ));
    }

    private void OnEnable()
    {
        eventHandler.RegisterListener<ObjectiveCompleteEvent>(OnObjectiveCompleted);
        eventHandler.RegisterListener<TextSequenceCompleteEvent>(OnTextSequenceComplete);
        eventHandler.RegisterListener<GameEndEvent>(OnGameEnd);
    }

    private void OnDisable()
    {
        eventHandler.UnregisterListener<ObjectiveCompleteEvent>(OnObjectiveCompleted);
        eventHandler.UnregisterListener<TextSequenceCompleteEvent>(OnTextSequenceComplete);
        eventHandler.UnregisterListener<GameEndEvent>(OnGameEnd);
    }

    private void OnObjectiveCompleted(ObjectiveCompleteEvent eventInfo)
    {
        collectedObjectives++;
        collectedObjectivesText.text = "Flutes collected: " + collectedObjectives + "/" + totalObjectives;
    }

    private void OnTextSequenceComplete(TextSequenceCompleteEvent eventInfo)
    {
        if (eventInfo.CompletedSequenceName.Equals("Introduction"))
        {
            instructionObject.SetActive(true);
            collectedObject.SetActive(true);
            introText.SetActive(false);
        }
        else
        {
            endText.SetActive(false);
        }
    }
    
    private void OnGameEnd(GameEndEvent eventInfo)
    {
        instructionObject.SetActive(false);
        collectedObject.SetActive(false);
        endText.SetActive(true);
    }
}
