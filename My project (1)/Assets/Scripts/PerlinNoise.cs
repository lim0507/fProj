using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    public Transform player;

    [Header("Map Size")]
    public int horizontalRange = 8;   // 고정 가로 범위
    public int maxDepth = 50;          // 전체 최대 깊이
    public int generateDepthRange = 10; // 플레이어 아래 생성 범위

    [Header("Stone Rule")]
    public int stoneStartDepth = -15; // 이 깊이부터 돌 등장

    public GameObject dirtBlock;
    public GameObject stoneBlock;

    HashSet<Vector3Int> generatedBlocks = new HashSet<Vector3Int>();

    int lastGeneratedDepth = 0;

    void Update()
    {
        GenerateDownward();
    }

    void GenerateDownward()
    {
        int playerY = Mathf.FloorToInt(player.position.y);

        // 더 깊이 내려가지 않았으면 생성 안 함
        if (playerY >= lastGeneratedDepth)
            return;

        int startY = playerY;
        int endY = Mathf.Max(playerY - generateDepthRange, -maxDepth);

        for (int y = startY; y >= endY; y--)
        {
            for (int x = -horizontalRange; x <= horizontalRange; x++)
            {
                for (int z = -horizontalRange; z <= horizontalRange; z++)
                {
                    Vector3Int blockPos = new Vector3Int(x, y, z);

                    if (generatedBlocks.Contains(blockPos))
                        continue;

                    if (blockPos.y > 0)
                        continue;

                    GameObject prefab =
                        blockPos.y <= stoneStartDepth ? stoneBlock : dirtBlock;

                    Instantiate(prefab, blockPos, Quaternion.identity, transform);
                    generatedBlocks.Add(blockPos);
                }
            }
        }

        lastGeneratedDepth = endY;
    }
}
