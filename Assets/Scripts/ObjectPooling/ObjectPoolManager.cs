using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    public List<ObjectPoolItem> itemsToPool;
    //pool of objects
    [SerializeField]
    private List<GameObject> pooledObjects;

    private void Start()
    {
        //traverse through each objectpoolitem in the list
        foreach(ObjectPoolItem item in itemsToPool)
        {
            //instantiate the object's prefab based on the initial amount to Pool
            for(int i = 0; i < item.amountToPool; i++)
            {
                //Instantiate the prefab and set its parent
                GameObject obj = Instantiate(item.objectToPool, item.parent);
                //Add the registered id to that object
                obj.AddComponent<PooledObjectItem>().id = item.id;
                //Make sure the object is disabled
                obj.SetActive(false);
                //We add it to the pool
                pooledObjects.Add(obj);
            }
        }
    }

    public GameObject GetPooledObject(string id)
    {
        //check each object from the pool
        for(int i = 0; i < pooledObjects.Count; i++)
        {
            //check if that object is currently inactive (not being used)
            //and that object has the same id as what we're looking for
            if(!pooledObjects[i].activeInHierarchy && 
                pooledObjects[i].GetComponent<PooledObjectItem>().id == id)
            {
                //return that object for use
                return pooledObjects[i];
            }
        }

        //if all objects are currently in use
        //check if the object can expand and then instantiate a new object and add it to the pool
        foreach(ObjectPoolItem item in itemsToPool)
        {
            if(item.id == id)
            {
                if (item.shouldExpand)
                {
                    //Instantiate the prefab and set its parent
                    GameObject obj = Instantiate(item.objectToPool, item.parent);
                    //Add the registered id to that object
                    obj.AddComponent<PooledObjectItem>().id = item.id;
                    //Make sure the object is disabled
                    obj.SetActive(false);
                    //We add it to the pool
                    pooledObjects.Add(obj);
                    return obj;
                }
            }
        }

        return null;
    }

    public void DespawnGameObject(GameObject obj)
    {
        //verify if the object is part of the pooledobjects
        if (pooledObjects.Contains(obj))
        {
            //Deactivate the gameobject so that the ObjectPoolManager can reuse it
            obj.SetActive(false);
            //put it back to the original parent
            //get the id of the object
            string id = obj.GetComponent<PooledObjectItem>().id;
            ObjectPoolItem item = itemsToPool.Find(i => i.id == id);
            //itemsToPool.Find(i => i.id == id) is a shortcut for this:
            /*
            for(int i = 0; i < itemsToPool.Count; i++)
            {
                if (itemsToPool[i].id == id)
                    return itemsToPool[i];
            }*/
            
            //put back the despawned object to the original parent
            obj.transform.SetParent(item.parent);
        }
    }
}
