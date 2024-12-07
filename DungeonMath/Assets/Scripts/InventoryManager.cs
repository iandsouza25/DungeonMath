using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryUI;     // Reference to the inventory UI
    public Transform itemContainer;   
    public GameObject itemPrefab;     

    private List<string> inventory = new List<string>(); // Inventory list

    public void AddItem(string item)
    {
        inventory.Add(item);
        UpdateInventoryUI();
    }

    public void RemoveItem(string item)
    {
        if (inventory.Contains(item))
        {
            inventory.Remove(item);
            UpdateInventoryUI();
        }
    }

    void UpdateInventoryUI()
    {
        // Clear existing items in the UI
        foreach (Transform child in itemContainer)
        {
            Destroy(child.gameObject);
        }

        // Populate UI with current inventory items
        foreach (string item in inventory)
        {
            GameObject newItem = Instantiate(itemPrefab, itemContainer);
            newItem.GetComponentInChildren<Text>().text = item; // Display item name
        }
    }
}