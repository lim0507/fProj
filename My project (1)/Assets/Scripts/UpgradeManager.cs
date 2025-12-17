using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public int damageUpgradeLevel = 0;
    public int speedUpgradeLevel = 0;

    public int DamageBonus => damageUpgradeLevel;       // +데미지
    public float SpeedBonus => 1f + speedUpgradeLevel * 0.1f; // 공격속도 배율
    public UpgradeData damageUpgrade;
    public UpgradeData speedUpgrade;

    public int GetDamageUpgradePrice()
    {
        return damageUpgrade.basePrice
               + damageUpgradeLevel * damageUpgrade.priceIncrease;
    }

    public int GetSpeedUpgradePrice()
    {
        return speedUpgrade.basePrice
               + speedUpgradeLevel * speedUpgrade.priceIncrease;
    }
    public void UpgradeDamage()
    {
        damageUpgradeLevel++;
        Debug.Log($"[Upgrade] 데미지 업그레이드 Lv.{damageUpgradeLevel}");
    }

    public void UpgradeSpeed()
    {
        speedUpgradeLevel++;
        Debug.Log($"[Upgrade] 공격속도 업그레이드 Lv.{speedUpgradeLevel}");
    }

    public void ResetUpgrades()
    {
        damageUpgradeLevel = 0;
        speedUpgradeLevel = 0;
    }
}
