using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public abstract class Spawner : MonoBehaviourPunCallbacks
{
    [SerializeField] protected List<Transform> spawnPoints;

    public virtual Transform GetRandomSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Count)];
    }

    public virtual string GetRandomID(List<string> id)
    {
        return id[Random.Range(0, id.Count)];
    }
}
