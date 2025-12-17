using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName;
    public int baseDamage = 1;
    public float baseAttackCooldown = 0.3f;

    float lastAttackTime;

    Inventory inventory;
    BuffManager buffManager;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        buffManager = FindObjectOfType<BuffManager>();
    }

    public virtual void Attack()
    {
        float finalCooldown = baseAttackCooldown / buffManager.weaponSpeedMultiplier;

        if (Time.time - lastAttackTime < finalCooldown)
            return;

        lastAttackTime = Time.time;

        int finalDamage = Mathf.RoundToInt(
            baseDamage * buffManager.weaponDamageMultiplier
        );

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            Block block = hit.collider.GetComponent<Block>();
            if (block != null)
            {
                block.Hit(finalDamage, inventory);
                Debug.Log($"[Weapon] Damage {finalDamage}");
            }
        }
    }
}
