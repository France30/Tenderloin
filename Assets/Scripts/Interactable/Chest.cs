using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class Chest : Interactable
{
    [SerializeField] private int costToOpen;
    [SerializeField] private TextMeshProUGUI costPopUp;

    private Transform poolParent;
    private bool isPoolParentSet = false;

    public int CostToOpen { get { return costToOpen; } set { costToOpen = value; } }

    public void SetCostToOpen(int cost)
    {
        photonView.RPC("RPCSetCostToOpen", RpcTarget.All, cost);
        photonView.RPC("RPCSetCostToOpen", RpcTarget.OthersBuffered, cost);
    }

    [PunRPC]
    private void RPCSetCostToOpen(int cost)
    {
        CostToOpen = cost;
        costPopUp.text = "$" + costToOpen;
    }

    public override string GetInteractableInfo()
    {
        return interactableInfo + " ($" + CostToOpen + ")";
    }

    public override void Interact()
    {
        if (!isInteractable) return;
        
        GameController.Instance.GameStats.CurrentMoney -= CostToOpen;

        photonView.RPC("RPCSyncOnInteract", RpcTarget.All);
        photonView.RPC("RPCSpawnPowerUp", RpcTarget.MasterClient);

        NetworkDestroy();
    }
    
    [PunRPC]
    public override void RPCSyncOnInteract()
    {
        base.RPCSyncOnInteract();
        costPopUp.enabled = false;
    }

    [PunRPC]
    private void RPCSpawnPowerUp()
    {
        GameController.Instance.PowerUpSpawner.SpawnPowerUp(this.transform);
    }

    public override void NetworkDestroy()
    {
        photonView.RPC("RPCSetParent", RpcTarget.AllBuffered, poolParent.name);
        base.NetworkDestroy();
    }

    [PunRPC]
    private void RPCSetParent(string parentName)
    {
        Transform parent = GameObject.Find(parentName).transform;
        transform.SetParent(parent);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        if (isPoolParentSet) return;

        isPoolParentSet = true;
        poolParent = transform.parent;

        costPopUp.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<PlayerController>())
        {
            costPopUp.enabled = true;           
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            if (!costPopUp.enabled) return;

            costPopUp.enabled = false;
        }
    }
}
