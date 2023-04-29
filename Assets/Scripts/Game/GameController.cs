using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
//GameController script is subject to change as it was originally designed for single-player use

public class GameController : SingletonPun<GameController>
{
    private bool isDebug = true; //change this to true if you want to test the game without starting from the title screen
    public bool IsDebug { get { return isDebug; } }


    private bool didPlayersWin;

    public int HighScore { get; private set; }

    public bool IsGameOver { get; private set; }
    public bool IsGameScene { get; private set; }


    public GameStats GameStats { get; private set; }

    public EnemySpawner EnemySpawner { get; set; }
    public ChestSpawner ChestSpawner { get; set; }
    public PowerUpSpawner PowerUpSpawner { get; set; }
    public GameScreenUI GameScreenUI { get; set; }
    public PlayerController Player { get; set; }

    protected override void Awake()
    {
        base.Awake();

        IsGameOver = true;
        IsGameScene = false;
        didPlayersWin = true;
        
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        GameStats = new GameStats();

        IsGameOver = false;
        HighScore = 0;

        GameScreenUI = GameObject.Find("GameScreenUI").GetComponent<GameScreenUI>();

        GameStats.Inventory = gameObject.AddComponent<Inventory>();

        IsGameScene = true;
        AudioManager.Instance.Play("GameSceneBGM");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameSync.Instance.SyncNewPlayerJoined(GameStats.CurrentEnemyCount, EnemySpawner.CurrentWaveCount, EnemySpawner.Difficulty, EnemySpawner.CurrentEnemyCount);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (IsGameOver) return;

        EnemyController[] enemy = FindObjectsOfType<EnemyController>();
        for (int i = 0; i < enemy.Length; i++)
            enemy[i].SetTarget(Player.PhotonView.ViewID);
      
        CheckIfAllPlayersDowned();
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        EnemySpawner.InvokeSpawnOnMasterChanged();
    }

    public void CheckIfAllPlayersDowned()
    {
        PlayerController[] players = FindObjectsOfType<PlayerController>();

        for(int i = 1; i <= players.Length; i++)
        {
            if (!players[i - 1].IsPlayerDown) return;
            if (i < players.Length) continue;

            didPlayersWin = false;
            GameOver();
        }
    }

    public void GameOver()
    {
        IsGameOver = true;

        string gameOverMessage;
        if (didPlayersWin)
        {
            gameOverMessage = "VICTORY";
            AudioManager.Instance.Play("PlayerWin");
        }
        else
        {
            gameOverMessage = "DEFEAT";
            AudioManager.Instance.Play("PlayerLose");
        }

        HighScore = GameStats.CurrentMoney;
        GameScreenUI.EnableGameOverUI(gameOverMessage);
        Cursor.lockState = CursorLockMode.None;
        DisableGame();
    }

    public void DespawnGameObjectsWithTag(string tag)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        GameObject[] gameObject = GameObject.FindGameObjectsWithTag(tag);
        for (int i = 0; i < gameObject.Length; i++)
        {
            PhotonNetwork.Destroy(gameObject[i]);
        }
    }

    private void DisableGame()
    {
        if (EnemySpawner != null)
        {
            EnemySpawner.CancelInvoke();
            EnemySpawner.enabled = false;
        }

        DespawnGameObjectsWithTag("Enemy");
        DespawnGameObjectsWithTag("Chest");
        DespawnGameObjectsWithTag("PowerUp");

        PhotonNetwork.CurrentRoom.IsOpen = false;
    }
}
