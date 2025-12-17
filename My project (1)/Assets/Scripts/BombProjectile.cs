using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombProjectile : MonoBehaviour
{
    public float fuseTime = 2f;
    public float explosionRadius = 3f;
    public int damage = 999;

    public GameObject explosionEffect;

    void Start()
    {
        Invoke(nameof(Explode), fuseTime);
    }

    void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            explosionRadius
        );

        foreach (Collider hit in hits)
        {
            Block block = hit.GetComponent<Block>();
            if (block != null)
            {
                Inventory inven = FindObjectOfType<Inventory>();
                block.Hit(damage, inven);
            }
        }

        if (explosionEffect)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
