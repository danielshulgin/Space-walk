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
        public List<Slot> Slots => new List<Slot>(_slots);

        public int EmptyPlaces => _slots.Count(slot => slot.id != Guid.Empty);
        
        /// <summary>
        /// sand int position of updated slot
        /// </summary>
        public event Action<int> OnUpdateSlot = (p) => { };
        
        private readonly List<Slot> _slots;

        public int MaxItemNumber { get;}
        
        public ItemsSet(int maxItemNumber)
        {
            MaxItemNumber = maxItemNumber;
            _slots = new List<Slot>(); 
            
            for (var i = 0; i < maxItemNumber; i++)
            {
                _slots.Add(new Slot());
            }
        }
        
        public bool AddItem(Guid id, int number = 1)
        {
            if (PutItemInEmptySlot(id, number))
            {
                return true;
            }
            return PutItemInExistingSlot(id, number);
        }

        public bool Split(int slotIndex, int fromNumber, int toNumber)
        {
            var itemId = _slots[slotIndex].id;
            if (EmptyPlaces > 0)
            {
                var item = DataBase.instance.GetItem(itemId);
                if (item.Stackable 
                    && ((fromNumber + toNumber) == _slots[slotIndex].number)
                    && fromNumber!= 0 
                    && toNumber!= 0)
                {
                    _slots[slotIndex].number = fromNumber;
                    PutItemInEmptySlot(itemId);
                    AddItem(itemId);
                    OnUpdateSlot(slotIndex);
                    return true;
                }
            }
            return false;
        }

        public void RemoveItem(Guid id, int number = 1)
        {
            var dropItem = DataBase.instance.GetItem(id);
            _slots.Find(slot => slot.id == id).number = 0;
            _slots.Find(slot => slot.id ==id).id = Guid.Empty;
        }

        public void RemoveItem(int slotIndex)
        {
            _slots[slotIndex].number = 0;
            _slots[slotIndex].id = Guid.Empty;
        }

        private bool PutItemInEmptySlot(Guid id, int number = 1)
        {
            for (var i = 0; i < _slots.Count; i++)
            {
                if (_slots[i].id == Guid.Empty)
                {
                    _slots[i].id = id;
                    _slots[i].number = number;
                    OnUpdateSlot(i);
                    return true;
                }
            }
            return false;
        }
        
        private bool PutItemInExistingSlot(Guid id, int number)
        {
            var item = DataBase.instance.GetItem(id);
            if (!item.Stackable) 
                return false;
            var canAccomodateItemsNumber = 0;
            var maxNumber = DataBase.instance.GetItem(id).MaxNumberInStack;
            for (var i = 0; i < _slots.Count; i++)
            {
                if (_slots[i].id == id)
                {
                    canAccomodateItemsNumber += maxNumber - _slots[i].number;
                }
            }
            if (canAccomodateItemsNumber <= number)
            {
                for (var i = 0; i < _slots.Count; i++)
                {
                    if (_slots[i].id == id)
                    {
                        var emptyPlace = maxNumber - _slots[i].number;
                        _slots[i].number += emptyPlace;
                        OnUpdateSlot(i);
                    }
                }
                return true;
            }
            return false;
        }
        
        public override string ToString()
        {
            return _slots.Aggregate("Inventory[", (current, slot) => 
                $"{current} {DataBase.instance.GetItem(slot.id).ToString()} - {slot.number}") +"]";
        }
    }
}