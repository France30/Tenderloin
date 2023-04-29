using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PowerUpSpawner : Spawner
{
    [SerializeField] private PowerUpSet[] powerUpSet;

    public void SpawnPowerUp(Transform transform)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        string difficulty = GameController.Instance.EnemySpawner.Difficulty;

        foreach(PowerUpSet powerUp in powerUpSet)
        {
            if (powerUp.difficultySet != difficulty) continue;

            string powerUpID = GetRandomID(powerUp.powerUpID);
            GameObject powerUpObj = ObjectPoolManager.Instance.GetPooledObject(powerUpID);

            while (powerUpObj == null) 
            {
                powerUpID = GetRandomID(powerUp.powerUpID);
                powerUpObj = ObjectPoolManager.Instance.GetPooledObject(powerUpID);
            }
            
            PhotonNetwork.InstantiateRoomObject(powerUpID, transform.position, Quaternion.identity);
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        GameController.Instance.PowerUpSpawner = this;
    }
}
