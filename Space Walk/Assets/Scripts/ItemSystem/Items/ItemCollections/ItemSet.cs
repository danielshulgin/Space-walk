using System;
using System.Collections.Generic;
using System.Linq;
using Boo.Lang.Environments;
using UnityEditor;
using UnityEngine;

namespace ItemSystem
{
    public class ItemsSet
    {
        private readonly List<Guid> _items;
        public ItemSlot[] ItemsInSlots;

        public int EmptyPlaces => ItemsInSlots.Count(itemsInSlot => itemsInSlot.id == Guid.Empty);
        
        public event Action<BaseItem> OnDropItemOnGround = (i) => { };
        
        /// <summary>
        /// sand int position in itemStack
        /// </summary>
        public event Action<int> OnUpdateSlot = (p) => { };
        
        public event Action OnItemSetChanged = () => { };
        
        public int MaxItemNumber { get; private set; }
        public ItemsSet(int maxItemNumber)
        {
            MaxItemNumber = maxItemNumber;
            _items = new List<Guid>();
            ItemsInSlots = new ItemSlot[maxItemNumber];
            for (int i = 0; i < maxItemNumber; i++)
            {
                ItemsInSlots[i] = new ItemSlot();
            }
        }
        
        /// <returns>
        /// return index of slot, if can't add return -1
        /// </returns>
        public int AddItem(Guid id)
        {
            var item = DataBase.instance.GetItem(id);
            //var index = PutItemInEmptySlot(item);
//            if (item is ItemStack itemStack)
//            {
//                if (AddStackableItem(itemStack))
//                {
//                    return -1;
//                }
//                //return -1;
//            }

            if (AddSingleItem(item.id))
            {
                return PutItemInEmptySlot(item);
            }
            
            return -1;
        }
        
        private bool AddStackableItem(ItemStack fromStack)
        {
            var itemWithSameTypeId = _items.Find(baseItemId => 
                DataBase.instance.GetItem(baseItemId).ScriptableObject == fromStack.ScriptableObject);
            if (itemWithSameTypeId != Guid.Empty)
            {
                var toStack = DataBase.instance.GetItem(itemWithSameTypeId) as ItemStack;
                if (toStack != null && toStack.CanAccommodate(fromStack))
                {
                    toStack.Accomodate(fromStack);
                    OnItemSetChanged();
                    return true;
                }
            }
            return false;
        }

        private bool AddSingleItem(Guid id)
        {
            if (_items.Count < MaxItemNumber)
            {
                _items.Add(id);
                OnItemSetChanged();
                return true;
            }
            return false;
        }

        public bool Split(int slotIndex, int fromNumber, int toNumber)
        {
            var slot = ItemsInSlots[slotIndex];
            if (slot != null && EmptyPlaces > 0)
            {
                var item = DataBase.instance.GetItem(slot.id);
                if (item is ItemStack itemStack 
                    && ((fromNumber + toNumber) == itemStack.Number)
                    && fromNumber!= 0 
                    && toNumber!= 0)
                {
                    itemStack.Number = fromNumber;
                    var newItem = new ItemStack(item.ScriptableObject, Guid.NewGuid(), toNumber);
                    DataBase.instance.AddItem(newItem);
                    var indexOfNewSlot = AddItem(newItem.id);
                    if (indexOfNewSlot == -1)
                        return false;
                    
                    OnUpdateSlot(slotIndex);
                    OnUpdateSlot(indexOfNewSlot);
                    return true;
                }
            }
            return false;
        }

        public BaseItem DropItem(Guid id)
        {
            var dropItem = DataBase.instance.GetItem(id);
            _items.Remove(id);
            ItemsInSlots.ToList().Find(itemsInSlot => itemsInSlot.id == id).id = Guid.Empty;
            return dropItem;
        }
        
        public BaseItem DropItemOnGround(Guid id)
        {
            var dropItem = DropItem(id);
            OnDropItemOnGround(dropItem);
            return dropItem;
        }

        private int PutItemInEmptySlot(BaseItem baseItem)
        {
            for (var i = 0; i < ItemsInSlots.Length; i++)
            {
                if (ItemsInSlots[i].id == Guid.Empty)
                {
                    ItemsInSlots[i].id = baseItem.id;
                    return i;
                }
            }
            return -1;
        }
        
        public override string ToString()
        {
            return _items.Aggregate("Inventory(", (current, item) => 
                $"{current} {DataBase.instance.GetItem(item).ToString()}") +")";
        }
    }
}