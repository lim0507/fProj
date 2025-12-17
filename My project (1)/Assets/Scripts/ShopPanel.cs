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
    public UpgradeManager upgradeManager;
    public BuffManager buffManager;
    public BuffData damageBuff;
    public BuffData speedBuff;
    public BuffData dropBuff;

    [Header("UI")]
    public GameObject root;
    public Text infoText;

    [Header("Mode Groups")]
    public GameObject sellGroup;
    public GameObject buyGroup;

    [Header("Tables")]
    public List<ShopItem> itemShopTable;         
    public List<WeaponShopItem> weaponShopTable;  
    public List<WeaponShopItem> weaponSellTable;
    public List<ShopItem> sellTable;

    ShopMode currentMode = ShopMode.Sell;
    bool isOpen;
    public bool IsOpen => isOpen;

    public GameObject weaponGroup;
    public GameObject upgradeGroup;
    public GameObject buffGroup;
    void Start()
    {
        SetOpen(false);
        SetModeSell();
        SetInfo("상점");
    }
    void Awake()
    {
        if (sellTable == null)
            sellTable = new List<ShopItem>();
    }

   
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
    public void SellAll()
    {
        bool soldAny = false;

        foreach (var item in sellTable)
        {
            int count = inventory.GetCount(item.type);
            if (count <= 0) continue;

            inventory.Consume(item.type, count);
            playerMoney.AddMoney(item.sellPrice * count);
            soldAny = true;
        }

        if (soldAny)
            SetInfo("모두 판매 완료");
        else
            SetInfo("판매할 아이템이 없습니다.");
    }
    public void ApplyStageShop(ShopStageData data)
    {
        if (data == null)
        {
            Debug.LogError("ShopStageData가 null");
            return;
        }

        // 테이블 교체
        weaponShopTable = data.weaponShopTable;
        itemShopTable = data.itemShopTable;
        sellTable = data.sellTable;

        // UI 그룹 전환
        switch (data.shopType)
        {
            case ShopStageType.Weapon:
                buyGroup.SetActive(true);
                break;

            case ShopStageType.Upgrade:
                buyGroup.SetActive(true);
                break;

            case ShopStageType.Buff:
                buyGroup.SetActive(true);
                break;
        }

        SetInfo($"스테이지 {data.stage} 상점");
    }
    public void BuyDoubleDropBuff()
    {
        if (playerMoney.money < 100)
        {
            SetInfo("돈이 부족합니다.");
            return;
        }

        playerMoney.AddMoney(-100);
        buffManager.ApplyBuff(
            BuffType.DoubleDrop,
            1f,
            30f   // 30초
        );

        SetInfo("30초간 드랍 2배!");
    }
    public void BuyDamageUpgrade()
    {
        int price = upgradeManager.GetDamageUpgradePrice();

        if (playerMoney.money < price)
        {
            SetInfo("돈이 부족합니다.");
            return;
        }

        playerMoney.AddMoney(-price);
        upgradeManager.UpgradeDamage();

        SetInfo($"데미지 업그레이드! (-{price}$)");
    }
    public void BuySpeedUpgrade()
    {
        int price = upgradeManager.GetSpeedUpgradePrice();

        if (playerMoney.money < price)
        {
            SetInfo("돈이 부족합니다.");
            return;
        }

        playerMoney.AddMoney(-price);
        upgradeManager.UpgradeSpeed();

        SetInfo($"공격속도 업그레이드! (-{price}$)");
    }
    public void BuyDamageBuff()
    {
        BuyBuff(damageBuff);
    }
    public void BuySpeedBuff()
    {
        BuyBuff(speedBuff); BuyBuff(speedBuff);
    }
    public void BuyDropBuff()
    {
        BuyBuff(dropBuff);
    }

    void BuyBuff(BuffData buff)
    {
        if (playerMoney.money < buff.price)
        {
            SetInfo("돈이 부족합니다.");
            return;
        }

        playerMoney.AddMoney(-buff.price);
        buffManager.ApplyBuff(buff.type, buff.value, buff.duration);

        SetInfo($"{buff.type} 버프 적용!");
    }
    public void EnableWeaponShop(bool enable)
    {
        if (weaponGroup) weaponGroup.SetActive(enable);
    }

    public void EnableUpgradeShop(bool enable)
    {
        if (upgradeGroup) upgradeGroup.SetActive(enable);
    }

    public void EnableBuffShop(bool enable)
    {
        if (buffGroup) buffGroup.SetActive(enable);
    }
}
