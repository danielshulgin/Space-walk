using System;

namespace ItemSystem
{
    public class BulletStack : ItemStack, IStackable
    {
        //TODO add Scriptable Objcet for stack, remove scriptable object from constructor
        public BulletStack(ItemStackScriptableObject baseScriptableObject, int count, Guid guid) : base(baseScriptableObject, guid, count)
        {
            
        }
    }
}