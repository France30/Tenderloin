using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private const int INVENTORY_SIZE = 10;
    private List<Sprite> inventory = new List<Sprite>();

    public bool CheckIfInventoryFull()
    {
        if (inventory.Count == INVENTORY_SIZE) return true;

        return false;
    }
    public void AddItem(GameObject item)
    {
        Sprite itemIcon = item.GetComponent<PowerUp>().GetItemSprite();
        inventory.Add(itemIcon);

        //Debug.Log("Adding to inventory");

        GameScreenUI gameScreenUI = GameController.Instance.GameScreenUI;
        UpdateInventoryIcon(gameScreenUI.InventoryIcon);
    }

    public void UpdateInventoryIcon(Image[] inventoryIcon)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryIcon[i].sprite = inventory[i];
            inventoryIcon[i].enabled = true;
        }
    }
}
