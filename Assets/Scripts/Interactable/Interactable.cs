using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] protected string interactableInfo;

    public abstract string GetInteractableInfo();
    public virtual void Interact() { }
    public virtual void Interact(PlayerController player) { }
}
