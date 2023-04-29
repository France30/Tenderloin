using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObjectItem : MonoBehaviour
{
    [SerializeField]
    private string _id;

    public string ID
    {
        get { return _id; }
        set { _id = value; }
        //get => _id;
        //set => _id = value;
    }
}

