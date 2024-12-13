using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    public GameObject canvas
    private bool canvasActive = false;
    // wip
    // Start is called before the first frame update
    void Start()
    {
        if (canvas != null)
        {
            canvas.SetActive(false);  
        }
    }

    // Update is called once per frame
     void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))  
        {
            if (canvas != null) {
                canvasActive = !canvasActive;  
                canvas.SetActive(canvasActive);  
            }
        }
    }
}
