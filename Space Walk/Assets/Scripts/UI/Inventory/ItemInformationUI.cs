﻿using System;
using Entity.Stuff;
using ItemSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class ItemInformationUI : MonoBehaviour//, IDropHandler
    {
        [SerializeField] private EntityStuffComponent entityStuff;
        [SerializeField] private InventorySlotsUI inventorySlotsUi;
        [SerializeField] private Button splitButton;
        [SerializeField] private Slider slider;
        [SerializeField] private Text descriptionText;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            inventorySlotsUi.OnDescriptionCalled += Activate;
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Activate(SlotUIData slotUiData)
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
            descriptionText.text = slotUiData.Description;
            if (slotUiData.Stackable)
            {
                splitButton.gameObject.SetActive(true);
                slider.gameObject.SetActive(true);
            }
            else
            {
                splitButton.gameObject.SetActive(false);
                slider.gameObject.SetActive(false);
            }
        }
        
        public void Split()
        {
            /*var numberInStack = (DataBase.instance.GetItem(entityStuff.Stuff.GetSlotGuid(inventorySlotsUi.SelectedSlotHandler)) as ItemStack).Number;
            var toNumber = (int) (slider.value * numberInStack);
            var fromNumber = numberInStack - toNumber;
            entityStuff.Stuff.SplitBagItem(inventorySlotsUi.SelectedSlotHandler.SlotIndex,fromNumber,toNumber);
            DeActivate();*/
        }
        
        public void DeActivate()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}