using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameSync : SingletonPun<GameSync>
{
    private PhotonView photonView;

    // Start is called before the first frame update
    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void SyncNewPlayerJoined(int currentEnemyCount, int currentWaveCount, string currentDifficulty, int spawnerEnemyCount)
    {
        photonView.RPC("RPCSyncNewPlayerJoined", RpcTarget.OthersBuffered, currentEnemyCount, currentWaveCount, currentDifficulty, spawnerEnemyCount);
    }

    [PunRPC]
    private void RPCSyncNewPlayerJoined(int currentEnemyCount, int currentWaveCount, string currentDifficulty, int spawnerEnemyCount)
    {
        GameController.Instance.GameStats.CurrentEnemyCount = currentEnemyCount;
        GameController.Instance.EnemySpawner.CurrentWaveCount = currentWaveCount;
        GameController.Instance.EnemySpawner.Difficulty = currentDifficulty;
        GameController.Instance.EnemySpawner.CurrentEnemyCount = spawnerEnemyCount;
    }
}
