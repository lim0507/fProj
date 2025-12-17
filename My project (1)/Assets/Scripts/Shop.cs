using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public ShopPanel shopPanel;
    public Text hintText;

    bool playerInRange = false;

    void Start()
    {
        if (hintText)
            hintText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            shopPanel.SetOpen(!shopPanel.IsOpen);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;

        if (hintText)
            hintText.gameObject.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;

        if (hintText)
            hintText.gameObject.SetActive(false);

        shopPanel.SetOpen(false);
    }
}