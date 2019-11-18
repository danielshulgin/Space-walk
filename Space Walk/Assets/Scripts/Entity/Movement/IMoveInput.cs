using System;
using UnityEngine;

namespace Entity.Movement
{
    public interface IMoveInput 
    {
        event Action<Vector2> OnMove;
    }
}

