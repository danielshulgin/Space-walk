using UnityEngine;

namespace Entity.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EntityMovement : MonoBehaviour
    {
        public GameObject movementInputGo;
        private IMoveInput _moveInput;

        [SerializeField] private float linearSpeed = 10f;
        [SerializeField] private float maxLinearSpeed = 1f;
        
        [SerializeField] private float rotationSpeed = 220f;
        [SerializeField] private float maxRotationSpeed = 1f;

        private Rigidbody2D _rigidbody2D;

        public float Speed { 
            get => linearSpeed;
            set
            {
                if (value <= maxLinearSpeed)
                {
                    linearSpeed = value; 
                }
            } 
        }

        public float RotationSpeed
        {
            get => rotationSpeed;
            set
            {
                if (value <= maxRotationSpeed)
                {
                    rotationSpeed = value;
                }
            }
        }


        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            _moveInput = movementInputGo.GetComponent<IMoveInput>();
            _moveInput.OnMove += MovementTick;
            _moveInput.OnMove += RotationTick;
        }

        private void OnDisable()
        {
            _moveInput.OnMove -= MovementTick;
            _moveInput.OnMove -= RotationTick;
        }

        private void MovementTick(Vector2 direction)
        {
            _rigidbody2D.velocity = linearSpeed * direction;
        }

        private void RotationTick(Vector2 direction)
        {
            var degreeAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            var quaternionAngle = Quaternion.AngleAxis(degreeAngle, Vector3.forward);
        
            transform.rotation = Quaternion.RotateTowards(transform.rotation, quaternionAngle,
                rotationSpeed * Time.deltaTime);
        }
    }
}
