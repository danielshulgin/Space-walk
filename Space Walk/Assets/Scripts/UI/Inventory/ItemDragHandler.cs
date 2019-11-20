using System;
using System.Collections;
using System.Linq;
using ItemSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class ItemDragHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IDropHandler
    {
        public event Action<SlotHandlerUI> OnItemStartHandling = (sh) => { };
        public event Action<SlotHandlerUI> OnDropItemInSlot = (sh) => { };

        public event Action OnDescriptionCalled = () => { }; 
        
        [SerializeField] private SlotHandlerUI slotHandler;
        [SerializeField] private RectTransform backgroundRectTransform;
        
        private GameObject _slotGameObject;
        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;

        private Canvas _canvas;
        private Camera _cam;

        private Vector2 _lastPointerPosition;
        private Vector2 _startAnchoredPosition;

        private bool _handling = false;
        private bool _drag = false;
        private bool _isTapCutDownRunning = false;


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
            _handling = true;
            _canvasGroup.blocksRaycasts = false;
            _lastPointerPosition = eventData.position / _canvas.scaleFactor;
            OnItemStartHandling(slotHandler);
            transform.SetParent(_canvas.transform);
            if (!_isTapCutDownRunning)
            {
                StartCoroutine(CheckForStartHandling());
            }
        }

        
        //interface implementations IDragHandler
        public void OnDrag(PointerEventData eventData)
        {
            if (_drag)
            {
                var scaleFactor = _canvas.scaleFactor;
                var input = (eventData.position - _lastPointerPosition) / scaleFactor;
                _lastPointerPosition = eventData.position / scaleFactor;
                _rectTransform.anchoredPosition += input;
            }
        }

        //interface implementations IPointerUpHandler
        public void OnPointerUp(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;
            transform.SetParent(_slotGameObject.transform);
            _rectTransform.anchoredPosition = _startAnchoredPosition;
            _handling = false;
            _drag = false;
        }

        public void OnDrop(PointerEventData eventData)
        {
            OnDropItemInSlot(slotHandler);
        }

        IEnumerator CheckForStartHandling()
        {
            _isTapCutDownRunning = true;
            yield return new WaitForSeconds(.1f);
            if (_handling)
            {
                _drag = true;
            }
            else
            {
                OnDescriptionCalled();
            }
            _isTapCutDownRunning = false;
        }
    }
}