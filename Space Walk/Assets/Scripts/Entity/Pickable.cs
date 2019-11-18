﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var entityStuff = other.GetComponent<EntityStuff>();
        if (entityStuff != null)
        {
            entityStuff.pickables.Enqueue(this.GetComponent<IItemComponent>());
        }
    }
}
