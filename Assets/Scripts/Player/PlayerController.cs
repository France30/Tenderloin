using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int health = 100;

    private int initialHealth;

    private PlayerDamage playerDamage;

    public int Health { get { return health; } private set { health = value; } }

    public void TakeDamage(int amount)
    {
        if (Health > 0)
        {
            //Health -= amount;
            if (Health < 0) Health = 0;

            //for (int i = initialHealth - 1 ; i > Health - 1; i--)
            //GameController.Instance.GameScreenUI.DisableHealthBar(i);

            playerDamage.DamageEffect();

            if (Health <= 0)
                GameController.Instance.GameOver();
        }
    }

    private void Awake()
    {
        initialHealth = Health;
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
