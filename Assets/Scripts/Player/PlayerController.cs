using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;

    private float currentHealth;
    private bool isHealthUISet = false;
    private bool isPlayerFiring = false;

    private PlayerDamage playerDamage;
    private GameScreenUI gameScreenUI;

    public float CurrentHealth { get { return currentHealth; } }
    public bool IsPlayerFiring { get { return isPlayerFiring; } }

    public void TakeDamage(int amount)
    {
        if (currentHealth > 0)
        {
            currentHealth -= amount;
            //Debug.Log(currentHealth);
            if (currentHealth < 0) currentHealth = 0;

            GameController.Instance.GameScreenUI.UpdateHealthBar(currentHealth, maxHealth);
            playerDamage.DamageEffect();

            //if (currentHealth <= 0)
                //GameController.Instance.GameOver();
        }
    }

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        playerDamage = gameObject.GetComponent<PlayerDamage>();
        gameObject.AddComponent<Inventory>();
    }

    private void OnEnable()
    {
        GameController.Instance.Player = this;
    }

    private void Update()
    {
        if (!isHealthUISet)
        {
            InitializeHealthUI();
            return;
        }

        CheckIfPlayerIsFiring();
    }

    private void InitializeHealthUI()
    {
        try
        {
            gameScreenUI = GameController.Instance.GameScreenUI;
            gameScreenUI.UpdateHealthBar(currentHealth, maxHealth);

            isHealthUISet = true;
        }
        catch (NullReferenceException e)
        {
            //do nothing
        }
    }

    private void CheckIfPlayerIsFiring()
    {
        if (Input.GetButton("Fire1"))
            isPlayerFiring = true;
        else
            isPlayerFiring = false;
    }
}
