using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ShopMode
{
    Sell,
    Buy
}
public class ShopPanel : MonoBehaviour
{
    [Header("References")]
    public Inventory inventory;
    public PlayerMoney playerMoney;
    public WeaponManager weaponManager;

    [Header("UI")]
    public GameObject root;
    public Text infoText;

    [Header("Mode Groups")]
    public GameObject sellGroup;
    public GameObject buyGroup;

    [Header("Tables")]
    public List<ShopItem> itemShopTable;          // 아이템(흙, 나사 등)
    public List<WeaponShopItem> weaponShopTable;  // 무기 전용
    public List<WeaponShopItem> weaponSellTable;

    ShopMode currentMode = ShopMode.Sell;
    bool isOpen;
    public bool IsOpen => isOpen;

    void Start()
    {
        SetOpen(false);
        SetModeSell();
        SetInfo("상점");
    }

    /* =========================
     * 상점 열기 / 닫기
     * ========================= */
    public void SetOpen(bool open)
    {
        isOpen = open;
        if (root)
            root.SetActive(open);

        PlayerController player = FindObjectOfType<PlayerController>();

        if (open)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (player)
                player.inputLocked = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (player)
                player.inputLocked = false;
        }
    }

    /* =========================
     * 모드 전환
     * ========================= */
    public void SetModeSell()
    {
        currentMode = ShopMode.Sell;
        RefreshModeUI();
    }

    public void SetModeBuy()
    {
        currentMode = ShopMode.Buy;
        RefreshModeUI();
    }

    void RefreshModeUI()
    {
        if (sellGroup)
            sellGroup.SetActive(currentMode == ShopMode.Sell);

        if (buyGroup)
            buyGroup.SetActive(currentMode == ShopMode.Buy);

        SetInfo(currentMode == ShopMode.Sell ? "판매 모드" : "구매 모드");
    }

    /* =========================
     * 아이템 판매
     * ========================= */
    public void Sell(ItemType type)
    {
        if (inventory.GetCount(type) <= 0)
        {
            SetInfo("아이템이 없습니다.");
            return;
        }

        ShopItem item = itemShopTable.Find(x => x.type == type);
        if (item == null)
        {
            SetInfo("판매 불가 아이템");
            return;
        }

        inventory.Consume(type, 1);
        playerMoney.AddMoney(item.sellPrice);

        SetInfo($"{type} 판매 +{item.sellPrice}$");
    }

    /* =========================
     * 아이템 구매
     * ========================= */
    public void BuyItem(ItemType type)
    {
        ShopItem item = itemShopTable.Find(x => x.type == type);
        if (item == null)
        {
            SetInfo("구매 불가 아이템");
            return;
        }

        if (playerMoney.money < item.buyPrice)
        {
            SetInfo("돈이 부족합니다.");
            return;
        }

        playerMoney.AddMoney(-item.buyPrice);
        inventory.Add(type, item.buyCount);

        SetInfo($"{type} 구매 완료 (-{item.buyPrice}$)");
    }

    
    public void BuyWeapon(WeaponType type)
    {
        if (!weaponManager)
        {
            SetInfo("WeaponManager 없음");
            return;
        }

        bool success = weaponManager.BuyWeapon(type, playerMoney);

        if (success)
            SetInfo($"{type} 구매 완료");
        else
            SetInfo("구매 실패");
    }
    public void BuyShovel()
    {
        BuyWeaponInternal(WeaponType.Shovel);
    }

    public void BuyDrill()
    {
        BuyWeaponInternal(WeaponType.Drill);
    }

    public void BuyBomb()
    {
        BuyWeaponInternal(WeaponType.Bomb);
    }

    void BuyWeaponInternal(WeaponType type)
    {
        if (!weaponManager)
        {
            SetInfo("WeaponManager 없음");
            return;
        }

        bool success = weaponManager.BuyWeapon(type, playerMoney);

        if (success)
            SetInfo($"{type} 구매 완료");
        else
            SetInfo("구매 실패");
    }
    void SetInfo(string msg)
    {
        if (infoText)
            infoText.text = msg;
    }
}
