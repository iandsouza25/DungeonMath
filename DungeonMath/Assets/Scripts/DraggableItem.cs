using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // public Image image;
    public CanvasGroup canvasGroup;
    [HideInInspector] public Transform parentAfterDrag;
    public InventoryManager inventorymanager;
    public LockManager lockmanager;

    public void OnBeginDrag(PointerEventData eventData) {

        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        // image.raycastTarget = false;
        canvasGroup.blocksRaycasts = false;

    }
    public void OnDrag(PointerEventData eventData) {
     
        transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData) {
        transform.SetParent(parentAfterDrag);
        // image.raycastTarget = true;
        canvasGroup.blocksRaycasts = true;
        Debug.Log(inventorymanager != null);
        Debug.Log(lockmanager != null);
        if (inventorymanager != null && lockmanager != null)
        {
            inventorymanager.UpdateInventory(); 
            lockmanager.UpdateInventory();
        }
    }
}
