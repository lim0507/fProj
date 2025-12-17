using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public int currentStage = 1;

    [Header("References")]
    public PlayerMoney playerMoney;
    public Inventory inventory;
    public WeaponManager weaponManager;
    public ShopPanel shopPanel;

    public Text hintText;
    public Text stageText;

    public int baseTargetMoney = 100;
    public int increasePerStage = 100;

    bool canGoNext;

    int CurrentTargetMoney => baseTargetMoney + (currentStage - 1) * increasePerStage;

    void Start()
    {
        UpdateStageUI();
        ConfigureShopForStage();
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
        // 인벤 초기화
        inventory.ClearAll();

        // 돈 초기화
        playerMoney.ResetMoney();

        // 무기 초기화
        weaponManager.ResetWeapons();

        // 맵 초기화
        PerlinNoise mapGenerator = FindObjectOfType<PerlinNoise>();
        if (mapGenerator != null)
            mapGenerator.ResetMap();

        // 스테이지 증가
        currentStage++;

        UpdateStageUI();
        ConfigureShopForStage();
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

    void ConfigureShopForStage()
    {
        if (!shopPanel) return;

        // 기본 초기화: 모두 비활성화
        shopPanel.EnableWeaponShop(false);
        shopPanel.EnableUpgradeShop(false);
        shopPanel.EnableBuffShop(false);

        switch (currentStage)
        {
            case 1: // Stage 1: 무기만
                shopPanel.EnableWeaponShop(true);
                break;
            case 2: // Stage 2: 무기 + 업그레이드
                shopPanel.EnableWeaponShop(true);
                shopPanel.EnableUpgradeShop(true);
                break;
            case 3: // Stage 3: 무기 + 업그레이드 + 버프
                shopPanel.EnableWeaponShop(true);
                shopPanel.EnableUpgradeShop(true);
                shopPanel.EnableBuffShop(true);
                break;
            default: // 이후 스테이지는 모두 활성
                shopPanel.EnableWeaponShop(true);
                shopPanel.EnableUpgradeShop(true);
                shopPanel.EnableBuffShop(true);
                break;
        }
    }
}