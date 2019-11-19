using System;
using System.Collections.Generic;
using System.Linq;
using Boo.Lang.Environments;
using UnityEditor;
using UnityEngine;

namespace ItemSystem
{
    public class ItemSet
    {
        private readonly List<Guid> _items;
        public ItemSlot[] ItemsInSlots;
        public event Action<BaseItem> OnDropItemOnGround = (i) => { };
        public int MaxItemNumber { get; private set; }
        public event Action OnItemSetChanged = () => { };
        public ItemSet(int maxItemNumber)
        {
            MaxItemNumber = maxItemNumber;
            _items = new List<Guid>();
            ItemsInSlots = new ItemSlot[maxItemNumber];
            for (int i = 0; i < maxItemNumber; i++)
            {
                ItemsInSlots[i] = new ItemSlot();
            }
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
                OnItemSetChanged();
                return true;
            }
            return AddSingleItem(item.id);
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

        private bool PutItemInEmptySlot(BaseItem baseItem)
        {
            for (var i = 0; i < ItemsInSlots.Length; i++)
            {
                if (ItemsInSlots[i].id == Guid.Empty)
                {
                    ItemsInSlots[i].id = baseItem.id;
                    return true;
                }
            }

            //TODO checks
            return false;
        }
        public override string ToString()
        {
            return _items.Aggregate("Inventory(", (current, item) => 
                $"{current} {DataBase.instance.GetItem(item).ToString()}") +")";
        }
    }
}