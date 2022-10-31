using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//GameController script is subject to change as it was originally designed for single-player use

public class GameController : Singleton<GameController>
{
    private bool isDebug = true; //change this to true if you want to test the game without starting from the title screen
    public bool IsDebug { get { return isDebug; } }


    private int currentScore = 0;
    public int HighScore { get; private set; }

    public bool IsGameOver { get; private set; }
    public bool IsGameScene { get; private set; }

    public EnemySpawner EnemySpawner { private get; set; }
    public GameScreenUI GameScreenUI { get; private set; }  
    public PlayerController Player { get; private set; }

    public override void Awake()
    {
        base.Awake();
        IsGameOver = true;
        IsGameScene = false;
    }

    public void Start()
    {
        if(!isDebug) return;
            
        IsGameOver = false;
        HighScore = 0;
        currentScore = 0;

        Player = GameObject.Find("Player").GetComponent<PlayerController>();
        GameScreenUI = GameObject.Find("GameScreenUI").GetComponent<GameScreenUI>();

        IsGameScene = true;
        AudioManager.Instance.Play("GameSceneBGM");
    }

    public void LoadGame()
    {
        StartCoroutine(InitializeGame());
    }

    public void QuitToMainMenu()
    {
        // MULTIPLAYER IMPLEMENTATION COMMENTS
        // --> should remove room and associated game instance if room is empty
        // --> if room still has a player in it, do nothing when user quits the instance
        // --> if the host quits the game, change ownership to current player in the room

        IsGameScene = false;

        AudioManager.Instance.Stop("GameSceneBGM");
    }

    public void AddScore(int point)
    {
        currentScore += point;
        GameScreenUI.UpdateScoreUI(currentScore.ToString());
    }

    public void GameOver() //subject to change since this was originally designed for a single-player game
    {
        IsGameOver = true;

        string gameOverMessage;
        if (Player.Health > 0)
        {
            gameOverMessage = "YOU WIN";
            AudioManager.Instance.Play("PlayerWin");
        }
        else
        {
            gameOverMessage = "YOU LOSE";
            AudioManager.Instance.Play("PlayerLose");
        }

        HighScore = currentScore;
        GameScreenUI.EnableGameOverUI(gameOverMessage);
        Cursor.lockState = CursorLockMode.Confined;
        DisableGame();
    }

    private void Update()
    {
        if (IsGameOver) return;
        
        //will prolly do some stuff here idk yet
    }

    private IEnumerator InitializeGame() //originally written for single-player use, subject to change
    {
        IsGameOver = false;
        HighScore = 0;
        currentScore = 0;        

        yield return SceneManager.LoadSceneAsync("Game Scene");

        Player = GameObject.Find("Player").GetComponent<PlayerController>();
        GameScreenUI = GameObject.Find("GameScreenUI").GetComponent<GameScreenUI>();
        
        IsGameScene = true;
        AudioManager.Instance.Play("GameSceneBGM");
    }

    private void DisableGame()
    {
        if (EnemySpawner != null)
        {
            EnemySpawner.CancelInvoke();
            EnemySpawner.enabled = false;
        }

        GameObject.Find("SciFiHandGun").SetActive(false);

        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemy.Length; i++)
        {
            ObjectPoolManager.Instance.DespawnGameObject(enemy[i]);
        }
    }
}
