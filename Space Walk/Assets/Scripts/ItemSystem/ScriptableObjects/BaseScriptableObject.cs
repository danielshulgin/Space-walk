using System;
using UnityEngine;

namespace ItemSystem
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Pistol", menuName = "ScriptableObjects/ScriptableObject", order = 1)]
    public class BaseScriptableObject : ScriptableObject
    {
        public string guidAsString;
        
        public string description;
        
        public GameObject prefab;

        public Sprite inventorySprite;
        
        public Sprite sprite;
        
        private Guid _guid;
        public Guid Guid
        {
            get
            {
                if ( _guid == System.Guid.Empty &&
                         !string.IsNullOrEmpty( guidAsString ) )
                {
                    _guid = new System.Guid( guidAsString );
                }
                return _guid;
            }
        }
 
        public void GenerateGuid()
        {
            _guid = System.Guid.NewGuid();
            guidAsString = Guid.ToString();
        }
    }
}