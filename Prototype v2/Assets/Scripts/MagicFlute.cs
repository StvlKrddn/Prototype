using System;
using System.Collections;
using System.Collections.Generic;
using EventSystem;
using UnityEngine;
using EventHandler = EventSystem.EventHandler;

public class MagicFlute : MonoBehaviour
{
    private EventHandler eventHandler;

    private void Awake()
    {
        eventHandler = EventHandler.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        eventHandler.InvokeEvent(new ObjectiveCompleteEvent(
            completedObjective: gameObject,
            description: gameObject.name + " was collected"
            ));
    }
}
