using System;
using System.Collections.Generic;
using System.Linq;
using Boo.Lang.Environments;
using UI;
using UnityEditor;
using UnityEngine;

namespace ItemSystem
{
    public class ItemsSet
    {
        public List<Slot> Slots => new List<Slot>(_slots);

        public int EmptyPlaces => _slots.Count(slot => slot.id != Guid.Empty);
        
        public SlotType Type { get;}

        /// <summary>
        /// sand int index of updated slot
        /// </summary>
        public event Action<int> OnUpdateSlot = (p) => { };
        
        private readonly List<Slot> _slots;

        public int MaxSlotNumber { get;}
        
        public ItemsSet(int maxSlotNumber, SlotType setType)
        {
            MaxSlotNumber = maxSlotNumber;
            _slots = new List<Slot>();
            Type = setType;
            
            for (var i = 0; i < maxSlotNumber; i++)
            {
                _slots.Add(new Slot(setType, i));
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
            _slots.Find(slot => slot.id == id).Reset();
        }

        public void RemoveItem(int slotIndex)
        {
            _slots[slotIndex].Reset();
        }

        public void SwapSlots(int firstIndex, int secondIndex)
        {
            _slots[firstIndex].Swap(_slots[secondIndex]);
        }
        
        public void SwapSlots(int index, Slot slot)
        {
            _slots[index].Swap(slot);
        }

        private bool PutItemInEmptySlot(Guid id, int number = 1)
        {
            for (var i = 0; i < _slots.Count; i++)
            {
                if (_slots[i].id == Guid.Empty)
                {
                    return PutItemInEmptySlotWithIndex(i, id, number);
                }
            }
            return false;
        }
        
        private bool PutItemInEmptySlotWithIndex(int slotIndex, Guid id, int number = 1)
        {
            if (slotIndex < MaxSlotNumber && _slots[slotIndex].id == Guid.Empty)
            {
                _slots[slotIndex].SetSlot(id, number);
                OnUpdateSlot(slotIndex);
                return true;
            }
            return false;
        }
        
        private bool PutItemInExistingSlot(Guid id, int number)
        {
            var item = DataBase.instance.GetItem(id);
            if (!item.Stackable) 
                return false;
            var canAccomodateItemsNumber = 0;
            var maxNumberInStack = DataBase.instance.GetItem(id).MaxNumberInStack;
            for (var i = 0; i < _slots.Count; i++)
            {
                if (_slots[i].id == id)
                {
                    canAccomodateItemsNumber += maxNumberInStack - _slots[i].number;
                }
            }
            if (canAccomodateItemsNumber <= number)
            {
                for (var i = 0; i < _slots.Count; i++)
                {
                    if (_slots[i].id == id)
                    {
                        var emptyPlace = maxNumberInStack - _slots[i].number;
                        _slots[i].number += emptyPlace;
                        OnUpdateSlot(i);
                    }
                }
                return true;
            }
            return false;
        }

        public SlotUIData GetSlotDescription(int index)
        {
            return _slots[index].SlotUiData;
        }
        
        public override string ToString()
        {
            return _slots.Aggregate("Inventory[", (current, slot) =>slot.id != Guid.Empty? 
                $"{current} {DataBase.instance.GetItem(slot.id).ToString()} - {slot.number}":current) +"]";
        }
    }
}