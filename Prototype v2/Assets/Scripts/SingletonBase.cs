using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBase : MonoBehaviour
{
    // The instance used when accessing the singleton. Change the type to whatever class you are making
    public static SingletonBase instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one [CLASS NAME] in the scene!");
        }
        else
        {
            instance = this;
        }
    }
}
