using System;
using System.Collections;
using System.Collections.Generic;
using Entity.Stuff;
using UnityEditorInternal;
using UnityEngine;

public class PickableComponent : MonoBehaviour
{
    //TODO move to player
    private void OnTriggerEnter2D(Collider2D other)
    {
        var entityStuff = other.GetComponent<EntityStuffComponent>();
        if (entityStuff != null)
        {
            entityStuff.pickables.Enqueue(this.GetComponent<IItemComponent>());
        }
    }
}
