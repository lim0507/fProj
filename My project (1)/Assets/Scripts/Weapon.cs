using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName;
    public float attackCooldown = 0.3f;
    public int currentDamage = 1;

    float lastAttackTime = 0f;

    public virtual void Attack()
    {
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        lastAttackTime = Time.time;

        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("MainCamera 태그 없음");
            return;
        }

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 4.5f))
        {
            Block block = hit.collider.GetComponentInParent<Block>();

            if (block != null)
            {
                block.Hit(currentDamage, null);
                Debug.Log("Hit block!");
            }
            else
            {
                Debug.Log("Ray hit but no Block");
            }
        }
    }
}
