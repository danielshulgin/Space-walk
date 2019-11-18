using System;
using System.Collections.Generic;
using ItemSystem;
using ItemSystem.ScriptableObjects.ConcreteScriptableObjects;
using UnityEngine;

//TODO remove monobehaviour
public class EntityStuff : MonoBehaviour
{
    //TODO remove
    [SerializeField] private BulletScriptableObject _bulletScriptableObject;
    [SerializeField] private GunScriptableObject _gunScriptableObject;

    public Inventory _inventory { get; private set; }
    [SerializeField] private int MaxNumberInInventory;
    public Queue<IItemComponent> pickables;

    //TODO public Dictionary<ItemSlotType, BaseItem> stuff;
    private void Start()
    {
        pickables = new Queue<IItemComponent>();
        _inventory = new Inventory(10);
        _inventory.OnDropItemOnGround += DropItem;
        var gun = new Gun(_gunScriptableObject, Guid.NewGuid()); 
        var bulletStack = new BulletStack(_bulletScriptableObject,10, Guid.NewGuid());
        var bulletStack1 = new BulletStack(_bulletScriptableObject,10, Guid.NewGuid());
        DataBase.instance.AddItem(gun);
        DataBase.instance.AddItem(bulletStack);
        DataBase.instance.AddItem(bulletStack1);
        
        _inventory.AddItem(gun.id);
        _inventory.AddItem(bulletStack.id);
        _inventory.AddItem(bulletStack1.id);
        Debug.Log(_inventory);
    }

    public void DropItem(BaseItem dropItem)
    {
        var dropGameObject = Instantiate(dropItem.ScriptableObject.prefab);
        dropGameObject.GetComponent<IItemComponent>().Initialize(dropItem);
        //TODO in specific place
        dropGameObject.transform.position = transform.position 
            + .5f * new Vector3(UnityEngine.Random.Range(-1f,1f), UnityEngine.Random.Range(-1f,1f));
    }

    public void Pick()
    {
        //TODO suck in animation add by id 
        var itemComponent = pickables.Dequeue();
        var baseItem = itemComponent.Item;
        itemComponent.Serialize();
        if (baseItem != null)
        {
            _inventory.AddItem(baseItem.id);
        }
    }
    
}

public enum ItemSlotType
{
    Bag, Hand
}
