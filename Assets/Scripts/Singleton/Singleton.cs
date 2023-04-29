using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T:MonoBehaviour
{
    //Enable this if you want the object to persist in between scenes
    [SerializeField]
    private bool isPresist;

    private static T _instance;

    public static T Instance
    {
        get
        {
            //if there is no instance yet,
            if (!_instance)
            {
                //find an exisiting gameobject in the scene that has the generic component
                _instance = (T)FindObjectOfType(typeof(T));
            }
            //if after checking the scene we still don't have an instance...
            if (!_instance)
            {
                //load a gameobject from the resources folder that has the generic component
                //check if the System folder in the Resources folder has a prefab of the generic component
                if(Resources.Load<T>("System/" + typeof(T).Name) != null)
                {
                    //if the prefab is found, instantiate that prefab
                    T instance = Resources.Load<T>("System/" + typeof(T).Name);
                    //Instantiate the component
                    T go = (T)Instantiate(instance);
                    //set our instance to the component that we have just instantiated
                    _instance = go;
                }
                //if we didnt find any prefab, then our instance is null
                else
                {
                    _instance = null;
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        //if there is no instance, set self as the instance
        if (_instance == null)
            _instance = this as T;

        //if there is already an existing instance
        if(_instance != null)
        {
            //check if self is not the actual instance
            if(_instance != this as T)
            {
                //Destroy self because there can only be one instance - SINGLETON DESIGN PATTERN
                Destroy(this.gameObject);
            }
        }

        if (isPresist)
            DontDestroyOnLoad(this.gameObject);
    }
}
