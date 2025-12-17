using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopItem
{
    public ItemType type;
    public int sellPrice;
    public int buyPrice;

    public int buyCount = 1;
}
