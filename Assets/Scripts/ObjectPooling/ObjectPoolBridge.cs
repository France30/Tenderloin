using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ObjectPoolBridge : MonoBehaviour, IPunPrefabPool
{
    public void Destroy(GameObject gameObject)
    {
        ObjectPoolManager.Instance.ReturnToPool(gameObject);
    }

    public new GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
    {
        //Whenever Photon instantiates an object, we simply get an object from our ObjectPoolManager
        if (ObjectPoolManager.Instance == null) return null;

        GameObject go = ObjectPoolManager.Instance.GetPooledObject(prefabId);

        if (go == null) return null;

        go.transform.position = position;
        go.transform.rotation = rotation;
        //Return the object
        return go;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Set our prefabpool to this
        PhotonNetwork.PrefabPool = this;
    }

}
