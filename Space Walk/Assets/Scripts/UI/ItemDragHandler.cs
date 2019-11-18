using System;
using ItemSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasGroup))]
    public class ItemDragHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IDropHandler
    {
        public static BaseItem selectedItem;
        public static int selectedSlotIndex = -1;
        private GameObject slotGameObject;
        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;
        public int slotIndex;
        public InventoryUI inventoryUi;
        
        [SerializeField]private RectTransform backgroundRectTransform;
        

        private Canvas _canvas;
        private Camera _cam;

        private Vector2 _lastPointerPosition;
        private Vector2 _startAnchoredPosition;

        protected void Start()
        {
            slotGameObject = transform.parent.gameObject;
            _canvas = GetComponentInParent<Canvas>();
            _cam = _canvas.worldCamera;
            _rectTransform = GetComponent<RectTransform>();
            _startAnchoredPosition = _rectTransform.anchoredPosition;
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        //interface implementations IPointerDownHandler
        public void OnPointerDown(PointerEventData eventData)
        {
            _lastPointerPosition = eventData.position / _canvas.scaleFactor;
            selectedSlotIndex = slotIndex;
            _canvasGroup.blocksRaycasts = false;
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
            transform.SetParent(slotGameObject.transform);
            _rectTransform.anchoredPosition = _startAnchoredPosition;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (selectedSlotIndex != -1)
            {
                inventoryUi.entityStuff._inventory.MoveToAnotherSlot(ItemDragHandler.selectedSlotIndex, slotIndex);
                inventoryUi.UpdateUI();
            }
        }
    }
}