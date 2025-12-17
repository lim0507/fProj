using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlacer : MonoBehaviour
{
    [Header("References")]
    public Transform playerCamera;
    public float placeDistance = 3f;
    public Inventory inventory;          // 플레이어 인벤토리 연결
    public PerlinNoise perlinNoise;      // forbiddenPositions 체크용
    public ItemType placeBlockType = ItemType.Dirt; // 설치할 기본 블록 타입
    public GameObject dirtBlockPrefab;   // 설치할 프리팹
    public GameObject stoneBlockPrefab;
    public PlayerController player;
    void Update()
    {
        // 조합창 열리면 블록 설치 무시
        if (player.craftingPanel != null && player.craftingPanel.IsOpen)
            return;

        if (Input.GetMouseButtonDown(1)) // 우클릭 설치
        {
            TryPlaceBlock();
        }
    }

    void TryPlaceBlock()
    {
        if(player == null || player.craftingPanel == null || player.craftingPanel.IsOpen)
    {
            // 조합창 열려있거나 PlayerController 연결 안 됨
            return;
        }

        if (inventory == null) return;

        // 인벤토리에 설치할 블록이 있는지 확인
        if (inventory.GetCount(placeBlockType) <= 0)
        {
            Debug.Log("설치할 블록이 없습니다.");
            return;
        }

        

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, placeDistance))
        {
            // 설치 위치: 레이캐스트가 맞은 면 위
            Vector3 placePos = hit.point + hit.normal * 0.5f;
            placePos = new Vector3(
                Mathf.Round(placePos.x),
                Mathf.Round(placePos.y),
                Mathf.Round(placePos.z)
            );

            // forbiddenPositions 체크 (상점 등)
            if (perlinNoise != null)
            {
                Vector3Int checkPos = Vector3Int.RoundToInt(placePos);
                if (perlinNoise.forbiddenPositions.Contains(checkPos))
                {
                    Debug.Log("이 위치에는 블록을 설치할 수 없습니다.");
                    return;
                }
            }

            // 프리팹 선택
            GameObject prefabToPlace = placeBlockType == ItemType.Stone ? stoneBlockPrefab : dirtBlockPrefab;
            if (prefabToPlace == null) return;

            Instantiate(prefabToPlace, placePos, Quaternion.identity, perlinNoise.transform);

            // 인벤토리에서 1개 감소
            inventory.Consume(placeBlockType, 1);
        }
    }
}
