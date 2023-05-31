using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialougeScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> boxes;
    [SerializeField] private GameObject instructionObject;
    [SerializeField] private GameObject collectedObject;
    
    [SerializeField] private int startDelay = 2;
    [SerializeField] private int scrollSpeed = 5;

    private void Awake()
    {
        StartCoroutine(StartDialouge());
        InvokeRepeating(nameof(IterateBox), startDelay + scrollSpeed, scrollSpeed);
    }

    private IEnumerator StartDialouge()
    {
        yield return new WaitForSeconds(startDelay);
        boxes[0].transform.parent.gameObject.SetActive(true);
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
                box.transform.parent.gameObject.SetActive(false);
            }
            CancelInvoke(nameof(IterateBox));
            instructionObject.SetActive(true);
            collectedObject.SetActive(true);
        }
    }
}
