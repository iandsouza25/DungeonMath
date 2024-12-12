using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject[] slots; 

    public void UpdateInventory()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].transform.childCount > 0)
            {
                GameObject item = slots[i].transform.GetChild(0).gameObject;
                Debug.Log($"Slot {i} contains {item.name}");
            }
            else
            {
                Debug.Log($"Slot {i} is empty");
            }
        }
    }
}
