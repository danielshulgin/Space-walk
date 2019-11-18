using System;
using System.Collections.Generic;
using System.Linq;
using Boo.Lang.Environments;
using UnityEditor;
using UnityEngine;

namespace ItemSystem
{
    public class Inventory
    {
        private readonly List<Guid> _items;
        public Guid[] ItemsInSlots;
        public event Action<BaseItem> OnDropItemOnGround = (i) => { };
        public int MaxItemNumber { get; private set; }
        public event Action OnInventoryChanged = () => { };
        public Inventory(int maxItemNumber)
        {
            MaxItemNumber = maxItemNumber;
            _items = new List<Guid>();
            ItemsInSlots = new Guid[maxItemNumber];
        }

        public bool AddItem(Guid id)
        {
            var item = DataBase.instance.GetItem(id);
            PutItemInEmptySlot(item);
            if (item.ScriptableObject is IStackable)
            {
                return AddStackableItem(item);
            }

            return AddSingleItem(item.id);
        }

        //TODO replace BaseItem with id
        private bool AddStackableItem(BaseItem item)
        {
            var fromStack = item as ItemStack;
            var toStack = DataBase.instance.GetItem(_items.Find(baseItemId => 
                DataBase.instance.GetItem(baseItemId).ScriptableObject == item.ScriptableObject)) as ItemStack;

            if (toStack != null && toStack.CanAccommodate(fromStack))
            {
                toStack.Accomodate(fromStack);
                OnInventoryChanged();
                return true;
            }
            return AddSingleItem(item.id);
        }

        private bool AddSingleItem(Guid id)
        {
            if (_items.Count < MaxItemNumber)
            {
                _items.Add(id);
                OnInventoryChanged();
                return true;
            }
            return false;
        }

        public BaseItem DropItem(Guid id)
        {
            var dropItem = DataBase.instance.GetItem(id);
            _items.Remove(id);
            ItemsInSlots[Array.IndexOf(ItemsInSlots, id)] = Guid.Empty;
            return dropItem;
        }
        
        public BaseItem DropItemOnGround(Guid id)
        {
            var dropItem = DropItem(id);
            OnDropItemOnGround(dropItem);
            return dropItem;
        }

        private bool PutItemInEmptySlot(BaseItem baseItem)
        {
            for (var i = 0; i < ItemsInSlots.Length; i++)
            {
                var slotId = ItemsInSlots[i];
                if (slotId == Guid.Empty)
                {
                    ItemsInSlots[i] = baseItem.id;
                    return true;
                }
            }

            //TODO checks
            return false;
        }

        public bool MoveToAnotherSlot(int from, int to)
        {
            if ((ItemsInSlots[from] != Guid.Empty && ItemsInSlots[to] != Guid.Empty) 
                && DataBase.instance.GetItem(ItemsInSlots[from]) is ItemStack fromItemStack 
                && DataBase.instance.GetItem(ItemsInSlots[to]) is ItemStack toItemStack)
            {
                //TODO add checks
                toItemStack.Accomodate(fromItemStack);
                //TODO in single method
                _items.Remove(fromItemStack.id);
                ItemsInSlots[from] = Guid.Empty;
                OnInventoryChanged();
            }
            else
            {
                var tempFromGuid = ItemsInSlots[from];
                ItemsInSlots[from] = ItemsInSlots[to];
                ItemsInSlots[to] = tempFromGuid;
            }
            return true;
        }
        
        public override string ToString()
        {
            return _items.Aggregate("Inventory(", (current, item) => current + $" {item.ToString()}") +")";
        }
    }
}
