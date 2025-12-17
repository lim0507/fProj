using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffDisplay : MonoBehaviour
{
    public BuffManager buffManager;
    public Text buffText;

    void Update()
    {
        string text = "";

        float speedTime = buffManager.GetRemainingTime(BuffType.WeaponSpeed);
        if (speedTime > 0f) text += $"속도 버프: {speedTime:F1}s\n";

        float damageTime = buffManager.GetRemainingTime(BuffType.WeaponDamage);
        if (damageTime > 0f) text += $"데미지 버프: {damageTime:F1}s\n";

        float doubleDropTime = buffManager.GetRemainingTime(BuffType.DoubleDrop);
        if (doubleDropTime > 0f) text += $"더블 드랍: {doubleDropTime:F1}s\n";

        if (string.IsNullOrEmpty(text))
            text = "활성화된 버프 없음";

        buffText.text = text;
    }
}
