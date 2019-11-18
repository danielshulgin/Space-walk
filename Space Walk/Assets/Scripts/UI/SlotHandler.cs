using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotHandler : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject slotPanel;
    public void SetNumber(int number)
    {
        text.text = number.ToString();
    }
    public void SetActiveNumberText(bool active)
    {
        slotPanel.SetActive(active);
        Debug.Log("OK1");
    }
}
