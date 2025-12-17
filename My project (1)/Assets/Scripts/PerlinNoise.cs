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

    [Header("Shop")]
    public Transform shopTransform; // 상점 위치 연결

    HashSet<Vector3Int> generatedBlocks = new HashSet<Vector3Int>();
    public HashSet<Vector3Int> forbiddenPositions = new HashSet<Vector3Int>();

    int lastGeneratedDepth = 0;

    void Start()
    {
        // 상점 위치를 forbiddenPositions에 등록
        if (shopTransform != null)
        {
            Vector3Int shopPos = Vector3Int.RoundToInt(shopTransform.position);
            forbiddenPositions.Add(shopPos);

            // 상점 주변 1x1x1 영역 블록도 방지
            forbiddenPositions.Add(shopPos + Vector3Int.up);
            forbiddenPositions.Add(shopPos + Vector3Int.down);
            forbiddenPositions.Add(shopPos + Vector3Int.left);
            forbiddenPositions.Add(shopPos + Vector3Int.right);
            forbiddenPositions.Add(shopPos + new Vector3Int(0, 0, 1));
            forbiddenPositions.Add(shopPos + new Vector3Int(0, 0, -1));
        }
    }

    void Update()
    {
        GenerateDownward();
    }

    public void ResetMap()
    {
        // 기존 블록 제거
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        // 생성 기록 초기화
        generatedBlocks.Clear();
        lastGeneratedDepth = 0;
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
                    Vector3Int blockPos = new Vector3Int(Mathf.RoundToInt(player.position.x) + x,y,Mathf.RoundToInt(player.position.z) + z
);    
                    // 이미 생성했거나 forbidden 위치면 건너뜀
                    if (generatedBlocks.Contains(blockPos) || forbiddenPositions.Contains(blockPos))
                        continue;

                    // 지표면 위 블록은 생성 안 함
                    if (blockPos.y > 0)
                        continue;

                    GameObject prefab = blockPos.y <= stoneStartDepth ? stoneBlock : dirtBlock;

                    Instantiate(prefab, blockPos, Quaternion.identity, transform);
                    generatedBlocks.Add(blockPos);
                }
            }
        }

        lastGeneratedDepth = endY;
    }
}
