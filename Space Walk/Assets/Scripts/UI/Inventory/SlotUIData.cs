using ItemSystem;
using UnityEngine;

namespace UI
{
    public class SlotUIData
    {
        public bool Stackable { get; }
        public int Number { get; }
        public Sprite Sprite { get; }
        public string Description { get; }

        public bool Empty;

        public SlotUIData(bool stackable, int number, Sprite sprite, string description)
        {
            this.Stackable = stackable;
            this.Number = number;
            this.Sprite = sprite;
            this.Description = description;
            Empty = false;
        }

        public SlotUIData()
        {
            Empty = true;
        }
    }
}