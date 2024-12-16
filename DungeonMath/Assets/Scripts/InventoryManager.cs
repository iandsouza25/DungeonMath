using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public GameObject[] slots; 
    public EquationGenerator equationgenerator;

    public void UpdateInventory() {   
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

    public void AddInventory(List<string> val) {
        Debug.Log(val[0]);
        for (int i = 0; i < val.Count; i++) {
            if (slots[i].transform.childCount > 0) {
                TextMeshProUGUI keyText = slots[i].transform.GetComponentInChildren<TextMeshProUGUI>();
                if (keyText.text == "") {
                    keyText.text = val[i];
                }
            }
        }
    }
}

