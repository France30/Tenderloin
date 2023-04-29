using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    //unique identifier
    public string id;
    //prefab
    public GameObject objectToPool;
    //parent transform to attach the gameobject once instantiate
    public Transform parent;
    //how many objects will be initially instantiated
    public int amountToPool;
    //if we reach the maximum amount to pool, check if we need to instantiate a new prefab instane
    public bool shouldExpand;
}

