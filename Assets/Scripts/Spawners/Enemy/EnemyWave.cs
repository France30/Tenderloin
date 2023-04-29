using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyWave
{
    public int minEnemyCount, maxEnemyCount;
    public float spawnTime;
    public int totalEnemies;
    [Range(1f, 5f)]
    public int healthMultiplier;
    [Range(1f, 5f)]
    public int damageMultiplier;
    [Range(1f, 2f)]
    public float speedMultiplier;
    [Range(1, 10)]
    public int pointMultiplier;
}
