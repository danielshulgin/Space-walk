using System;
using System.Collections.Generic;
using System.Linq;
using Boo.Lang.Environments;
using ItemSystem.Items.ItemCollections;
using UI;
using UnityEditor;
using UnityEngine;

namespace ItemSystem
{
    public class ItemsSet
    {
        /// <summary>
        /// sand slotType and int index of updated slot
        /// </summary>
        public event Action<SlotType, int> OnUpdateSlot = (st, index) => { };

        public int EmptyPlaces => _slots.Count(slot => slot.id != Guid.Empty);
        
        public SlotType Type { get;}
        
        public int MaxSlotNumber { get;}
        
        private readonly List<Slot> _slots;

        public IReadOnlyList<Slot> Slots => _slots;

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
                    OnUpdateSlot(Type, slotIndex);
                    return true;
                }
            }
            return false;
        }

        public Slot GetSlot(int slotIndex)
        {
            var slot = new Slot(_slots[slotIndex].Type, slotIndex);
            slot.SetSlot(_slots[slotIndex].id, _slots[slotIndex].number);
            return slot;
        }
        
        public void RemoveItem(Guid id, int number = 1)
        {
            _slots.Find(slot => slot.id == id).Reset();
        }

        public Guid RemoveItem(int slotIndex)
        {
            var id = _slots[slotIndex].id;
            _slots[slotIndex].Reset();
            return id; 
        }

        public void SwapSlotsInSet(int firstIndex, int secondIndex)
        {
            _slots[firstIndex].Swap(_slots[secondIndex]);
            OnUpdateSlot(Type, firstIndex);
            OnUpdateSlot(Type, secondIndex);
        }
        
        public void SwapSlots(int index, Slot slot)
        {
            _slots[index].Swap(slot);
            OnUpdateSlot(Type, index);
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
                OnUpdateSlot(Type, slotIndex);
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
                        OnUpdateSlot(Type, i);
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