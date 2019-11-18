using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryBackground : MonoBehaviour,IDropHandler,IPointerDownHandler
{
    public EntityStuff entityStuff;
    public bool active = true;
    public void OnDrop(PointerEventData eventData)
    {
        if ((ItemDragHandler.selectedSlotIndex != -1) && active)
        {
            entityStuff._inventory.DropItemOnGround(entityStuff._inventory.ItemsInSlots[ItemDragHandler.selectedSlotIndex]);
            GetComponent<InventoryUI>().UpdateUI();
        }
    }

    //TODO remove and add second background for disable button
    public void OnPointerDown(PointerEventData eventData)
    {
        
    }
}
