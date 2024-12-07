using UnityEngine;

public class ItemCollect : MonoBehaviour
{
    public string itemType;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InventoryManager inventory = FindObjectOfType<InventoryManager>();
            if (inventory != null)
            {
                inventory.AddItem(itemType);
            }
            Destroy(gameObject);
        }
    }
}