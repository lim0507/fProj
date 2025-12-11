using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Transform hand;
    public Weapon currentWeapon;

    [Header("Weapon List")]
    public GameObject[] weaponPrefabs;

    int currentIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        currentWeapon = hand.GetComponentInChildren<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchToNextWeapon();
        }
    }
    public void SwitchToNextWeapon()
    {
        if (weaponPrefabs.Length == 0) return;
    }

    public void DrillWeapon(GameObject WeaponPrefab)
    {

    }
    public void Attack()
    {
        if(currentWeapon != null)
        {
            currentWeapon.Attack();
        }
    }
}
