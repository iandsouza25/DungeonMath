using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    public GameObject canvas;
    public Collider other;

    public GameObject slot;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
    {
    // Show Door UI
        canvas.SetActive(true);
        if (GameManager.currentLevel == 1 || GameManager.currentLevel == 3){
            slot.SetActive(false);
        }
        else{
            slot.SetActive(true);
        }
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
