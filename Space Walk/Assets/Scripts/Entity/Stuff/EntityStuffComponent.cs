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
        public Queue<IItemComponent> pickables;
        //TODO remove
        public GunScriptableObject gunScriptableObject;
        public BaseScriptableObject _bulletScriptableObject;
        public EntityStuff Stuff { get; private set; }

        private void Awake()
        {
            pickables = new Queue<IItemComponent>();
            var bag = new ItemsSet(10, SlotType.Bag);
            var gun = new Gun(gunScriptableObject, Guid.NewGuid()); 
            var gun1 = new Gun(gunScriptableObject, Guid.NewGuid()); 
            var bulletStack = new Bullet(_bulletScriptableObject, Guid.NewGuid());
            var bulletStack1 = new Bullet(_bulletScriptableObject, Guid.NewGuid());

            bag.AddItem(gun.id);
            bag.AddItem(gun1.id);
            bag.AddItem(bulletStack.id, 10);
            bag.AddItem(bulletStack1.id, 10);
            Debug.Log(bag);

            Stuff = new EntityStuff(bag);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var itemComponent = other.GetComponent<ItemComponent>();
            if (itemComponent != null)
            {
                pickables.Enqueue(itemComponent);
            }
        }
    }
}