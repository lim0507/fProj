using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSpowner : MonoBehaviour
{
    public GameObject shopPrefab;

    void Start()
    {
        // 지표면 y=0 기준
        Vector3 groundPosition = new Vector3(0, 0, 0);
        Instantiate(shopPrefab, groundPosition, Quaternion.identity);
    }
}
