using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoney : MonoBehaviour
{
    public int money;
    public Text moneyText;

    void Start()
    {
        RefreshUI();
    }

    public void AddMoney(int amount)
    {
        money += amount;
        RefreshUI();
    }

    void RefreshUI()
    {
        if (moneyText)
            moneyText.text = $"$ {money}";
    }
    public void ResetMoney()
    {
        money = 0;
        RefreshUI();
    }
}
