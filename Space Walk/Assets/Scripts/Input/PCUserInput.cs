using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class PCUserInput : MonoBehaviour, IMoveInput
{
    //interface implementations IMoveInput
    public event Action<Vector2> OnMove = (v2) => { };

    private void Update()
    {
        float horizontalDelta = Input.GetAxis("Horizontal");
        float verticalDelta = Input.GetAxis("Vertical");
        if (horizontalDelta != 0f || verticalDelta != 0f)
        {
            OnMove.Invoke(new Vector2(horizontalDelta, verticalDelta));
        }
    }
}

