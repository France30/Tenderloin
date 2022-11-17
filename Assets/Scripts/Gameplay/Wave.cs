using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public int minEnemyCount, maxEnemyCount;
    public float spawnTime;
    public int totalEnemies;
    [Range(1f, 2f)]
    public float speedMultiplier;
    [Range(1, 10)]
    public int pointMultiplier;
}
