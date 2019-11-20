using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(InventorySlotsUI))]
[RequireComponent(typeof(CanvasGroup))]
public class InventoryUI : MonoBehaviour
{
    private InventorySlotsUI _inventorySlotsUi;
    private CanvasGroup _canvasGroup;
    private void Start()
    {
        _inventorySlotsUi = GetComponent<InventorySlotsUI>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetActive(bool active)
    {
        if (active)
        {
            if (!_inventorySlotsUi.Initialized)
            {
                _inventorySlotsUi.InitializeSlots();
            }
            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }
        else
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}
