using System;
using UI;
using UnityEngine;

namespace ItemSystem
{
    public abstract class BaseItem
    {
        public Guid TypeId { get; }
        
        public Guid id { get; }

        public string Name => ScriptableObject.name;

        public int MaxNumberInStack => ScriptableObject.maxNumberInStack;

        public bool Stackable => _scriptableObject.stackable;

        public BaseScriptableObject ScriptableObject
        {
            get
            {
                if (_scriptableObject!= null)
                {
                    return _scriptableObject;
                }
                return ScriptableObjectDataBase.Instance.GetById(id);
            }
            private set => _scriptableObject = value;
        }

        protected BaseScriptableObject _scriptableObject;

        public BaseItem(BaseScriptableObject baseScriptableObject, Guid guid)
        {
            TypeId = baseScriptableObject.Guid;
            id = guid;
            ScriptableObject = baseScriptableObject;
            AddToDataBase();
        }

        public virtual void AddToDataBase()
        {
            DataBase.instance.AddItem(this);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}