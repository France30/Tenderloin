using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{ 
    [SerializeField] private List<string> enemies;
    [SerializeField] private List<Transform> spawnPoints;

    [SerializeField] private float timeBetweenWaves;
    [SerializeField] private Wave[] wave;

    private int currentEnemyCount;
    private int currentWaveCount = 0;

    private float resumeTime;

    private void Start()
    {
        resumeTime = timeBetweenWaves;
        InvokeRepeating("SpawnEnemies", timeBetweenWaves, wave[0].spawnTime);

        GameController.Instance.EnemySpawner = this;
        GameController.Instance.CurrentEnemyCount = wave[0].totalEnemies;
    }

    private void Update()
    {
        if (resumeTime > 0)
            resumeTime -= Time.deltaTime;

        bool isEnemyAlive = GameObject.FindGameObjectWithTag("Enemy") != null;
        if (!IsInvoking("SpawnEnemies") && !isEnemyAlive)
            NextWave();
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
        Debug.Log("current enemy count: " + currentEnemyCount);

        //Debug.Log("spawning " + enemyCount + " in the " + parent.name);

        for (int i = 0; i < enemyCount; i++)
        {
            //Use the object pool manager to get an enemy from the list
            GameObject pooledEnemy = ObjectPoolManager.Instance.GetPooledObject(GetRandomEnemyID());
            //Debug.Log(pooledEnemy);
            //Check first if we received a valid gameobject from the pool
            if (pooledEnemy != null)
            {
                pooledEnemy.transform.parent = parent;
                pooledEnemy.transform.localPosition = Vector3.zero;
                //Activate it to use the object
                pooledEnemy.SetActive(true);

                SetEnemyDifficulty(pooledEnemy);
            }
            else
                i--;
        }
    }

    private Transform GetRandomSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Count)];
    }

    private string GetRandomEnemyID()
    {
        return enemies[Random.Range(0, enemies.Count)];
    }

    private void SetEnemyDifficulty(GameObject enemy)
    {
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        float waveEnemySpeed = enemyController.Speed * wave[currentWaveCount].speedMultiplier;
        enemyController.Speed = waveEnemySpeed;

        int waveEnemyPoint = enemyController.MoneyDrop * wave[currentWaveCount].pointMultiplier;
        enemyController.MoneyDrop = waveEnemyPoint;
    }

    private void NextWave()
    {
        currentEnemyCount = 0;

        if (currentWaveCount < wave.Length - 1)
        {
            currentWaveCount++;
            InvokeRepeating("SpawnEnemies", timeBetweenWaves, wave[currentWaveCount].spawnTime);
            resumeTime = timeBetweenWaves;

            GameController.Instance.GameScreenUI.UpdateWaveCount((currentWaveCount + 1).ToString());

            GameController.Instance.CurrentEnemyCount = wave[currentWaveCount].totalEnemies;
        }
        else
        {
            CancelInvoke();
            GameController.Instance.GameOver();
        }
    }
}
