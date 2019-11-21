using System;
using System.Collections.Generic;
using ItemSystem;
using ItemSystem.ScriptableObjects.ConcreteScriptableObjects;
using UI;
using UnityEngine;

namespace Entity.Stuff
{
    public class EntityStuffComponent : MonoBehaviour
    {
        public EntityStuff Stuff { get; private set; }

        private void Start()
        {
            Stuff = new EntityStuff();
        }
    }

    public enum ItemSlotType
    {
        Bag, Hand
    }
}