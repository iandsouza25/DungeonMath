using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LockManager : MonoBehaviour
{
    public GameObject[] slots; 
    public EquationGenerator equationgenerator;
    private List<string> solution = new List<string>();

    public void UpdateInventory(){   
        for (int i = 0; i < slots.Length; i++) {
            if (slots[i].transform.childCount > 0) {
                TextMeshProUGUI keyText = GetComponentInChildren<TextMeshProUGUI>();
                Debug.Log($"Slot {i} contains {keyText.text}");
            }
            else {
                Debug.Log($"Slot {i} is empty");
            }
        }
    }

    public void CheckSolution() {
        for (int i = 0; i < slots.Length; i++) {
            if (slots[i].transform.childCount > 0) {
                TextMeshProUGUI keyText = GetComponentInChildren<TextMeshProUGUI>();
                solution.Add(keyText.text);
            }
        }
        if (solution.Contains("")) {
            Debug.Log("missingParameter");
        }
        else {
            bool equationCorrect = equationgenerator.isEquationCorrect(solution);
            if (equationCorrect) {
            //advance
            }
            else {
            //try again
            }
        }

        
    }

}

