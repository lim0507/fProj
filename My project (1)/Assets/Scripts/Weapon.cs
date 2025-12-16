using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName;
    public int currentDamage = 1;
    public float attackCooldown = 0.3f;

    float lastAttackTime;
    Inventory inventory;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    public virtual void Attack()
    {
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        lastAttackTime = Time.time;

        Camera cam = Camera.main;
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            Block block = hit.collider.GetComponent<Block>();

            if (block != null)
            {
                block.Hit(currentDamage, inventory);
                Debug.Log($"Block hit ¡æ {block.type}");
            }
        }
    }
}
