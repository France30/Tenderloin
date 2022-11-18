using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;

    private float currentHealth;

    private PlayerDamage playerDamage;

    public float CurrentHealth { get { return currentHealth; } }

    public void TakeDamage(int amount)
    {
        if (currentHealth > 0)
        {
            currentHealth -= amount;
            Debug.Log(currentHealth);
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

    }
}
