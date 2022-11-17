using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : Interactable
{
    public enum BoostEffect { DoubleDamage, IncreaseMovementSpeed, IncreaseFireRate}

    [SerializeField] private BoostEffect boostType;
    [SerializeField] private float multiplier;

    [SerializeField] private Sprite icon;

    public override string GetInteractableInfo()
    {
        return interactableInfo;
    }

    public override void Interact(PlayerController player)
    {
        float stat = GetStatToBoost(player);

        stat *= multiplier;
        SetStatToBoost(player, stat);

        player.TryGetComponent<Inventory>(out Inventory inventory);
        inventory.AddItem(this.gameObject);

        this.gameObject.SetActive(false);
    }

    public Sprite GetItemSprite()
    {
        return icon;
    }

    private float GetStatToBoost(PlayerController player)
    {
        float value = 0;
        switch (boostType)
        {
            case BoostEffect.DoubleDamage:
                //value = player.BulletDmg;
                break;

            case BoostEffect.IncreaseMovementSpeed:
                //value = player.MoveSpeed;
                break;

            case BoostEffect.IncreaseFireRate:
                //value = player.FireRate;
                break;
        }

        return value;
    }

    private void SetStatToBoost(PlayerController player, float value)
    {
        switch (boostType)
        {
            case BoostEffect.DoubleDamage:
                //player.BulletDmg = value;
                break;

            case BoostEffect.IncreaseMovementSpeed:
                //player.MoveSpeed = value;
                break;

            case BoostEffect.IncreaseFireRate:
                //player.FireRate = value;
                break;
        }
    }
}
