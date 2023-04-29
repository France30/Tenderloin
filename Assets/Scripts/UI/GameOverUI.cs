using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameOverMessageText;
    [SerializeField] private TextMeshProUGUI wavesSurvivedText;
    [SerializeField] private TextMeshProUGUI enemyKillsText;
    [SerializeField] private TextMeshProUGUI moneyText;

    [SerializeField] private Image[] inventoryIcon;

    private void OnEnable()
    {
        wavesSurvivedText.text = GameController.Instance.GameStats.WavesSurvived.ToString();
        enemyKillsText.text = GameController.Instance.GameStats.EnemyKills.ToString();
        moneyText.text ="$" + GameController.Instance.GameStats.CurrentMoney.ToString();

        GameController.Instance.GameStats.Inventory.UpdateInventoryIcon(inventoryIcon);
    }

    public void SetGameOverMessage(string message = "VICTORY")
    {
        gameOverMessageText.text = message;
    }
}
