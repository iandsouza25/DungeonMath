using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class changeText : MonoBehaviour
{
    private EquationGenerator equationgenerator;
    public TextMeshProUGUI equationText;

     void Start()
    {   
  
        equationgenerator = FindObjectOfType<EquationGenerator>();

        StartCoroutine(UpdateTextAfterDelay(0.2f));
    }

    IEnumerator UpdateTextAfterDelay(float delay)
    {
        // Wait for the delay
        yield return new WaitForSeconds(delay);

        // Update the text only once after the delau
        if (equationgenerator != null) {
            equationText.text = equationgenerator.GetEquation();
        }
    }

   
}
