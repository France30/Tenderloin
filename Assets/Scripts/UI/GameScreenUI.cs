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
    public GameObject GameOverRanking { get { return UI.gameOverRanking; } }
    public TextMeshProUGUI InteractablePopUp { get { return UI.interactablePopUp; } }

    public void UpdateMoneyUI(int money)
    {
        UI.moneyUI.text = '$' + money.ToString();
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

    public void UpdateInventoryIcon(List<Sprite> inventory)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            UI.inventoryIcon[i].sprite = inventory[i];
            UI.inventoryIcon[i].enabled = true;
        }
    }

    public void UpdateAmmoUI(int currentAmmo, int maxAmmo )
    {
        UI.ammoUI.text = currentAmmo.ToString() + " / " + maxAmmo.ToString();
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        UI.healthTextUI.text = currentHealth.ToString() + " / " + maxHealth.ToString();
        UI.healthBar.fillAmount = currentHealth / maxHealth;
    }

    public void UpdateDownedIndicator(bool isPlayerDowned, Transform target)
    {
        UI.arrowIndicatorUI.SetActive(isPlayerDowned);

        GameObject player = GameController.Instance.Player.gameObject;
        Vector3 dir = player.transform.InverseTransformPoint(target.position);
        float a = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        a += 180;
        UI.arrowIndicatorUI.transform.localEulerAngles = new Vector3(0, 180, a);

        //float distance = Vector3.Distance(player.transform.position, target.position);
        //Debug.Log(distance.ToString("0.00"));
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

    public void EnableGameOverUI(string message = "YOU WIN")
    {
        UI.gameOverUI.SetActive(true);
        UI.gameOverText.text = message;
        UI.gameOverScore.text = GameController.Instance.HighScore.ToString();
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
    }

    private void Update()
    {
        if (UI.nextWaveUI.activeSelf)
            NextWaveAlert();

        if (UI.dangerUI.activeSelf)
            DangerAlert();
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
