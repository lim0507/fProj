using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public float weaponSpeedMultiplier = 1f;
    public float weaponDamageMultiplier = 1f;
    public bool doubleDrop = false;

    // 남은 지속시간을 추적
    Dictionary<BuffType, Coroutine> activeBuffs = new();
    Dictionary<BuffType, float> remainingTime = new();

    public void ApplyBuff(BuffType type, float value, float duration)
    {
        if (activeBuffs.ContainsKey(type))
            StopCoroutine(activeBuffs[type]);

        remainingTime[type] = duration;
        activeBuffs[type] = StartCoroutine(BuffRoutine(type, value, duration));
    }

    IEnumerator BuffRoutine(BuffType type, float value, float duration)
    {
        EnableBuff(type, value);

        float timeLeft = duration;
        while (timeLeft > 0)
        {
            remainingTime[type] = timeLeft;
            yield return null;
            timeLeft -= Time.deltaTime;
        }

        DisableBuff(type, value);
        remainingTime.Remove(type);
        activeBuffs.Remove(type);
    }

    void EnableBuff(BuffType type, float value)
    {
        switch (type)
        {
            case BuffType.WeaponSpeed: weaponSpeedMultiplier *= value; break;
            case BuffType.WeaponDamage: weaponDamageMultiplier *= value; break;
            case BuffType.DoubleDrop: doubleDrop = true; break;
        }
    }

    void DisableBuff(BuffType type, float value)
    {
        switch (type)
        {
            case BuffType.WeaponSpeed: weaponSpeedMultiplier /= value; break;
            case BuffType.WeaponDamage: weaponDamageMultiplier /= value; break;
            case BuffType.DoubleDrop: doubleDrop = false; break;
        }
    }

    // 남은 시간을 반환 (없으면 0)
    public float GetRemainingTime(BuffType type)
    {
        if (remainingTime.TryGetValue(type, out float t))
            return t;
        return 0f;
    }

    public void ResetAll()
    {
        StopAllCoroutines();
        activeBuffs.Clear();
        remainingTime.Clear();
        weaponSpeedMultiplier = 1f;
        weaponDamageMultiplier = 1f;
        doubleDrop = false;
    }
}
