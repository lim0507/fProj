using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombWeapon : Weapon
{
    public GameObject bombPrefab;
    public Transform throwPoint;
    public float throwForce = 12f;
    public float attackCooldown = 0.5f;

    public ItemType bombItemType = ItemType.Bomb; // ¿Œ∫• ∆¯≈∫ æ∆¿Ã≈€

    float lastAttackTime;
    Inventory inventory;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    public override void Attack()
    {
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        if (inventory == null)
        {
            Debug.LogError("Inventory æ¯¿Ω");
            return;
        }

        
        if (inventory.GetCount(bombItemType) <= 0)
        {
            Debug.Log("∆¯≈∫¿Ã æ¯Ω¿¥œ¥Ÿ");
            return;
        }

        lastAttackTime = Time.time;

        
        inventory.Consume(bombItemType, 1);

        
        GameObject bomb = Instantiate(
            bombPrefab,
            throwPoint.position,
            Quaternion.identity
        );

        Rigidbody rb = bomb.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.AddForce(throwPoint.forward * throwForce, ForceMode.Impulse);
        }
    }
}
