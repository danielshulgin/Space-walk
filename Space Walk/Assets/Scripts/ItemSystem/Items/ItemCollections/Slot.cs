using System;
using UI;

public class Slot
{
    public Guid id;
    public int number;
    public SlotType slotType { get; private set; }
    public int slotIndex { get; private set; }
    public bool Empty => id == Guid.Empty;
    
    public SlotUIData SlotUiData
    {
        get
        {
            var item = DataBase.instance.GetItem(id);
            if (item == null)
            {
                return  new SlotUIData();
            }
            return new SlotUIData(item.ScriptableObject.stackable, number, 
                item.ScriptableObject.inventorySprite, item.ScriptableObject.description);
        }
    }

    public Slot(SlotType slotType, int slotIndex)
    {
        this.slotType = slotType;
        this.slotIndex = slotIndex;
    }

    public void SetSlot(Guid id, int number = 1)
    {
        this.id = id;
        this.number = number;
    }

    public void Reset()
    {
        number = 0;
        id = Guid.Empty;
    }

    public void Swap(Slot anotherSLot)
    {
        var anotherSlotNumber = anotherSLot.number;
        var anotherSlotId = anotherSLot.id;
        anotherSLot.number = number;
        anotherSLot.id = id;
        this.number = anotherSlotNumber;
        this.id = anotherSlotId;
    }
}