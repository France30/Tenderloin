using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float maxHealth = 1f;
    [SerializeField] private int moneyDrop = 1;
    [SerializeField] private int damage = 1;
    [SerializeField] private GameObject target;

    private NavMeshAgent enemyAgent;

    private EnemyAnimationController animationController;

    private float currentHealth;
    private float initialSpeed;
    private int initialMoneyDrop;

    private bool isDeath = false;

    public float Speed { get { return enemyAgent.speed; } set { enemyAgent.speed = value;} }
    public int MoneyDrop { get { return moneyDrop;  } set { moneyDrop = value;  } }

    public void PauseMove()
    {
        if (enemyAgent.hasPath)
        {
            enemyAgent.ResetPath();
        }
        else
        {
            if (currentHealth > 0)
                enemyAgent.SetDestination(target.transform.position);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth > 0)
        {
            animationController.Play("Hit");
        }
        else if(currentHealth <= 0 && !isDeath)
            animationController.Play("Death");
    }

    public void Attack()
    {
        GameController.Instance.Player.TakeDamage(damage);
    }

    public void OnDeath() //function should only be called once when the enemy dies
    {
        isDeath = true; //this bool prevents the function from being called multiple times

        GameController.Instance.CurrentMoney += MoneyDrop;
        GameController.Instance.CurrentEnemyCount -= 1;
        ObjectPoolManager.Instance.DespawnGameObject(gameObject);
    }

    private void Awake()
    {
        animationController = GetComponent<EnemyAnimationController>();
        enemyAgent = GetComponent<NavMeshAgent>();

        initialSpeed = this.Speed;
        initialMoneyDrop = this.moneyDrop;
    }

    private void OnEnable()
    {
        bool isGameScene = GameController.Instance.IsGameScene;
        if (!isGameScene) return;

        isDeath = false;
        currentHealth = maxHealth;
        this.MoneyDrop = initialMoneyDrop;

        if(!enemyAgent.enabled)
            enemyAgent.enabled = true;

        this.target = FindObjectOfType<PlayerController>().gameObject;
        enemyAgent.SetDestination(target.transform.position);
        this.Speed = initialSpeed;
    }

    // Update is called once per frame
    private void Update()
    {
        LookAtTarget();
        if (!enemyAgent.hasPath) return;

        enemyAgent.SetDestination(target.transform.position);

        if (enemyAgent.remainingDistance > enemyAgent.stoppingDistance)
        {
            animationController.Play("Chase");
            animationController.Stop("Attack"); // in case enemy was in the attack state previously
            return;
        }

        if (enemyAgent.remainingDistance <= enemyAgent.stoppingDistance)
        {
            if (enemyAgent.velocity.sqrMagnitude <= 0.1f)
            {
                animationController.Play("Attack");
            }
        }
    }

    private void LookAtTarget()
    {
        Vector3 targetPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        transform.LookAt(targetPosition);
    }
}
