using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChestSpawner : Spawner
{
    [SerializeField] private string chestID;
    [SerializeField] private ChestWave[] wave;

    public IEnumerator SpawnChestsOnStart()
    {
        yield return new WaitForSeconds(0.5f);

        SpawnChests(0);
    }

    public void SpawnChests(int currentWave)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        int chestCount = Random.Range(wave[currentWave].minChestCount, wave[currentWave].maxChestCount);

        for (int i = 0; i < chestCount; i++)
        {
            Transform parent = GetRandomSpawnPoint();

            GameObject chest = ObjectPoolManager.Instance.GetPooledObject(chestID);
            PhotonNetwork.InstantiateRoomObject(chestID, parent.position, chest.transform.rotation);
            chest.GetComponent<Chest>().photonView.RPC("RPCSetParent", RpcTarget.AllBuffered, parent.name);
            int cost = Random.Range(wave[currentWave].minChestCost, wave[currentWave].maxChestCost);
            chest.GetComponent<Chest>().SetCostToOpen(cost);
        }    
    }

    public override Transform GetRandomSpawnPoint()
    {
        Transform spawnPoint = base.GetRandomSpawnPoint();
        while (!CheckIfSpawnPointEmpty(spawnPoint))
            spawnPoint = base.GetRandomSpawnPoint();

        return spawnPoint;
    }

    private bool CheckIfSpawnPointEmpty(Transform spawnPoint)
    {
        if (spawnPoint.childCount == 0) return true;
        return false;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        GameController.Instance.ChestSpawner = this;

        if (!PhotonNetwork.IsMasterClient) return;
        StartCoroutine(SpawnChestsOnStart());
    }
}
