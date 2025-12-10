using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtBlock : MonoBehaviour
{
    public int maxHitCount = 5;   // 몇 번 찍어야 부서지는지
    private int currentHitCount = 0;

    public void Hit()
    {
        currentHitCount++;

        if (currentHitCount >= maxHitCount)
        {
            Break();
        }
    }

    void Break()
    {
        // 아이템 드랍
        ItemDrop drop = GetComponent<ItemDrop>();
        if (drop != null)
        {
            drop.Drop(transform.position);
        }

        Destroy(gameObject);
    }
}
