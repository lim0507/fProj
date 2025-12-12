using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName;
    public float attackCooldown = 0.3f;

    float lastAttackTime = 0f;

    public virtual void Attack()
    {
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        lastAttackTime = Time.time;

        // 메인 카메라 기준으로 Raycast 쏘기 (1인칭)
        Camera cam = Camera.main;

        if (cam == null)
        {
            Debug.LogError("Camera.main 없음! 카메라에 MainCamera 태그 붙었는지 확인");
            return;
        }

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3f))
        {
            DirtBlock dirt = hit.collider.GetComponent<DirtBlock>();

            if (dirt != null)
            {
                dirt.Hit();  // 흙 체력 깎기
                Debug.Log("Hit dirt!");
            }
            else
            {
                Debug.Log("Ray hit but no DirtBlock");
            }
        }
        else
        {
            Debug.Log("Ray missed");
        }
    }
}
