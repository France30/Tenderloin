using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemySpawner : Spawner
{ 
    [SerializeField] private List<string> enemies;
    
    [SerializeField] private float timeBetweenWaves;
    [SerializeField] private EnemyWave[] wave;

    private int currentEnemyCount;
    private int currentWaveCount = 0;

    private float resumeTime;

    private PhotonView photonView;

    private bool isValuesInitialized = false;

    public string Difficulty { get; set; }
    public int CurrentEnemyCount { get { return currentEnemyCount; } set { currentEnemyCount = value; } }
    public int CurrentWaveCount { get { return currentWaveCount; } 
        set 
        { 
            currentWaveCount = value; 
            GameController.Instance.GameScreenUI.UpdateWaveCount((currentWaveCount + 1).ToString()); 
        } 
    }

    public void InvokeSpawnOnMasterChanged()
    {
        InvokeRepeating("SpawnEnemies", resumeTime, wave[currentWaveCount].spawnTime);
    }

    private void Start()
    {
        photonView = GetComponent<PhotonView>();

        Difficulty = "Easy";

        resumeTime = timeBetweenWaves;

        if(PhotonNetwork.IsMasterClient)
            InvokeRepeating("SpawnEnemies", timeBetweenWaves, wave[0].spawnTime);

        GameController.Instance.EnemySpawner = this;
    }

    private void Update()
    {
        InitializeValues(); //only runs once

        if (resumeTime > 0)
            resumeTime -= Time.deltaTime;

        if (!PhotonNetwork.IsMasterClient) return;

        bool isEnemyAlive = GameObject.FindGameObjectWithTag("Enemy") != null;
        if (!IsInvoking("SpawnEnemies") && !isEnemyAlive)
        {
            photonView.RPC("RPCNextWave", RpcTarget.All);
        }
    }

    private void InitializeValues()
    {
        if (isValuesInitialized) return;

        try
        {
            GameController.Instance.GameStats.CurrentEnemyCount = wave[0].totalEnemies;
            GameController.Instance.GameStats.WavesSurvived = 0;
            GameController.Instance.GameScreenUI.UpdateWaveCount("1");
            isValuesInitialized = true;
        }
        catch
        {
            //do nothing
        }
    }

    private void SpawnEnemies()
    {
        bool waveGoalReached = currentEnemyCount >= wave[currentWaveCount].totalEnemies;
        if (waveGoalReached)
        {
            CancelInvoke();
            return;
        }

        resumeTime = wave[currentWaveCount].spawnTime;
        
        Transform parent = GetRandomSpawnPoint();
        int enemyCount;
        int enemiesLeft = wave[currentWaveCount].totalEnemies - currentEnemyCount;
        if (wave[currentWaveCount].maxEnemyCount > enemiesLeft)
            enemyCount = Random.Range(1, enemiesLeft);
        else
            enemyCount = Random.Range(wave[currentWaveCount].minEnemyCount, wave[currentWaveCount].maxEnemyCount);

        currentEnemyCount += enemyCount;

        for (int i = 0; i < enemyCount; i++)
        {
            //Use the object pool manager to get an enemy from the list
            string enemyID = GetRandomID(enemies);
            GameObject enemy = ObjectPoolManager.Instance.GetPooledObject(enemyID);
           
            if (enemy != null)
            {
                PhotonNetwork.InstantiateRoomObject(enemyID, parent.position, Quaternion.identity);
                SetEnemyDifficulty(enemy);
                PlayerController[] players = FindObjectsOfType<PlayerController>();
                PlayerController player = players[Random.Range(0, players.Length - 1)];

                if(player.IsPlayerDown)
                {
                    foreach(PlayerController p in players)
                    {
                        if (p.IsPlayerDown) continue;

                        player = p;
                        break;
                    }
                }

                enemy.GetComponent<EnemyController>().SetTarget(player.PhotonView.ViewID);
            }
            else
                i--;
        }

        photonView.RPC("RPCSyncEnemySpawner", RpcTarget.Others, resumeTime, currentEnemyCount);
    }

    private void SetEnemyDifficulty(GameObject enemy)
    {
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        float waveEnemySpeed = enemyController.Speed * wave[currentWaveCount].speedMultiplier;
        enemyController.SetEnemySpeed(waveEnemySpeed);

        int waveEnemyHealth = enemyController.MaxHealth * wave[currentWaveCount].healthMultiplier;
        enemyController.SetEnemyMaxHealth(waveEnemyHealth);

        int waveEnemyDamage = enemyController.Damage * wave[currentWaveCount].damageMultiplier;
        enemyController.SetEnemyDamage(waveEnemyDamage);

        int waveEnemyPoint = enemyController.MoneyDrop * wave[currentWaveCount].pointMultiplier;
        enemyController.SetMoneyDrop(waveEnemyPoint);
    }

    [PunRPC]
    private void RPCNextWave()
    {
        GameController.Instance.GameStats.WavesSurvived += 1;

        currentEnemyCount = 0;
        if (PhotonNetwork.IsMasterClient)
        {
            GameController.Instance.DespawnGameObjectsWithTag("Chest");
            GameController.Instance.DespawnGameObjectsWithTag("PowerUp");
        }

        if (CurrentWaveCount < wave.Length - 1)
        {
            CurrentWaveCount++;

            if(PhotonNetwork.IsMasterClient)
                InvokeRepeating("SpawnEnemies", timeBetweenWaves, wave[CurrentWaveCount].spawnTime);

            resumeTime = timeBetweenWaves;

            SetDifficultyString();
            GameController.Instance.ChestSpawner.SpawnChests(CurrentWaveCount);

            GameController.Instance.GameStats.CurrentEnemyCount = wave[CurrentWaveCount].totalEnemies;      
            
        }
        else
        {
            CancelInvoke();
            GameController.Instance.GameOver();
        }
    }

    [PunRPC]
    private void RPCSyncEnemySpawner(float resumeTime, int currentEnemyCount)
    {
        this.resumeTime = resumeTime;
        this.currentEnemyCount = currentEnemyCount;
    }

    private void SetDifficultyString()
    {
        if (currentWaveCount + 1 < 2)
            Difficulty = "Easy";
        else if (currentWaveCount + 1 < 4)
            Difficulty = "Medium";
        else
            Difficulty = "Hard";
    }
}
