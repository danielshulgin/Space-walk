using System;

public class ItemSlot
{
    public Guid id;
    public bool CanHandleStackableObject { get; private set; }

    public ItemSlot(bool canHandleStackableObject = true)
    {
        CanHandleStackableObject = canHandleStackableObject;
    }
}