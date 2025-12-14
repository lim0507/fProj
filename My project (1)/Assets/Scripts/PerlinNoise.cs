using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    public Transform player;

    [Header("Generation")]
    public int horizontalRange = 8;   // 플레이어 기준 가로 반경
    public int depthRange = 15;        // 아래로 생성할 깊이
    public int maxDepth = 50;          // 전체 최대 깊이

    public GameObject dirtBlock;
    public GameObject stoneBlock;

    HashSet<Vector3Int> generatedBlocks = new HashSet<Vector3Int>();

    void Update()
    {
        GenerateAroundPlayer();
    }

    void GenerateAroundPlayer()
    {
        Vector3Int playerPos = Vector3Int.RoundToInt(player.position);

        for (int x = -horizontalRange; x <= horizontalRange; x++)
        {
            for (int z = -horizontalRange; z <= horizontalRange; z++)
            {
                for (int y = -depthRange; y <= 1; y++)
                {
                    int worldY = playerPos.y + y;
                    if (worldY < -maxDepth) continue;

                    Vector3Int blockPos = new Vector3Int(
                        playerPos.x + x,
                        worldY,
                        playerPos.z + z
                    );

                    if (generatedBlocks.Contains(blockPos)) continue;

                    // 지표면 위는 생성 안 함
                    if (blockPos.y > 0) continue;

                    float noise = Mathf.PerlinNoise(
                        blockPos.x * 0.1f,
                        blockPos.z * 0.1f
                    );

                    GameObject prefab = noise > 0.6f ? stoneBlock : dirtBlock;

                    Instantiate(prefab, blockPos, Quaternion.identity, transform);
                    generatedBlocks.Add(blockPos);
                }
            }
        }
    }
}
