using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Sprite dirtSprite;
    public Sprite stoneSprite;
    public Sprite shovelSprite;
    public Sprite screwSprite;
    public Sprite canSprite;
    public Sprite plasticSprite;

    public CraftingPanel craftingPanel;
    public SlotItemPrefab slotPrefab;
    public Transform slotRoot;

    public List<Transform> Slot = new List<Transform>();
    public GameObject SlotItem;
    List<GameObject> items = new List<GameObject>();

    public int selectedIndex = -1;
    public void UpdateInventory(Inventory myInven)
    {
        foreach (var slotItems in items)
        {
            Destroy(slotItems);
        }
        items.Clear();

        int idx = 0;
        foreach (var item in myInven.items)
        {
            var go = Instantiate(SlotItem, Slot[idx].transform);
            go.transform.localPosition = Vector3.zero;

            SlotItemPrefab sItem = go.GetComponent<SlotItemPrefab>();
            sItem.craftingPanel = craftingPanel;
            items.Add(go);

            switch (item.Key)
            {
                case ItemType.Dirt:
                    sItem.ItemSetting(dirtSprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case ItemType.Stone:
                    sItem.ItemSetting(dirtSprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case ItemType.Screw:
                    sItem.ItemSetting(screwSprite, "x" + item.Value, item.Key);
                    break;

                case ItemType.Can:
                    sItem.ItemSetting(canSprite, "x" + item.Value, item.Key);
                    break;

                case ItemType.Plastic:
                    sItem.ItemSetting(plasticSprite, "x" + item.Value, item.Key);
                    break;
            }
            idx++;
        }
    }
    private void Update()
    {
        for (int i = 0; i < Mathf.Min(9, Slot.Count); i++)
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SetselectedIndex(i);
            }
    }
    public void SetselectedIndex(int idx)
    {
        Resetselection();
        if (selectedIndex == idx)
        {
            selectedIndex = -1;
        }
        else
        {
            Setselection(idx);
            selectedIndex = idx;
        }
    }
    public void Resetselection()
    {
        foreach (var slot in Slot)
        {
            slot.GetComponent<Image>().color = Color.white;
        }
    }
    void Setselection(int _idx)
    {
        Slot[_idx].GetComponent<Image>().color = Color.yellow;
    }
    public ItemType GetInventoryslot()
    {
        return items[selectedIndex].GetComponent<SlotItemPrefab>().blockType;
    }
}
