using System;
using ItemSystem.ScriptableObjects.ConcreteScriptableObjects;

namespace ItemSystem
{
    public class Gun : BaseItem
    {
        public int damage;
        public Gun(GunScriptableObject gunScriptableObject, Guid guid) : base(gunScriptableObject, guid)
        {
            damage = gunScriptableObject.damage;
        }

        public override string ToString()
        {
            return $"{Name} damage: {damage}";
        }
    }
}