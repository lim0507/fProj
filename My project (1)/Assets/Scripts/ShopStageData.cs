using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum ShopStageType
{
    Weapon,
    Upgrade,
    Buff
}

[System.Serializable]
public class ShopStageData
{
    public int stage;

    public ShopStageType shopType;

    public List<WeaponShopItem> weaponShopTable;
    public List<ShopItem> itemShopTable;
    public List<ShopItem> sellTable;

    public GameObject shopVisualPrefab; // 상점 외형
}