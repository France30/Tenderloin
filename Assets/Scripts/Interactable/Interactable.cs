using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class Interactable : MonoBehaviourPunCallbacks
{
    [SerializeField] protected string interactableInfo;

    protected PhotonView photonView;
    protected bool isInteractable;

    public override void OnEnable()
    {
        isInteractable = true;
        base.OnEnable();
        photonView = GetComponent<PhotonView>();
    }

    public virtual void NetworkDestroy()
    {
        photonView.RPC("RPCNetworkDestroy", RpcTarget.MasterClient);
    }

    [PunRPC]
    public void RPCNetworkDestroy()
    {
        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    public virtual void RPCSyncOnInteract()
    {
        isInteractable = false;
    }

    public abstract string GetInteractableInfo();
    public virtual void Interact() { }
    public virtual void Interact(PlayerController player) { }
}
