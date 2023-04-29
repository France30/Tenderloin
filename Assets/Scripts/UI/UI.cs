using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class UI
{
    [Header("Game UI")]
    public TextMeshProUGUI moneyUI;
    public TextMeshProUGUI waveCountUI;
    public TextMeshProUGUI enemyCountUI;
    public TextMeshProUGUI interactablePopUp;
    public TextMeshProUGUI ammoUI;

    [Header("PowerUp Inventory UI")]
    public Image[] inventoryIcon;


    public float alertTime = 2.5f;

    [Header("Wave Alert UI")]
    public GameObject nextWaveUI;

    [Header("Danger Alert UI")]
    public GameObject dangerUI;

    [Header("Health UI")]
    public Image healthBar;
    public TextMeshProUGUI healthTextUI;

    [Header("Damage UI")]
    public GameObject damageUI;
    public Image damageScreen;

    [Header("Game Over UI")]
    public GameObject gameOverUI;
}
