using System;
using System.Collections;
using System.Collections.Generic;
using ItemSystem;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    [HideInInspector] public static DataBase instance { get; private set; }

    public Dictionary<Guid, BaseItem> items;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        items = new Dictionary<Guid, BaseItem>();
    }

    public BaseItem GetItem(Guid id)
    {
        if (items.ContainsKey(id))
        {
            return items[id];
        }
        return null;
    }

    public void AddItem(BaseItem item)
    {
        items[item.id] = item;
    }
    
}
