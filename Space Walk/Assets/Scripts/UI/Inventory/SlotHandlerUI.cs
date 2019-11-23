using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class SlotHandlerUI : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject slotPanel;
    
    public SlotType Type => type;
    [SerializeField] private SlotType type;
    
    public int Index => index;
    [SerializeField] private int index;
    
    public void SetNumber(int number)
    {
        text.text = number.ToString();
    }
    
    public void Initialize(SlotType slotType, int slotIndex = 0)
    {
        this.type = slotType;
        this.index = slotIndex;
    }
    
    public void UpdateSlot(SlotUIData slotData)
    {
        if (!slotData.Empty)
        {
            EnableSlotImage(slotData.Sprite);
            if (slotData.Stackable)
            {
                SetNumber(slotData.Number);
                SetActiveNumberText(true);
            }
            else
            {
                SetActiveNumberText(false);
            }
        }
        else
        {
            DisableSlotImage();
        }
    }
    
    public void SetActiveNumberText(bool active)
    {
        slotPanel.SetActive(active);
        
    }

    public void EnableSlotImage(Sprite itemSprite)
    {
        transform.GetChild(0).GetComponent<Image>().overrideSprite = itemSprite;
        GetComponentInChildren<CanvasGroup>().alpha = 1f;
    }
    
    public void DisableSlotImage()
    {
        GetComponentInChildren<CanvasGroup>().alpha = 0f;
    }
}
