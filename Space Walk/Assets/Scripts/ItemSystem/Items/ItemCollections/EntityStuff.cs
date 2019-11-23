using System;
using System.Collections.Generic;
using ItemSystem;
using ItemSystem.Items.ItemCollections;
using ItemSystem.ScriptableObjects.ConcreteScriptableObjects;
using UI;
using UnityEngine;

namespace Entity.Stuff
{
    public class EntityStuff
    {
        public event Action<SlotType, int> OnUpdateSlot = (st, index) => { };

        public event Action<Guid, int> OnDropItem = (id, number) => { };

        private List<Slot> _slots;

        private ItemsSet _bag;

        //TODO maybe remove public access
        public Slot HadSlot { get; private set; }
        public Slot BodySlot { get; private set; }
        public Slot LagsSlot { get; private set; }

        public int BagSlotsNumber => _bag.MaxSlotNumber; 
        
        
        public EntityStuff(ItemsSet bag)
        {
            _bag = bag;
            _bag.OnUpdateSlot += (type, index)=> OnUpdateSlot(type, index);
            _slots = new List<Slot>();
            HadSlot = new Slot(SlotType.Had, 0);
            BodySlot = new Slot(SlotType.Body, 0);
            LagsSlot = new Slot(SlotType.Lags, 0);
            _slots.Add(HadSlot);
            _slots.Add(BodySlot);
            _slots.Add(LagsSlot);
        }

        public void DropItem(SlotType slotType, int slotIndex = 0)
        {
            if (slotType == SlotType.Bag)
            {
                var slot = _bag.GetSlot(slotIndex);
                OnDropItem(slot.id, slot.number);
                _bag.RemoveItem(slotIndex);
            }
        }
        
        
        public bool SwapSlots(SlotType firstSlotType, int firstSlotIndex, SlotType secondSlotType, int secondSlotIndex)
        {
            if (firstSlotType == SlotType.Bag && secondSlotType == SlotType.Bag)
            {
                _bag.SwapSlotsInSet(firstSlotIndex, secondSlotIndex);
                return true;
            }
            if (firstSlotType == SlotType.Bag)
            {
                _bag.SwapSlots(firstSlotIndex,_slots.Find(slot => slot.Type == secondSlotType));
                return true;
            }
            if (secondSlotType == SlotType.Bag)
            {
                _bag.SwapSlots(secondSlotIndex,_slots.Find(slot => slot.Type == firstSlotType));
                return true;
            }
            _slots.Find(slot => slot.Type == firstSlotType)
                .Swap(_slots.Find(slot => slot.Type == secondSlotType));
            return true;
        }
        
        public SlotUIData GetSlotDescription(SlotType slotType, int slotIndex = 0)
        {
            if(slotType == SlotType.Bag)
            {
                return _bag.GetSlotDescription(slotIndex);
            }
            return _slots.Find(slot => slot.Type == slotType && slot.Index == slotIndex).SlotUiData;
        }
        
        public bool AddItem(Guid itemId, int number)
        {
            _bag.AddItem(itemId, number);
            return true;
        }

        public void SplitBagItem(int index, int fromNumber, int toNumber)
        {
            _bag.Split(index, fromNumber, toNumber);
        }
    }
}