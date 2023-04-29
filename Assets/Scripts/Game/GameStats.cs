using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats
{
    private int currentMoney = 0;
    private int currentEnemyCount = 0;

    public int WavesSurvived { get; set; }
    public int EnemyKills { get; set; }
    public int TimesDowned { get; set; }
    public int PartnerRevives { get; set; }
    public Inventory Inventory { get; set; }

    public int CurrentMoney 
    { 
        get { return currentMoney; } 
        set
        {
            currentMoney = value;
            GameController.Instance.GameScreenUI.UpdateMoneyUI(currentMoney);
        }
    }

    public int CurrentEnemyCount
    {
        get { return currentEnemyCount; }
        set
        {
            currentEnemyCount = value;
            GameController.Instance.GameScreenUI.UpdateEnemyCountUI(currentEnemyCount);
        }
    }
}
