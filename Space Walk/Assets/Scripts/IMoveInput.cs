using UnityEngine;
using System;

public interface IMoveInput 
{
    Action<Vector2> OnMove { get; set; }
}

