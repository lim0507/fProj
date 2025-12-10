using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    public Camera playerCamera;
    public Weapon weapon;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            UseWeapon();
    }

    void UseWeapon()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, weapon.range))
        {
            weapon.Use(hit);
        }
    }
}
