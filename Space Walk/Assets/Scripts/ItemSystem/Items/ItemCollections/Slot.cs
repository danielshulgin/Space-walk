using System;

public class Slot
{
    public Guid id;
    public int number;
    public bool Empty => id == Guid.Empty;
}