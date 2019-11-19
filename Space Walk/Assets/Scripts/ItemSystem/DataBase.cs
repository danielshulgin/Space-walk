using System;
using System.Collections;
using System.Collections.Generic;
using ItemSystem;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    [HideInInspector]
    public static DataBase instance;

    public Dictionary<Guid, BaseItem> items;
    
    private void Awake()
    {
        instance = this;
        items = new Dictionary<Guid, BaseItem>();
    }

    public BaseItem GetItem(Guid id)
    {
        return items[id];
    }

    public void AddItem(BaseItem item)
    {
        items[item.id] = item;
    }
    
}
