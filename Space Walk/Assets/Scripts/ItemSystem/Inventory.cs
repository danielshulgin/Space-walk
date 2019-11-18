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
        private readonly List<BaseItem> _items;
        public Guid[] ItemsInSlots;
        public event Action<BaseItem> OnDropItemOnGround = (i) => { };
        public int MaxItemNumber { get; private set; }
        public event Action OnInventoryChanged = () => { };
        public Inventory(int maxItemNumber)
        {
            MaxItemNumber = maxItemNumber;
            _items = new List<BaseItem>();
            ItemsInSlots = new Guid[maxItemNumber];
        }

        public bool AddItem(BaseItem item)
        {
            PutItemInEmptySlot(item);
            if (item.ScriptableObject is IStackable)
            {
                return AddStackableItem(item);
            }

            return AddSingleItem(item);
        }

        private bool AddStackableItem(BaseItem item)
        {
            var fromStack = item as ItemStack;
            var toStack = _items.Find(baseItem => 
                baseItem.ScriptableObject == item.ScriptableObject) as ItemStack;

            if (toStack != null && toStack.CanAccommodate(fromStack))
            {
                toStack.Accomodate(fromStack);
                OnInventoryChanged();
                return true;
            }
            return AddSingleItem(item);
        }

        private bool AddSingleItem(BaseItem item)
        {
            if (_items.Count < MaxItemNumber)
            {
                _items.Add(item);
                OnInventoryChanged();
                return true;
            }
            return false;
        }

        public BaseItem DropItem(Guid id)
        {
            var dropItem = GetItem(id);
            _items.Remove(dropItem);
            ItemsInSlots[Array.IndexOf(ItemsInSlots, id)] = Guid.Empty;
            return dropItem;
        }
        
        public BaseItem DropItemOnGround(Guid id)
        {
            var dropItem = DropItem(id);
            OnDropItemOnGround(dropItem);
            return dropItem;
        }
        
        public BaseItem GetItem(Guid id)
        {
            var dropItem = _items.Find(item => item.id == id);
            return dropItem;
        }
        
        public T GetItem<T>(Guid id) where T:BaseItem
        {
            var dropItem = _items.Find(item => item.id == id);
            return dropItem as T;
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
            if (GetItem(ItemsInSlots[from]) is ItemStack fromItemStack 
                && GetItem(ItemsInSlots[to]) is ItemStack toItemStack)
            {
                //TODO add checks
                toItemStack.Accomodate(fromItemStack);
                //TODO in single method
                _items.Remove(fromItemStack);
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
