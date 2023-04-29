using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Photon.Pun;

public class RaycastController : MonoBehaviour
{
    [SerializeField] private float raycastDistance = 5.0f;
    [SerializeField] private LayerMask layerMask;

    private GameObject interactable;
    private PlayerInteraction playerInteraction;
    private PhotonView photonView;

    private void OnEnable()
    {
        photonView = GetComponentInParent<PhotonView>();
        playerInteraction = gameObject.AddComponent<PlayerInteraction>();
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        if (Input.GetKeyDown("e"))
        {
            if (interactable == null) return;

            playerInteraction.PlayerTryInteract(interactable);
        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance, layerMask))
        {            
            try
            {
                interactable = hit.collider.gameObject;

                GameScreenUI gameScreenUI = GameController.Instance.GameScreenUI;
                string interactableInfo = interactable.GetComponent<Interactable>().GetInteractableInfo();
                gameScreenUI.UpdateInteractablePopUp(interactableInfo);
            }
            catch (NullReferenceException e)
            {
                //do nothing
            }

        }
        else
        {           
            try
            {
                GameScreenUI gameScreenUI = GameController.Instance.GameScreenUI;
                gameScreenUI.UpdateInteractablePopUp("");
                interactable = null;
            }
            catch (NullReferenceException e) 
            {
                //do nothing
            }
        }
    }
}
