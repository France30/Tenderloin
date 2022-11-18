using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] private float hurtTimer = 1f;
    [SerializeField] private float hurtCooldownSpeed = 0.5f;

    private float timer;
    private float cooldown;

    public bool IsHurt { get; private set; }

    public void DamageEffect()
    {
        AudioManager.Instance.Play("PlayerHit");

        float alpha = GameController.Instance.GameScreenUI.DamageScreenColor.a;
        GameController.Instance.GameScreenUI.DamageUI(alpha);

        timer = hurtTimer;
        IsHurt = true;
    }

    private void Awake()
    {
        timer = hurtTimer;       
        IsHurt = false;
    }

    private void Update()
    {
        if (IsHurt)
            HurtTimer();

        if (timer < 0f)
            DamageCooldown();
    }

    private void HurtTimer()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
        else
        {
            cooldown = GameController.Instance.GameScreenUI.DamageScreenColor.a;
            IsHurt = false;
        }
    }

    private void DamageCooldown()
    {
        GameController.Instance.GameScreenUI.DamageUI(cooldown);
        cooldown -= hurtCooldownSpeed * Time.deltaTime;
    }
}
