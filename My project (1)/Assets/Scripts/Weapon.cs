using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName;
    public int hitsToBreak = 5;
    public float attackCoolDown = 0.3f;

    float lastAttackTime = 0f;

    public virtual void Attack()
    {
        if (Time.time - lastAttackTime < attackCoolDown)
            return;

        lastAttackTime = Time.time;
        
    }
}
