using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    [SerializeField] private float speed;
    void Update()
    {
        transform.Rotate(transform.up, speed * Time.deltaTime);
    }
}
