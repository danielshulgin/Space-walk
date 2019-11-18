using System;
using Entity.Movement;
using UnityEngine;
using UnityEngine.EventSystems;


namespace UI
{
    public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IMoveInput
    {
        //interface implementations IMoveInput
        public event Action<Vector2> OnMove = (v2) => { };

        private float HandleRange
        {
            get => handleRange;
            set => handleRange = Mathf.Abs(value);
        }

        private float DeadZone
        {
            get => deadZone;
            set => deadZone = Mathf.Abs(value);
        }

        [SerializeField] private float handleRange = 1;
        [SerializeField] private float deadZone = 0;

        [SerializeField] private RectTransform background;
        [SerializeField] private RectTransform handle;

        private Canvas _canvas;
        private Camera _cam;

        private Vector2 _input = Vector2.zero;
        private bool _stay;

        protected void Start()
        {
            HandleRange = handleRange;
            DeadZone = deadZone;
            _canvas = GetComponentInParent<Canvas>();
            if (_canvas == null)
                Debug.LogError("The Joystick is not placed inside a canvas");

            var center = new Vector2(0.5f, 0.5f);
            background.pivot = center;
            handle.anchorMin = center;
            handle.anchorMax = center;
            handle.pivot = center;
            handle.anchoredPosition = Vector2.zero;
            _cam = _canvas.worldCamera;
        }

        private void Update()
        {
            if (_stay)
            {
                OnMove(_input);
            }
        }

        //interface implementations IPointerDownHandler
        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
            _stay = true;
        }

        //interface implementations IDragHandler
        public void OnDrag(PointerEventData eventData)
        {
            Vector2 position = RectTransformUtility.WorldToScreenPoint(_cam, background.position);
            Vector2 radius = background.sizeDelta / 2;
            _input = (eventData.position - position) / (radius * _canvas.scaleFactor);
            HandleInput(_input.magnitude, _input.normalized);
            handle.anchoredPosition = handleRange * _input * radius;
        }

        private void HandleInput(float magnitude, Vector2 normalised)
        {
            if (magnitude > deadZone)
            {
                if (magnitude > 1)
                    _input = normalised;
                OnMove(normalised);
            }
            else
            {
                _input = Vector2.zero;
            }
        }

        //interface implementations IPointerUpHandler
        public void OnPointerUp(PointerEventData eventData)
        {
            _input = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;
            _stay = false;
        }
    }
}