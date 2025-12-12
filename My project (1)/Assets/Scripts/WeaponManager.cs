using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Transform hand;
    public Weapon currentWeapon;

    [Header("Weapon List")]
    public GameObject[] weaponPrefabs;   // 삽, 드릴, 폭탄 …

    int currentIndex = 0;

    void Start()
    {
        // 시작 시 손 안에 있는 무기 자동 인식
        currentWeapon = hand.GetComponentInChildren<Weapon>();
    }

    void Update()
    {
        // TAB 키 눌러 무기 교체
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchToNextWeapon();
        }
    }

    public void SwitchToNextWeapon()
    {
        if (weaponPrefabs.Length == 0) return;

        currentIndex++;

        // 리스트 순환
        if (currentIndex >= weaponPrefabs.Length)
            currentIndex = 0;

        EquipWeapon(weaponPrefabs[currentIndex]);
    }

    public void EquipWeapon(GameObject weaponPrefab)
    {
        // 기존 무기 제거
        if (currentWeapon != null)
            Destroy(currentWeapon.gameObject);

        // 새 무기 생성
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
}
