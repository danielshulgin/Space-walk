using System;
using ItemSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class ItemDragHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IDropHandler
    {
        public event Action<SlotHandler> OnItemStartHandling = (sh) => { };
        public event Action<SlotHandler> OnDropItemInSlot = (sh) => { };
        
        [SerializeField] private SlotHandler slotHandler;
        [SerializeField] private RectTransform backgroundRectTransform;
        
        private GameObject _slotGameObject;
        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;

        private Canvas _canvas;
        private Camera _cam;

        private Vector2 _lastPointerPosition;
        private Vector2 _startAnchoredPosition;


        protected void Start()
        {
            _slotGameObject = transform.parent.gameObject;
            _canvas = GetComponentInParent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _cam = _canvas.worldCamera;
            _rectTransform = GetComponent<RectTransform>();
            _startAnchoredPosition = _rectTransform.anchoredPosition;
            
        }

        //interface implementations IPointerDownHandler
        public void OnPointerDown(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = false;
            _lastPointerPosition = eventData.position / _canvas.scaleFactor;
            OnItemStartHandling(slotHandler);
            transform.SetParent(_canvas.transform);
        }

        
        //interface implementations IDragHandler
        public void OnDrag(PointerEventData eventData)
        {
            var scaleFactor = _canvas.scaleFactor;
            var input = (eventData.position - _lastPointerPosition) / scaleFactor;
            _lastPointerPosition = eventData.position / scaleFactor;
            _rectTransform.anchoredPosition += input;
        }

        //interface implementations IPointerUpHandler
        public void OnPointerUp(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;
            transform.SetParent(_slotGameObject.transform);
            _rectTransform.anchoredPosition = _startAnchoredPosition;
        }

        public void OnDrop(PointerEventData eventData)
        {
            OnDropItemInSlot(slotHandler);
        }
    }
}