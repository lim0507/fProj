using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    public int damagePerHit = 1;
    public float range = 5f;

    public void Use(RaycastHit hit)
    {
        DirtBlock dirt = hit.collider.GetComponent<DirtBlock>();

        if (dirt != null)
        {
            for (int i = 0; i < damagePerHit; i++)
            {
                dirt.Hit();
            }
        }
    }
}
