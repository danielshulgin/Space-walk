using UnityEngine;

namespace ItemSystem.ScriptableObjects.ConcreteScriptableObjects
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Pistol", menuName = "ScriptableObjects/PistolScriptableObject", order = 1)]
    public class GunScriptableObject : BaseScriptableObject
    {
        public int damage;
    }
}