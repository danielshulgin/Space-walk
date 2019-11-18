﻿using UnityEngine;

namespace ItemSystem
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Bullet", menuName = "ScriptableObjects/BulletScriptableObject", order = 1)]
    public class BulletScriptableObject : BaseScriptableObject
    {
        public int maxNumberInStack;
    }
}