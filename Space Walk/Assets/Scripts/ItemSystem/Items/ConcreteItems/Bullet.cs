using System;

namespace ItemSystem
{
    public class Bullet : BaseItem
    {
        //TODO add Scriptable Objcet for stack, remove scriptable object from constructor
        protected static Guid id;
        public Bullet(ItemStackScriptableObject baseScriptableObject, int count, Guid id): base(baseScriptableObject, id)
        {
            if (id == Guid.Empty)
            {
                Bullet.id = id;
            }
        }

        protected override void AddToDataBase()
        {
            
            if (DataBase.instance.GetItem(id) != null)
            {
                DataBase.instance.AddItem(this);
            }
        }
    }
}