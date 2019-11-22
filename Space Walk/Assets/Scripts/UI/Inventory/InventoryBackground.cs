using System;
using System.Collections;
using System.Collections.Generic;
using Entity.Stuff;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryBackground : MonoBehaviour,IDropHandler//,IPointerDownHandler
{
    public EntityStuffComponent entityStuff;
    public bool active = true;
    [SerializeField] private InventorySlotsUI inventoryUi;
    
    public void OnDrop(PointerEventData eventData)
    {
        /*inventoryUi.DropSelectedItem();*/
    }
}
