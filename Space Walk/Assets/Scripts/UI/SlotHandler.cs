using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class SlotHandler : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject slotPanel;
    
    public SlotType SlotType => slotType;
    [SerializeField] private SlotType slotType;
    public int SlotIndex => slotIndex;
    [SerializeField] private int slotIndex;
    public void SetNumber(int number)
    {
        text.text = number.ToString();
    }
    
    public void Initialize(SlotType slotType, int slotIndex = 0)
    {
        this.slotType = slotType;
        this.slotIndex = slotIndex;
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
