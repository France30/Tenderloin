using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PowerUp : Interactable
{
    [SerializeField] private Stat[] stats;

    public override string GetInteractableInfo()
    {
        return interactableInfo;
    }

    public override void Interact(PlayerController player)
    {
        if (!isInteractable) return;

        photonView.RPC("RPCSyncOnInteract", RpcTarget.All);

        foreach (Stat stat in stats)
            SetStatValue(player, stat.statType, stat.value);

        GameController.Instance.GameStats.Inventory.AddItem(this.gameObject);
        AudioManager.Instance.Play("ActionPickUp");

        NetworkDestroy();
    }

    public Sprite GetItemSprite()
    {
        Sprite sprite = gameObject.GetComponentInChildren<SpriteRenderer>().sprite;
        return sprite;
    }

    private void SetStatValue(PlayerController player, Stat.StatType statType, float value)
    {
        switch (statType)
        {
            case Stat.StatType.Damage:
                player.PlayerGun.Damage = player.PlayerGun.Damage + (int)value;
                break;

            case Stat.StatType.Ammo:
                player.PlayerGun.MaxAmmo = player.PlayerGun.MaxAmmo + (int) value;
                break;

            case Stat.StatType.FireRate:
                player.PlayerGun.FireRate = player.PlayerGun.FireRate + value;
                break;

            case Stat.StatType.Health:
                if(value > 0)
                    player.CurrentHealth = player.MaxHealth + (int)value;
                else
                    player.CurrentHealth = player.CurrentHealth + (int)value;

                player.MaxHealth = player.MaxHealth + (int)value; 
                break;

            case Stat.StatType.Movement:
                player.PlayerMovement.MoveSpeed = player.PlayerMovement.MoveSpeed + value;
                player.PlayerMovement.SprintSpeed = player.PlayerMovement.SprintSpeed + value;
                break;

            case Stat.StatType.DamageResist:
                player.DamageResist = player.DamageResist + (int)value;
                break;

            default:
                break;
        }
    }
}
