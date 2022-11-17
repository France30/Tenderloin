using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public void PlayerTryInteract(GameObject interactable)
    {
        if (interactable.TryGetComponent<PowerUp>(out PowerUp powerUp))
            PowerUpTryInteract(powerUp);

        if (interactable.TryGetComponent<Chest>(out Chest chest))
            ChestTryInteract(chest);
    }

    private void PowerUpTryInteract(PowerUp powerUp)
    {
        PlayerController player = gameObject.GetComponentInParent<PlayerController>();

        player.TryGetComponent<Inventory>(out Inventory inventory);
        if (inventory.CheckIfInventoryFull())
        {
            InteractFailed("Inventory is Full");
            return;
        }

        powerUp.Interact(player);
    }

    private void ChestTryInteract(Chest chest)
    {
        //do not interact if player doesn't have enough money
        if (GameController.Instance.CurrentMoney < chest.CostToOpen)
        {
            InteractFailed("Not Enough Money");
            return;
        }

        chest.Interact();
        GameController.Instance.CurrentMoney -= chest.CostToOpen;
    }

    private void InteractFailed(string failMessage = "")
    {
        Debug.Log(failMessage); //placeholder, will replace with something else
    }
}
