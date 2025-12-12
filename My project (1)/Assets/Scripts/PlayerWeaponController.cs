using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    public Transform hand;
    public Weapon currentWeapon;

    void Start()
    {
        // 시작 시 손 안에 있는 무기 자동 인식
        currentWeapon = hand.GetComponentInChildren<Weapon>();
    }

    public void EquipWeapon(GameObject weaponPrefab)
    {
        // 기존 무기 제거
        if (currentWeapon != null)
            Destroy(currentWeapon.gameObject);

        // 새 무기 생성
        GameObject newWeapon = Instantiate(weaponPrefab, hand);
        currentWeapon = newWeapon.GetComponent<Weapon>();

        // 손 좌표 기준 맞추기
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localRotation = Quaternion.identity;
    }

    public void Attack()
    {
        if (currentWeapon != null)
        {
            currentWeapon.Attack();
        }
    }
}
