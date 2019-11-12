using UnityEngine;
using System;

public interface IMoveInput 
{
    event Action<Vector2> OnMove;
}

