using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameScreenUI : MonoBehaviour
{
    [SerializeField] private UI UI;

    private float timer;
    private Color initialDamageScreenColor;
    public Color DamageScreenColor { get { return UI.damageScreen.color; } private set { UI.damageScreen.color = value; } }
    public Image[] InventoryIcon { get { return UI.inventoryIcon; } }

    public void UpdateMoneyUI(int money)
    {
        UI.moneyUI.text = '$' + money.ToString();
        AudioManager.Instance.Play("MoneyPickUp");
    }

    public void UpdateEnemyCountUI(int enemyCount)
    {
        UI.enemyCountUI.text = enemyCount.ToString();
    }

    public void UpdateWaveCount(string waveCount)
    {
        UI.waveCountUI.text = waveCount;
        UI.nextWaveUI.SetActive(true);
        timer = UI.alertTime;
    }

    public void UpdateAmmoUI(int currentAmmo, int maxAmmo )
    {
        UI.ammoUI.text = currentAmmo.ToString() + " / " + maxAmmo.ToString();
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        UI.healthTextUI.text = currentHealth.ToString();
        UI.healthBar.fillAmount = currentHealth / maxHealth;
    }

    public void UpdateInteractablePopUp(string interactableInfo)
    {
        UI.interactablePopUp.text = interactableInfo;
    }

    public void EnableDangerUI()
    {
        UI.dangerUI.SetActive(true);
        timer = UI.alertTime;
    }

    public void DamageUI(float alpha)
    {
        if (!UI.damageUI.activeSelf)
            UI.damageUI.SetActive(true);

        DamageScreenColor = new Color(UI.damageScreen.color.r, UI.damageScreen.color.g, UI.damageScreen.color.b, alpha);

        if (alpha <= 0)
        {
            UI.damageUI.SetActive(false);
            DamageScreenColor = initialDamageScreenColor; //reset the color
        }
    }

    public void EnableGameOverUI(string message = "VICTORY")
    {
        UI.gameOverUI.SetActive(true);
        UI.gameOverUI.GetComponent<GameOverUI>().SetGameOverMessage(message);
    }

    public void DisableGameUI()
    {
        UI.dangerUI.SetActive(false);
        UI.nextWaveUI.SetActive(false);
        UI.damageUI.SetActive(false);
        gameObject.SetActive(false);
    }

    private void Start()
    {
        initialDamageScreenColor = DamageScreenColor; //hold the initial DamageScreenColor

        UI.nextWaveUI.SetActive(false);
        UI.damageUI.SetActive(false);
        UI.gameOverUI.SetActive(false);
    }

    private void Update()
    {
        if (UI.nextWaveUI.activeSelf)
            NextWaveAlert();
    }

    private void NextWaveAlert()
    {
        if(!AudioManager.Instance.IsPlaying("NextWaveAlert"))
            AudioManager.Instance.Play("NextWaveAlert");

        if (timer > 0)
            timer -= Time.deltaTime;

        else 
        {
            UI.nextWaveUI.SetActive(false);
            AudioManager.Instance.Stop("NextWaveAlert");
        }
    }

    private void DangerAlert()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }

        UI.dangerUI.SetActive(false);
    }
}
