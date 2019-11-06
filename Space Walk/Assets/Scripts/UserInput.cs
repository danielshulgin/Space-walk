using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UserInput : MoveInput
{
    public event Action<Vector2> OnMoveRaw = (vector2) => { };

    private void Update()
    {
        float horizontalDelta = Input.GetAxis("Horizontal");
        float verticalDelta = Input.GetAxis("Vertical");
        if (horizontalDelta != 0f || verticalDelta != 0f)
        {
            OnMove(new Vector2(horizontalDelta, verticalDelta));
        }

        float rawHorizontalDelta = Input.GetAxisRaw("Horizontal");
        float rawVerticalDelta = Input.GetAxisRaw("Vertical");
        if (rawHorizontalDelta != 0f || rawVerticalDelta != 0f)
        {
            OnMoveRaw(new Vector2(rawHorizontalDelta, rawVerticalDelta));
        }
    }
}
