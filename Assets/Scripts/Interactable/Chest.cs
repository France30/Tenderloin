using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Chest : Interactable
{
    [SerializeField] private int costToOpen;
    [SerializeField] private TextMeshProUGUI costPopUp;

    public int CostToOpen { get { return costToOpen; } }
    public override string GetInteractableInfo()
    {
        return interactableInfo;
    }

    public override void Interact()
    {
        //check if player has enough money
        //spawn power-up if player has money
        //disable object

        GameObject powerUp = ObjectPoolManager.Instance.GetPooledObject("PowerUp");
        //Debug.Log(pooledEnemy);
        //Check first if we received a valid gameobject from the pool
        if (powerUp != null)
        {
            powerUp.transform.position = transform.position;
            //Activate it to use the object
            powerUp.SetActive(true);
        }

        Destroy(gameObject); //placeholder
    }

    private void OnEnable()
    {
        costPopUp.text = "$" + costToOpen;
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
