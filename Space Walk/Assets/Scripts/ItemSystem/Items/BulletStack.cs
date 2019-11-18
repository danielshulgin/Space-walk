using System;

namespace ItemSystem
{
    //TODO remove
    public class BulletStack : ItemStack, IStackable
    {
        //TODO add Scriptable Objcet for stack, remove scriptable object from constructor
        public BulletStack(BulletScriptableObject baseScriptableObject, int count, Guid guid) : base(baseScriptableObject, guid, count)
        {
            
        }
    }
}