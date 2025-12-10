using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropItem
{
    public GameObject prefab;
    public float weight;
}

public class ItemDrop : MonoBehaviour
{
    [Header("Drop Tables")]
    public DropItem[] shallowDrops;
    public DropItem[] midDrops;
    public DropItem[] deepDrops;

    public void Drop(Vector3 pos)
    {
        int depth = Mathf.Abs(Mathf.FloorToInt(pos.y));

        DropItem[] table =
            depth < 30 ? shallowDrops :
            depth < 80 ? midDrops :
                         deepDrops;

        SpawnFromTable(table, pos);
    }

    void SpawnFromTable(DropItem[] table, Vector3 pos)
    {
        if (table.Length == 0) return;

        float totalWeight = 0f;
        foreach (var item in table)
            totalWeight += item.weight;

        float rand = Random.value * totalWeight;
        float current = 0f;

        foreach (var item in table)
        {
            current += item.weight;
            if (rand <= current)
            {
                Instantiate(item.prefab, pos, Quaternion.identity);
                return;
            }
        }
    }
}
