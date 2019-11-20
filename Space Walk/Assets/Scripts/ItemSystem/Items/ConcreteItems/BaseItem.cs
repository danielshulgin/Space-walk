using System;
using UnityEngine;

namespace ItemSystem
{
    public class BaseItem
    {
        public Guid TypeId { get; }
        public Guid id { get; }
        
        public string Name { get; }

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
        }

        public override string ToString()
        {
            return Name;
        }
    }
}