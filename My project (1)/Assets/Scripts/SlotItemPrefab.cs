using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotItemPrefab : MonoBehaviour, IPointerClickHandler
{
    public Image itemImage;
    public Text itemText;

    public ItemType blockType;
    public CraftingPanel craftingPanel;

    // InventoryUI에서 호출하는 함수 (이게 빠져서 에러 났던 것)
    public void ItemSetting(Sprite itemSprite, string txt, ItemType type)
    {
        itemImage.sprite = itemSprite;
        itemText.text = txt;
        blockType = type;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("SlotItemPrefab.OnPointerClick 호출됨");
        Debug.Log("클릭된 오브젝트: " + gameObject.name);

        if (eventData.button != PointerEventData.InputButton.Right)
            return;

        if (!craftingPanel)
        {
            Debug.LogError("craftingPanel NULL");
            return;
        }

        Debug.Log("AddPlanned 호출");
        craftingPanel.AddPlanned(blockType, 1);
    }

}
