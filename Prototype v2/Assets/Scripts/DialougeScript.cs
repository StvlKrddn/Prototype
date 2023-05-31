using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using EventHandler = EventSystem.EventHandler;

public class DialougeScript : MonoBehaviour
{
    [SerializeField] private Transform boxContainer;
    [SerializeField] private GameObject clickIcon;
    [SerializeField] private GameObject instructionObject;
    [SerializeField] private GameObject collectedObject;
    
    [SerializeField] private int clickDelay = 2;

    private InputAction nextAction;
    private EventHandler eventHandler;
    
    private List<GameObject> boxes = new List<GameObject>();

    private bool allowedToContinue;

    private void Awake()
    {
        foreach (Transform box in boxContainer)
        {
            boxes.Add(box.gameObject);
        }
        
        eventHandler = EventHandler.instance;
        
        //StartCoroutine(StartDialouge());
        //InvokeRepeating(nameof(IterateBox), startDelay + scrollSpeed, scrollSpeed);
        
        boxContainer.parent.gameObject.SetActive(true);
        boxes[0].SetActive(true);

        StartCoroutine(ClickDelay());
    }

    private void OnEnable()
    {
        nextAction = new InputAction("nextAction", binding: "mouse/leftButton");
        nextAction.performed += OnMouseClicked;
        nextAction.Enable();
    }

    private void OnDestroy()
    {
        nextAction.performed -= OnMouseClicked;
        nextAction.Disable();
    }

    private void OnMouseClicked(InputAction.CallbackContext context)
    {
        if (allowedToContinue)
        {
            StartCoroutine(ClickDelay());
            IterateBox();
        }
    }

    private IEnumerator ClickDelay()
    {
        allowedToContinue = false;
        clickIcon.SetActive(false);
        yield return new WaitForSeconds(clickDelay);
        clickIcon.SetActive(true);
        allowedToContinue = true;
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
    
    private void IterateBox()
    {
        int activeBoxIndex = 0;
        foreach (GameObject box in boxes)
        {
            if (box.activeSelf)
            {
                activeBoxIndex = boxes.IndexOf(box);
            }
        }

        if (activeBoxIndex < boxes.Count - 1)
        {
            SetBoxActive(++activeBoxIndex);
        }
        else if (activeBoxIndex == boxes.Count - 1)
        {
            foreach (GameObject box in boxes)
            {
                box.SetActive(false);
                boxContainer.parent.gameObject.SetActive(false);
            }
            CancelInvoke(nameof(IterateBox));
            instructionObject.SetActive(true);
            collectedObject.SetActive(true);
            
            eventHandler.InvokeEvent(new IntroSequenceCompleteEvent(
                description: "Intro sequence complete"
                ));
        }
    }
}
