using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    public GameObject canvas;
    public Collider other;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
    {
    // Show Door UI
        canvas.SetActive(true);
    } 

    }

    private void OnTriggerExit(Collider other)
    {
    if (other.CompareTag("Player")) 
    {
    // Show Door UI
        canvas.SetActive(false);
    }   

    }
}
