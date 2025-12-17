using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellButton : MonoBehaviour
{
    public ItemType type;
    public ShopPanel shop;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Sell);
    }

    void Sell()
    {
        shop.Sell(type);
    }
}
