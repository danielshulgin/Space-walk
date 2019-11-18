using System;
using System.Collections;
using System.Collections.Generic;
using ItemSystem;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [HideInInspector]
    public EntityStuff entityStuff;

    public GameObject player;
    public GameObject slotPrefab;
    public GameObject slotsParent;

    private void Awake()
    {
        entityStuff = player.GetComponent<EntityStuff>();
        //UpdateUI();
        //TODO in on enable
        entityStuff._inventory.OnInventoryChanged += UpdateUI;
    }

    public void OnEnable()
    {
        UpdateUI();
    }
//TODO separate instantiation and update of the ui 
    public void UpdateUI()
    {
        //TODO remove, replace with more optimal way
        foreach (Transform child in slotsParent.transform) 
        {
            GameObject.Destroy(child.gameObject);
        }
        for (var i = 0; i < entityStuff._inventory.ItemsInSlots.Length; i++)
        {
            var itemId = entityStuff._inventory.ItemsInSlots[i];
            Sprite itemSprite = null;
            var slotGameObject = Instantiate(slotPrefab, slotsParent.transform, true);
            if (itemId != Guid.Empty)
            {
                var baseItem = DataBase.instance.GetItem(itemId);
                itemSprite = baseItem.ScriptableObject.inventorySprite;
                if (baseItem is ItemStack itemStack)
                {
                    var slotHandler = slotGameObject.GetComponent<SlotHandler>();
                    slotHandler.SetNumber(itemStack.Number);
                    slotHandler.SetActiveNumberText(true);
                    //TODO disable when update
                }
                slotGameObject.GetComponentInChildren<CanvasGroup>().alpha = 1f;
            }
            else
            {
                slotGameObject.GetComponentInChildren<CanvasGroup>().alpha = 0f;
            }

            var slotUi = slotGameObject.GetComponentInChildren<ItemDragHandler>();
            slotUi.slotIndex = i;
            slotUi.inventoryUi = this;
            slotGameObject.transform.GetChild(0).GetComponent<Image>().overrideSprite = itemSprite;
        }
    }
}
