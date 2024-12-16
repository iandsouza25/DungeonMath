using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class changeText : MonoBehaviour
{
    public EquationGenerator equationgenerator;
    public TextMeshProUGUI equationText;

     void Start()
    {   
        StartCoroutine(UpdateTextAfterDelay(1f));
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
