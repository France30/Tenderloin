using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public int Damage { get; set; }
    public Collider Collider { get; set; }

    private void Awake()
    { 
        Collider = gameObject.GetComponent<Collider>();       
    }

    private void OnEnable()
    {
        Collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
            player.TakeDamage(Damage);
    }
}
