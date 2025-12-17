using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    WeaponDamage,
    WeaponSpeed,
    DoubleDrop
}

[System.Serializable]
public class BuffData
{
    public BuffType type;
    public int price;
    public float value;
    public float duration;
}