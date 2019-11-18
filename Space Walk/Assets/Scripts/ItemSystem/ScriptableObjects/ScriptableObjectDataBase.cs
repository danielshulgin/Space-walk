using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace ItemSystem
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "ScriptableObjectDataBase", menuName = "ScriptableObjects/ScriptableObjectDataBase", order = 1)]
    public class ScriptableObjectDataBase : ScriptableObject
    {
        public List<BaseScriptableObject> scriptableObjects;

        #region SINGLETON PATTERN
        static ScriptableObjectDataBase _instance = null;
        public static ScriptableObjectDataBase Instance
        {
            get
            {
                if (!_instance)
                    _instance = Resources.FindObjectsOfTypeAll<ScriptableObjectDataBase>().FirstOrDefault();
                return _instance;
            }
        }
        #endregion

        public BaseScriptableObject GetById(Guid id)
        {
            return scriptableObjects.Find(scriptableObject => scriptableObject.Guid == id);
        }
    }
}