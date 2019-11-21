using System;
using UnityEngine;

namespace ItemSystem
{
    public abstract class BaseItem
    {
        public Guid TypeId { get; }
        
        public Guid id { get; }
        
        public string Name { get; }

        public int MaxNumberInStack { get; }

        public bool Stackable { get; }

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
            Name = baseScriptableObject.name;
            MaxNumberInStack = baseScriptableObject.maxNumberInStack;
            Stackable = baseScriptableObject.stackable;
            AddToDataBase();
        }

        protected virtual void AddToDataBase()
        {
            DataBase.instance.AddItem(this);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}