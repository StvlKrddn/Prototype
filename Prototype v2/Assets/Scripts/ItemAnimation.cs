using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemAnimation : MonoBehaviour
{
    [Header("Bobbing")] 
    [SerializeField] private bool bobbing;
    [SerializeField] private float bobbingAmplitude;
    [SerializeField] private float bobbingSpeed;

    [Header("Rotation")] 
    [SerializeField] private bool rotating;
    [SerializeField] private float rotationSpeed;
    
    // Caching transform is more efficient 
    private Transform itemTransform;
    private float startY;
    
    // Start is called before the first frame update
    void Start()
    {
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        itemTransform = transform;
        if (bobbing)
        {
            BobbingAnimation();
        }

        if (rotating)
        {
            RotatingAnimation();
        }
    }

    // Moves item up and down in a sign-curve
    private void BobbingAnimation()
    {
        var currentPos = itemTransform.position;
        currentPos = new Vector3(currentPos.x, startY + bobbingAmplitude * Mathf.Sin(bobbingSpeed * Time.time), currentPos.z);
        itemTransform.position = currentPos;
    }
    
    // Rotates item around its Y-axis
    private void RotatingAnimation()
    {
        itemTransform.Rotate(0, rotationSpeed * 20 * Time.deltaTime, 0);
    }
    
}
