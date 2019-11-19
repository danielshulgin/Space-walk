using System;
using System.Collections;
using System.Collections.Generic;
using ItemSystem;
using UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotsUI : MonoBehaviour
{
    public SlotHandler SelectedSlotHandler { get; private set; }

    public bool Initialized { get; private set; }

    [HideInInspector]
    public EntityStuff entityStuff;

    public GameObject player;
    public GameObject slotPrefab;
    public GameObject slotsParent;
//TODO slots handlers
    [SerializeField] private SlotHandler hadSlot;
    [SerializeField] private SlotHandler bodySlot;
    [SerializeField] private SlotHandler lagsSlot;

    private List<SlotHandler> _bagSlotHandlers;

    private void Awake()
    {
        entityStuff = player.GetComponent<EntityStuff>();
        _bagSlotHandlers = new List<SlotHandler>();
        entityStuff.OnEntityStuffChanged += UpdateBagSlots;
    }
    
    public void InitializeSlots()
    {
        InitializeBagSlots();
        Initialized = true;
        new List<SlotHandler>(){hadSlot, bodySlot, lagsSlot}.ForEach(InitializeSingleSlot);
    }

    private void InitializeSingleSlot(SlotHandler slotHandler)
    {
        slotHandler.Initialize(slotHandler.SlotType);
        var itemDragHandler = slotHandler.GetComponentInChildren<ItemDragHandler>();
        itemDragHandler.OnItemStartHandling += ItemStartHandling;
        itemDragHandler.OnDropItemInSlot += DropItemInSlot;
        UpdateSlot(slotHandler);
    }

    public void InitializeBagSlots()
    {
        foreach (Transform child in slotsParent.transform) 
        {
            GameObject.Destroy(child.gameObject);
        }
        for (var i = 0; i < entityStuff._inventory.ItemsInSlots.Length; i++)
        {
            var slotGameObject = Instantiate(slotPrefab, slotsParent.transform, true);
            var slotHandler = slotGameObject.GetComponent<SlotHandler>();
            slotHandler.Initialize(SlotType.Bag, i);
            _bagSlotHandlers.Add(slotHandler);
            var itemDragHandler = slotGameObject.GetComponentInChildren<ItemDragHandler>();
            itemDragHandler.OnItemStartHandling += ItemStartHandling;
            itemDragHandler.OnDropItemInSlot += DropItemInSlot;
        }

        UpdateBagSlots();
    }

    public void UpdateBagSlots()
    {
        for (int i = 0; i < _bagSlotHandlers.Count; i++)
        {
            UpdateSlot(_bagSlotHandlers[i]);
        }
    }

    public void ItemStartHandling(SlotHandler slotHandler)
    {
        SelectedSlotHandler = slotHandler;
    }
    
    public void DropItemInSlot(SlotHandler slotHandler)
    {
        if (SelectedSlotHandler != null)
        {
            entityStuff.ShiftFromSlotToSlot(SelectedSlotHandler, slotHandler);
            UpdateSlot(slotHandler);
            UpdateSlot(SelectedSlotHandler);
        }
    }
    
    public void DropSelectedItem()
    {
        if (SelectedSlotHandler != null)
        {
            entityStuff.DropItem(entityStuff.GetSlotGuid(SelectedSlotHandler));
            UpdateSlot(SelectedSlotHandler);
        }
    }

    public void UpdateSlot(SlotHandler slotHandler)
    {
        var itemId = entityStuff.GetSlotGuid(slotHandler);
        if (itemId != Guid.Empty)
        {
            var baseItem = DataBase.instance.GetItem(itemId);
            
            var itemSprite = baseItem.ScriptableObject.inventorySprite;
            slotHandler.EnableSlotImage(itemSprite);
            
            if (baseItem is ItemStack itemStack)
            {
                slotHandler.SetNumber(itemStack.Number);
                slotHandler.SetActiveNumberText(true);
            }
            else
            {
                slotHandler.SetActiveNumberText(false);
            }
        }
        else
        {
            slotHandler.DisableSlotImage();
        }
    }
}
