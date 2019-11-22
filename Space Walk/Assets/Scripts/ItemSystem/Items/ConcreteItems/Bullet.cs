using System;

namespace ItemSystem
{
    public class Bullet : BaseItem
    {
        //TODO add Scriptable Objcet for stack, remove scriptable object from constructor
        protected static Guid globalId;

        public Guid id
        {
            get => globalId;
        }
        
        public Bullet(BaseScriptableObject baseScriptableObject, Guid id): base(baseScriptableObject, id)
        {
            if (globalId == Guid.Empty)
            {
                Bullet.globalId = id;
            }
        }

        public override void AddToDataBase()
        {
            if (DataBase.instance.GetItem(id) == null)
            {
                DataBase.instance.AddItem(this);
            }
        }
    }
}