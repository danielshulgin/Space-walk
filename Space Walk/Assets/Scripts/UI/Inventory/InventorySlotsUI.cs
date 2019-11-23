using System;
using System.Collections;
using System.Collections.Generic;
using Entity.Stuff;
using ItemSystem;
using UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotsUI : MonoBehaviour
{
    public event Action<SlotUIData> OnDescriptionCalled = (id) => { }; 
    public SlotHandlerUI SelectedSlotHandler { get; private set; }

    public bool Initialized { get; private set; }

    [HideInInspector]
    public EntityStuffComponent entityStuff;

    public GameObject player;
    public GameObject slotPrefab;
    public GameObject slotsParent;

    [SerializeField] private SlotHandlerUI hadSlot;
    [SerializeField] private SlotHandlerUI bodySlot;
    [SerializeField] private SlotHandlerUI lagsSlot;

    private List<SlotHandlerUI> _bagSlotHandlers;

    private void Start()
    {
        entityStuff = player.GetComponent<EntityStuffComponent>();
        _bagSlotHandlers = new List<SlotHandlerUI>();
        entityStuff.Stuff.OnUpdateSlot += (type, i) => UpdateSlot(_bagSlotHandlers[i]);
    }
    
    public void InitializeSlots()
    {
        InitializeBagSlots();
        Initialized = true;
        new List<SlotHandlerUI>(){hadSlot, bodySlot, lagsSlot}.ForEach(InitializeSingleSlot);
    }

    private void InitializeSingleSlot(SlotHandlerUI slotHandler)
    {
        slotHandler.Initialize(slotHandler.Type);
        SubscribeToSlotEvents(slotHandler.GetComponentInChildren<ItemDragHandler>());
        UpdateSlot(slotHandler);
    }

    public void InitializeBagSlots()
    {
        foreach (Transform child in slotsParent.transform) 
        {
            GameObject.Destroy(child.gameObject);
        }
        for (var i = 0; i < entityStuff.Stuff.BagSlotsNumber; i++)
        {
            var slotGameObject = Instantiate(slotPrefab, slotsParent.transform, true);
            var slotHandler = slotGameObject.GetComponent<SlotHandlerUI>();
            slotHandler.Initialize(SlotType.Bag, i);
            _bagSlotHandlers.Add(slotHandler);
            SubscribeToSlotEvents(slotGameObject.GetComponentInChildren<ItemDragHandler>());
        }

        UpdateBagSlots();
    }

    private void SubscribeToSlotEvents(ItemDragHandler itemDragHandler)
    {
        itemDragHandler.OnItemStartHandling += ItemStartHandling;
        itemDragHandler.OnDropItemInSlot += DropItemInSlot;
        itemDragHandler.OnDescriptionCalled += () =>
        {
            OnDescriptionCalled(entityStuff.Stuff.GetSlotDescription(SelectedSlotHandler.Type, SelectedSlotHandler.Index));
        };
    }

    public void UpdateBagSlots()
    {
        for (int i = 0; i < _bagSlotHandlers.Count; i++)
        {
            UpdateSlot(_bagSlotHandlers[i]);
        }
    }

    public void ItemStartHandling(SlotHandlerUI slotHandler)
    {
        SelectedSlotHandler = slotHandler;
    }
    
    public void DropItemInSlot(SlotHandlerUI slotHandler)
    {
        if (SelectedSlotHandler != null)
        {
            entityStuff.Stuff.SwapSlots(slotHandler.Type, slotHandler.Index,
                SelectedSlotHandler.Type, SelectedSlotHandler.Index);
            UpdateSlot(slotHandler);
            UpdateSlot(SelectedSlotHandler);
        }
    }
    
    public void DropSelectedItem()
    {
        if (SelectedSlotHandler != null)
        {
            entityStuff.Stuff.DropItem(SelectedSlotHandler.Type, SelectedSlotHandler.Index);
            UpdateSlot(SelectedSlotHandler);
        }
    }

    private void UpdateSlot(SlotHandlerUI slotHandler)
    {
        var slotData = entityStuff.Stuff.GetSlotDescription(slotHandler.Type, slotHandler.Index);
        slotHandler.UpdateSlot(slotData);
    }
}
