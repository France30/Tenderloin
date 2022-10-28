using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                //Try to find an existing type of object in the scene
                instance = FindObjectOfType<T>();
                //If we did not find that object in the scene
                if(instance == null)
                {
                    //Create a new gameobject of that type 
                    GameObject obj = new GameObject();
                    //and rename it according to its type
                    obj.name = typeof(T).Name;
                    //and add that type as a component and assign it as our instance
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    public virtual void Awake()
    {
        //if there is no instance yet,
        if(instance == null)
        {
            //assign ourselves as the instance
            instance = this as T;
            //prevent this object from being destroyed
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //if there is already an instance
            Destroy(this.gameObject);
        }

    }
}
