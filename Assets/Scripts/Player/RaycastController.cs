using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RaycastController : MonoBehaviour
{
    [SerializeField] private float raycastDistance = 5.0f;
    [SerializeField] private LayerMask layerMask;

    private GameObject interactable;
    private PlayerInteraction playerInteraction;

    private void OnEnable()
    {
        playerInteraction = gameObject.AddComponent<PlayerInteraction>();
    }

    private void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            if (interactable == null) return;

            playerInteraction.PlayerTryInteract(interactable);
        }
    }

    private void FixedUpdate()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance, layerMask))
        {            
            try
            {
                if (interactable != null) return;

                interactable = hit.collider.gameObject;

                GameScreenUI gameScreenUI = GameController.Instance.GameScreenUI;
                gameScreenUI.InteractablePopUp.text = interactable.GetComponent<Interactable>().GetInteractableInfo();
                gameScreenUI.InteractablePopUp.enabled = true;
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
                if (!gameScreenUI.InteractablePopUp.enabled) return;

                gameScreenUI.InteractablePopUp.enabled = false;
                interactable = null;
            }
            catch (NullReferenceException e) 
            {
                //do nothing
            }
        }
    }
}
