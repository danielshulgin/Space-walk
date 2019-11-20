﻿using System;
using System.Collections.Generic;
using ItemSystem;
using ItemSystem.ScriptableObjects.ConcreteScriptableObjects;
using UI;
using UnityEngine;

//TODO remove monobehaviour
public class EntityStuffComponent : MonoBehaviour
{
    //TODO remove
    [SerializeField] private ItemStackScriptableObject _bulletScriptableObject;
    [SerializeField] private GunScriptableObject _gunScriptableObject;
    
    //TODO event for single item changed
    public event Action OnEntityStuffChanged = () => { };
    public event Action<SlotType, int> OnEntityStuffSlotChanged = (st, index) => { };

    public ItemsSet _inventory { get; private set; }
    //TODO maybe remove public access
    public ItemSlot HadSlot;
    public ItemSlot BodySlot;
    public ItemSlot LagsSlot;
    
    [SerializeField] private int MaxNumberInInventory;
    public Queue<IItemComponent> pickables;

    //TODO public Dictionary<ItemSlotType, BaseItem> stuff;
    private void Start()
    {
        pickables = new Queue<IItemComponent>();
        _inventory = new ItemsSet(10);
        var gun = new Gun(_gunScriptableObject, Guid.NewGuid()); 
        var gun1 = new Gun(_gunScriptableObject, Guid.NewGuid()); 
        var bulletStack = new BulletStack(_bulletScriptableObject,10, Guid.NewGuid());
        var bulletStack1 = new BulletStack(_bulletScriptableObject,10, Guid.NewGuid());
        DataBase.instance.AddItem(gun);
        DataBase.instance.AddItem(gun1);
        DataBase.instance.AddItem(bulletStack);
        DataBase.instance.AddItem(bulletStack1);
        
        _inventory.AddItem(gun.id);
        _inventory.AddItem(gun1.id);
        _inventory.AddItem(bulletStack.id);
        _inventory.AddItem(bulletStack1.id);
        Debug.Log(_inventory);
        
        HadSlot = new ItemSlot(false);
        BodySlot = new ItemSlot(false);
        LagsSlot = new ItemSlot(false);
        _inventory.OnItemSetChanged += () => OnEntityStuffChanged();
        _inventory.OnUpdateSlot +=(index) => OnEntityStuffSlotChanged(SlotType.Bag, index);
    }

    public void DropItem(Guid itemId)
    {
        _inventory.DropItemOnGround(itemId);
        var dropItem = DataBase.instance.GetItem(itemId);
        var dropGameObject = Instantiate(dropItem.ScriptableObject.prefab);
        dropGameObject.GetComponent<IItemComponent>().Initialize(dropItem);
        //TODO in specific place
        dropGameObject.transform.position = transform.position 
            + .5f * new Vector3(UnityEngine.Random.Range(-1f,1f), UnityEngine.Random.Range(-1f,1f));
    }
    
    public bool ShiftFromSlotToSlot(SlotHandlerUI fromSlotHandler, SlotHandlerUI toSlotHandler)
    {
        var fromSlot = GetSlot(fromSlotHandler);
        var toSlot = GetSlot(toSlotHandler);
        if ((fromSlot.id != Guid.Empty && toSlot.id != Guid.Empty) 
            && DataBase.instance.GetItem(toSlot.id) is ItemStack toItemStack
            && DataBase.instance.GetItem(fromSlot.id) is ItemStack fromItemStack) 
        {
            toItemStack.Accomodate(fromItemStack);
            _inventory.DropItem(fromItemStack.id);
            return true;
        }

        if (DataBase.instance.GetItem(fromSlot.id) is ItemStack && !toSlot.CanHandleStackableObject) 
            return false;
        
        SwapSlots(fromSlot, toSlot);
        return true;
    }
    
    public void SwapSlots(ItemSlot first, ItemSlot second)
    {
        var tempFromGuid = first.id;
        first.id = second.id;
        second.id = tempFromGuid;
    }

    public Guid GetSlotGuid(SlotHandlerUI slotHandler)
    {
        var inventorySlot = GetSlot(slotHandler); 
        if(inventorySlot == null)
            return Guid.Empty;
        return inventorySlot.id;
    }
    
    public ItemSlot GetSlot(SlotHandlerUI slotHandler)
    {
        switch (slotHandler.SlotType)
        {
            case SlotType.Bag:
                return _inventory.ItemsInSlots[slotHandler.SlotIndex];
            case SlotType.Hands:
                break;
            case SlotType.Had:
                return HadSlot;
                break;
            case SlotType.Body:
                return BodySlot;
                break;
            case SlotType.Lags:
                return LagsSlot;
                break;
            case SlotType.Hat:
                break;
        }
        return null;
    } 

    public void Pick()
    {
        if (pickables.Count != 0)
        {
            //TODO with "animation"
            //TODO pick with pop up button
            var itemComponent = pickables.Dequeue();
            var baseItem = itemComponent.Item;
            itemComponent.Serialize();
            if (baseItem != null)
            {
                _inventory.AddItem(baseItem.id);
            }
        }
    }

    public void SplitBagItem(int index, int fromNumber, int toNumber)
    {
        _inventory.Split(index, fromNumber, toNumber);
    }
    
}

public enum ItemSlotType
{
    Bag, Hand
}
