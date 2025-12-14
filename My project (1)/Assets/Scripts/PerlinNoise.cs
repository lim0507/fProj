using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    [Header("Map Size")]
    public int holeSize = 100;
    public int maxDepth = 100;

    [Header("Prefabs")]
    public GameObject dirtBlock;
    public GameObject StoneBlock;
    public GameObject housePrefab;

    void Start()
    {
        GenerateSurface();
        GenerateUnderground();
    }

    void GenerateSurface()
    {
        // 지표면 먼저 생성
        for (int x = -holeSize; x <= holeSize; x++)
        {
            for (int z = -holeSize; z <= holeSize; z++)
            {
                Instantiate(dirtBlock,
                    new Vector3(x, 0, z),
                    Quaternion.identity,
                    transform);
            }
        }

        // 집을 지표면 위에 배치
        Instantiate(housePrefab, new Vector3(0, 1, 0), Quaternion.identity);
    }

    void GenerateUnderground()
    {
        for (int x = -holeSize; x <= holeSize; x++)
        {
            for (int z = -holeSize; z <= holeSize; z++)
            {
                for (int y = -1; y >= -maxDepth; y--)
                {
                    Instantiate(dirtBlock,
                        new Vector3(x, y, z),
                        Quaternion.identity,
                        transform);
                }
            }
        }
    }
}
