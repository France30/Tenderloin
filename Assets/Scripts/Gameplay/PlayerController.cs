using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float hurtTimer;
    [SerializeField] private float hurtCooldownSpeed = 0.5f;

    private float timer;
    private float cooldown;
    private int initialHealth;
    
    public int Health { get; private set; }
    public bool IsHurt { get; private set; }

    public void TakeDamage(int amount)
    {
        if (Health > 0)
        {
            AudioManager.Instance.Play("PlayerHit");

            //Health -= amount;
            if (Health < 0) Health = 0;

            for (int i = initialHealth - 1 ; i > Health - 1; i--)
                GameController.Instance.GameScreenUI.DisableHealthBar(i);

            float alpha = GameController.Instance.GameScreenUI.DamageScreenColor.a;
            GameController.Instance.GameScreenUI.DamageUI(alpha);

            timer = hurtTimer;
            IsHurt = true;

            if (Health <= 0)
                GameController.Instance.GameOver();
        }
    }

    private void Awake()
    {
        timer = hurtTimer;
        Health = 3;
        IsHurt = false;
        initialHealth = Health;
    }

    private void Update()
    {
        if(IsHurt)
            HurtTimer();

        if(timer < 0f)
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
