using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using EventHandler = EventSystem.EventHandler;

public class UIText : MonoBehaviour
{
    [SerializeField] private GameObject clickIcon;

    [SerializeField] private int startDelay = 2;
    [SerializeField] private int textDelay = 2;

    private InputAction nextAction;
    private EventHandler eventHandler;
    private Image backgroundImage;
    
    private List<GameObject> boxes = new List<GameObject>();

    private bool allowedToContinue;

    private void Awake()
    {
        foreach (Transform box in transform)
        {
            boxes.Add(box.gameObject);
        }
        
        eventHandler = EventHandler.instance;
        backgroundImage = GetComponent<Image>();
        backgroundImage.enabled = false;
    }

    private void OnEnable()
    {
        StartCoroutine(StartingDelay());
        InvokeRepeating(nameof(IterateBoxes), startDelay + textDelay, textDelay);
    }

    private IEnumerator StartingDelay()
    {
        yield return new WaitForSeconds(startDelay);
        backgroundImage.enabled = true;
        boxes[0].SetActive(true);
    }
    
    // Specify a box to activate and deactivate all other boxes
    private void SetBoxActive(int boxIndex)
    {
        foreach (GameObject box in boxes)
        {
            box.SetActive(false);
        }
        boxes[boxIndex].SetActive(true);
    }
    
    private void IterateBoxes()
    {
        int activeBoxIndex = 0;
        
        foreach (var box in boxes)
        {
            if (box.activeSelf)
            {
                activeBoxIndex = boxes.IndexOf(box);
            }
        }
        
        if (activeBoxIndex < boxes.Count - 1)
        {
            SetBoxActive(activeBoxIndex + 1);
            return;
        }
        if (activeBoxIndex == boxes.Count - 1)
        {
            EndTextSequence();
        }
    }
    
    private void EndTextSequence()
    {
        CancelInvoke(nameof(IterateBoxes));
        
        foreach (GameObject box in boxes)
        {
            box.SetActive(false);
        }
        eventHandler.InvokeEvent(new TextSequenceCompleteEvent(
            completedSequenceName: gameObject.name,
            description: "Text sequence \"" + gameObject.name + "\" complete"
        ));
    }
}
