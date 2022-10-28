using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    //unique id
    public string id;
    //prefab of the object we want to make copies of
    public GameObject objectToPool;
    //parent transform to attach the object once it is instantiated
    public Transform parent;
    //how many objects will be instantiated to the pool at the beginning
    public int amountToPool;
    //if we reached the maximum amount to pool, should we intantiate a new prefab instance?
    public bool shouldExpand;
}
