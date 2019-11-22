using System;
using System.Collections.Generic;
using ItemSystem;
using ItemSystem.ScriptableObjects.ConcreteScriptableObjects;
using UI;
using UnityEngine;

namespace Entity.Stuff
{
    public class EntityStuff
    {
                //TODO remove
        [SerializeField] private ItemStackScriptableObject _bulletScriptableObject;
        [SerializeField] private GunScriptableObject _gunScriptableObject;
        
        public event Action<SlotType, int> OnEntityStuffSlotChanged = (st, index) => { };

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
            _slots = new List<Slot>();
        }

        public void DropItem(SlotType slotType, int slotIndex = 0)
        {
            throw new NotImplementedException();
        }
        
        
        public bool SwapSlots(SlotType firstSlotType, int firstSlotIndex, SlotType secondSlotType, int secondSlotIndex)
        {
            if (firstSlotType == SlotType.Bag)
            {
                _bag.SwapSlots(firstSlotIndex,_slots.Find(slot => slot.slotType == secondSlotType));
                return true;
            }
            if (secondSlotType == SlotType.Bag)
            {
                _bag.SwapSlots(secondSlotIndex,_slots.Find(slot => slot.slotType == firstSlotType));
                return true;
            }
            _slots.Find(slot => slot.slotType == firstSlotType)
                .SwapWithAnotherSlot(_slots.Find(slot => slot.slotType == secondSlotType));
            return true;
        }
        
        public SlotUIData GetSlotDescription(SlotType slotType, int slotIndex = 0)
        {
            if(slotType == SlotType.Bag)
            {
                return _bag.GetSlotDescription(slotIndex);
            }
            return _slots[slotIndex].SlotUiData;
        }
        
        public bool AddItem(Guid itemId)
        {
            throw new NotImplementedException();
            return true;
        }

        public void SplitBagItem(int index, int fromNumber, int toNumber)
        {
            _bag.Split(index, fromNumber, toNumber);
        }
    }
}