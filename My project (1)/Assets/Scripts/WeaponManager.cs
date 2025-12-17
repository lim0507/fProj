using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Shovel,
    Drill,
    Bomb
}

public class WeaponManager : MonoBehaviour
{
    public Transform hand;
    public Weapon currentWeapon;

    [Header("Weapon List")]
    public GameObject[] weaponPrefabs;   // 전체 무기 프리팹
    public WeaponType[] weaponTypes;     // 프리팹과 같은 순서

    [Header("Weapon Shop")]
    public List<WeaponShopItem> weaponShopItems;

    int currentIndex = 0;

    // 해금된 무기 목록
    HashSet<WeaponType> unlockedWeapons = new HashSet<WeaponType>();

    void Start()
    {
        // 기본 무기 해금 (삽)
        UnlockWeapon(WeaponType.Shovel);

        EquipUnlockedFirst();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchToNextWeapon();
        }
    }

    public bool BuyWeapon(WeaponType type, PlayerMoney money)
    {
        if (HasWeapon(type))
        {
            Debug.Log("이미 구매한 무기");
            return false;
        }

        WeaponShopItem item =
            weaponShopItems.Find(x => x.weaponType == type);

        if (item == null)
        {
            Debug.LogError("무기 상점 데이터 없음");
            return false;
        }

        if (money.money < item.buyPrice)
        {
            Debug.Log("돈 부족");
            return false;
        }

        money.AddMoney(-item.buyPrice);
        UnlockWeapon(type);

        Debug.Log($"무기 구매 완료: {type}");
        return true;
    }
    public void UnlockWeapon(WeaponType type)
    {
        if (unlockedWeapons.Contains(type))
            return;

        unlockedWeapons.Add(type);
        Debug.Log($"[Weapon] 해금됨: {type}");
    }

    public bool HasWeapon(WeaponType type)
    {
        return unlockedWeapons.Contains(type);
    }

    public void SwitchToNextWeapon()
    {
        int count = Mathf.Min(weaponPrefabs.Length, weaponTypes.Length);
        if (count == 0) return;

        for (int i = 0; i < count; i++)
        {
            currentIndex++;
            if (currentIndex >= count)
                currentIndex = 0;

            if (unlockedWeapons.Contains(weaponTypes[currentIndex]))
            {
                EquipWeapon(weaponPrefabs[currentIndex]);
                return;
            }
        }
    }

    void EquipUnlockedFirst()
    {
        int count = Mathf.Min(weaponPrefabs.Length, weaponTypes.Length);

        for (int i = 0; i < count; i++)
        {
            if (unlockedWeapons.Contains(weaponTypes[i]))
            {
                currentIndex = i;
                EquipWeapon(weaponPrefabs[i]);
                return;
            }
        }

        Debug.LogError("[WeaponManager] 해금된 무기를 찾을 수 없음");
    }

    public void EquipWeapon(GameObject weaponPrefab)
    {
        if (currentWeapon != null)
            Destroy(currentWeapon.gameObject);

        GameObject newWeapon = Instantiate(weaponPrefab, hand);
        currentWeapon = newWeapon.GetComponent<Weapon>();

        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localRotation = Quaternion.identity;
    }

    public void Attack()
    {
        if (currentWeapon != null)
            currentWeapon.Attack();
    }
    public void ResetWeapons()
    {
        if (currentWeapon != null)
            Destroy(currentWeapon.gameObject);

        unlockedWeapons.Clear();
        UnlockWeapon(WeaponType.Shovel); // 기본 무기만 해금
        EquipUnlockedFirst();
    }
}