using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    [Header("Stage Info")]
    public int currentStage = 1;
    public int baseTargetMoney = 100;
    public int increasePerStage = 100;

    [Header("References")]
    public PlayerMoney playerMoney;
    public Inventory inventory;
    public WeaponManager weaponManager;
    public ShopPanel shopPanel;

    [Header("UI")]
    public Text hintText;
    public Text stageText;

    [Header("Shop Prefabs")]
    public GameObject[] shopPrefabs; // Stage별 상점 프리팹
    private GameObject currentShop;

    private bool canGoNext = false;

    int CurrentTargetMoney => baseTargetMoney + (currentStage - 1) * increasePerStage;

    void Start()
    {
        UpdateStageUI();
        SetupStageShop();
    }

    void Update()
    {
        if (!canGoNext && playerMoney.money >= CurrentTargetMoney)
        {
            canGoNext = true;
            ShowHint($"M 키를 눌러 다음 지역으로 이동 (목표 {CurrentTargetMoney}$ 달성)");
        }

        if (canGoNext && Input.GetKeyDown(KeyCode.M))
        {
            GoNextStage();
        }
    }

    void GoNextStage()
    {
        // 인벤토리, 돈, 무기 초기화
        inventory.ClearAll();
        if (playerMoney != null)
            playerMoney.ResetMoney();
        weaponManager.ResetWeapons();

        // 이전 상점 제거
        if (currentShop != null)
            Destroy(currentShop);

        // 맵 초기화
        PerlinNoise perlin = FindObjectOfType<PerlinNoise>();
        if (perlin != null)
            perlin.ResetMap();

        // 다음 스테이지
        currentStage++;
        UpdateStageUI();
        SetupStageShop();
        canGoNext = false;
        ShowHint($"Stage {currentStage} 시작!");
    }

    void UpdateStageUI()
    {
        if (stageText)
            stageText.text = $"Stage {currentStage}\n목표: {CurrentTargetMoney}$";
    }

    void ShowHint(string msg)
    {
        if (hintText)
            hintText.text = msg;
    }

    void SetupStageShop()
    {
        // stageIndex가 shopPrefabs 범위를 넘지 않도록
        int stageIndex = Mathf.Min(currentStage - 1, shopPrefabs.Length - 1);
        if (shopPrefabs.Length == 0 || shopPrefabs[stageIndex] == null) return;

        // 상점 생성 (지표면에 배치)
        Vector3 shopPosition = Vector3.zero;
        if (Camera.main != null)
        {
            shopPosition = Camera.main.transform.position + Camera.main.transform.forward * 5f;
            shopPosition.y = 0f; // 지표면 높이로 고정
        }

        currentShop = Instantiate(shopPrefabs[stageIndex], shopPosition, Quaternion.identity);

        // ShopPanel 가져오기
        shopPanel = currentShop.GetComponentInChildren<ShopPanel>();

        // 여기서 Scene의 실제 References 연결
        shopPanel.playerMoney = playerMoney;
        shopPanel.inventory = inventory;
        shopPanel.weaponManager = weaponManager;
        shopPanel.upgradeManager = FindObjectOfType<UpgradeManager>();
        shopPanel.buffManager = FindObjectOfType<BuffManager>();

        // UI 초기화
        shopPanel.SetOpen(false);
        shopPanel.SetModeBuy();
        shopPanel.SetInfo("상점");

        // Stage별 상점 기능 세팅
        ConfigureShopForStage();
    }
    void ConfigureShopForStage()
    {
        if (shopPanel == null) return;

        // 모든 상점 UI 비활성화
        shopPanel.EnableWeaponShop(false);
        shopPanel.EnableUpgradeShop(false);
        shopPanel.EnableBuffShop(false);

        // Stage별 활성화
        switch (currentStage)
        {
            case 1:
                shopPanel.EnableWeaponShop(true);
                break;
            case 2:
                shopPanel.EnableWeaponShop(true);
                shopPanel.EnableUpgradeShop(true);
                break;
            case 3:
                shopPanel.EnableWeaponShop(true);
                shopPanel.EnableUpgradeShop(true);
                shopPanel.EnableBuffShop(true);
                break;
            default:
                shopPanel.EnableWeaponShop(true);
                shopPanel.EnableUpgradeShop(true);
                shopPanel.EnableBuffShop(true);
                break;
        }
    }

}

