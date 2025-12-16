using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Dirt, Stone, Screw, Can, Plastic}
public class Block : MonoBehaviour
{
    [Header("Block Stat")]
    public ItemType type = ItemType.Dirt;
    public int maxHP = 3;
    [HideInInspector] public int hp;

    public int dropCount = 1;
    public bool mineable = true;

    [Header("Extra Drop")]
    [Range(0f, 1f)]
    public float extraDropChance = 0.3f;

    // Start is called before the first frame update
    void Awake()
    {
        hp = maxHP;
        if (GetComponent<Collider>() == null) gameObject.AddComponent<BoxCollider>();
        if (string.IsNullOrEmpty(gameObject.tag) || gameObject.tag == "Untagged")
            gameObject.tag = "Block";
    }

    public void Hit(int damage, Inventory inven)
    {
        if (!mineable) return;

        hp -= damage;

        if (hp <= 0)
        {
            if (inven != null && dropCount > 0)
                inven.Add(type, dropCount);

            if (type == ItemType.Dirt)
                TryExtraDrop(inven);

            Destroy(gameObject);
        }
    }
    void TryExtraDrop(Inventory inven)
    {
        if (inven == null) return;

        if (Random.value > extraDropChance)
            return;

        ItemType[] extraItems =
        {
            ItemType.Screw,
            ItemType.Can,
            ItemType.Plastic
        };

        ItemType randomItem =
            extraItems[Random.Range(0, extraItems.Length)];

        inven.Add(randomItem, 1);

        Debug.Log($"추가 드랍: {randomItem}");
    }

}
