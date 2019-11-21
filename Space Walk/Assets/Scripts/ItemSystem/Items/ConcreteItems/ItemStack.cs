using System;

namespace ItemSystem
{
    public class ItemStack : BaseItem
    {
        public int Number { get; set; }
        public int MaxNumberInStack 
        {
            get
            {
                if (ScriptableObject is ItemStackScriptableObject scriptableObject)
                {
                    return scriptableObject.maxNumberInStack;
                }
                return 1;
            }
        }

        public ItemStack(BaseScriptableObject baseScriptableObject, Guid guid, int number) : base(baseScriptableObject, guid)  
        {
            Number = number;
        }

        public bool CanAccommodate(ItemStack otherStack)
        {
            return (otherStack.Number + Number) <= MaxNumberInStack;
        }

        public bool Accomodate(ItemStack otherStack)
        {
            if (CanAccommodate(otherStack))
            {
                Number += otherStack.Number;
                otherStack.Reset();
                return true;
            }

            return false;
        }

        public void Reset()
        {
            Number = 0;
        }

        public override string ToString()
        {
            return $"{base.ToString()}Stack Number: {Number}";
        }
    }
}