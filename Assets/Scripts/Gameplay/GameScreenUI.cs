using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameScreenUI : MonoBehaviour
{
    [Header("Game UI")]
    [SerializeField] private TextMeshProUGUI scoreUI;
    [SerializeField] private TextMeshProUGUI waveCountUI;

    [SerializeField] private float alertTime = 2.5f;

    [Header("Wave Alert UI")]
    [SerializeField] private GameObject nextWaveUI;

    [Header("Danger Alert UI")]
    [SerializeField] private GameObject dangerUI;

    [Header("Health UI")]
    [SerializeField] private GameObject[] healthBar;

    [Header("Damage UI")]
    [SerializeField] private GameObject damageUI;
    [SerializeField] private Image damageScreen;

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI gameOverScore;
    [SerializeField] private GameObject gameOverRanking;

    private float timer;
    private Color initialDamageScreenColor;
    public Color DamageScreenColor { get { return damageScreen.color; } private set { damageScreen.color = value; } }
    public GameObject GameOverRanking { get { return gameOverRanking; } }

    public void UpdateScoreUI(string score)
    {
        scoreUI.text = score;
    }

    public void UpdateWaveCount(string waveCount)
    {
        waveCountUI.text = waveCount;
        nextWaveUI.SetActive(true);
        timer = alertTime;
    }

    public void DisableHealthBar(int healthCount)
    {
        healthBar[healthCount].SetActive(false);
    }

    public void EnableDangerUI()
    {
        dangerUI.SetActive(true);
        timer = alertTime;
    }

    public void DamageUI(float alpha)
    {
        if (!damageUI.activeSelf)
            damageUI.SetActive(true);

        DamageScreenColor = new Color(damageScreen.color.r, damageScreen.color.g, damageScreen.color.b, alpha);

        if (alpha <= 0)
        {
            damageUI.SetActive(false);
            DamageScreenColor = initialDamageScreenColor; //reset the color
        }
    }

    public void EnableGameOverUI(string message = "YOU WIN")
    {
        gameOverUI.SetActive(true);
        gameOverText.text = message;
        gameOverScore.text = GameController.Instance.HighScore.ToString();
    }

    public void DisableGameUI()
    {
        dangerUI.SetActive(false);
        nextWaveUI.SetActive(false);
        damageUI.SetActive(false);
        gameObject.SetActive(false);
    }

    private void Start()
    {
        initialDamageScreenColor = DamageScreenColor; //hold the initial DamageScreenColor
    }

    private void Update()
    {
        if (nextWaveUI.activeSelf)
            NextWaveAlert();

        if (dangerUI.activeSelf)
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
            nextWaveUI.SetActive(false);
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

        dangerUI.SetActive(false);
    }
}
