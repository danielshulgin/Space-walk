using System;
using Entity.Movement;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace Input
{
    public class PcUserInput : MonoBehaviour, IMoveInput
    {
        //interface implementations IMoveInput
        public event Action<Vector2> OnMove = (v2) => { };
        
        private void Update()
        {
            var horizontalDelta = UnityEngine.Input.GetAxis("Horizontal");  
            var verticalDelta = UnityEngine.Input.GetAxis("Vertical");
            const float tolerance = .001f;
            if (Math.Abs(horizontalDelta) > tolerance || Math.Abs(verticalDelta) > tolerance)
            {
                OnMove.Invoke(new Vector2(horizontalDelta, verticalDelta));
            }
        }
    }
}

