using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float maxHealth = 1f;
    [SerializeField] private int enemyScore = 1;
    [SerializeField] private int damage = 1;
    [SerializeField] private GameObject target;

    private NavMeshAgent enemyAgent;

    private EnemyAnimationController animationController;

    private float currentHealth;
    private float initialSpeed;
    private int initialScore;

    private bool isAgentResetting = false;

    public float Speed { get { return enemyAgent.speed; } set { enemyAgent.speed = value;} }
    public int EnemyScore { get { return enemyScore;  } set { enemyScore = value;  } }

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
            //enable physics when shot
            SetRigidbodyIsKinematic(false);
        }
        else
            animationController.Play("Death");
    }

    public void Attack()
    {
        GameController.Instance.Player.TakeDamage(damage);
    }

    public void OnDeath()
    {
        SetRigidbodyIsKinematic(true);
        GameController.Instance.AddScore(enemyScore);
        ObjectPoolManager.Instance.DespawnGameObject(gameObject);
    }

    private void Awake()
    {
        animationController = GetComponent<EnemyAnimationController>();
        enemyAgent = GetComponent<NavMeshAgent>();

        initialSpeed = this.Speed;
        initialScore = this.EnemyScore;
    }

    private void OnEnable()
    {
        bool isGameScene = GameController.Instance.IsGameScene;
        if (!isGameScene) return;

        currentHealth = maxHealth;
        this.EnemyScore = initialScore;

        SetRigidbodyIsKinematic(false);

        if(!enemyAgent.enabled)
            enemyAgent.enabled = true;

        enemyAgent.SetDestination(target.transform.position);
        this.Speed = initialSpeed;
    }

    // Update is called once per frame
    private void Update()
    {
        LookAtTarget();
        if (!enemyAgent.hasPath) return;

        if (enemyAgent.remainingDistance > enemyAgent.stoppingDistance)
        {
            //reset path to update target location
            if (!isAgentResetting)
            {
                StartCoroutine(ResetEnemy());
            }

            animationController.Play("Chase");
            animationController.Stop("Attack"); // in case enemy was in the attack state previously
            return;
        }

        if (enemyAgent.remainingDistance <= enemyAgent.stoppingDistance)
        {
            //disable kinematic when destination has been reached
            SetRigidbodyIsKinematic(false);

            if (enemyAgent.velocity.sqrMagnitude <= 0.1f)
            {
                animationController.Play("Attack");
            }
        }
    }

    private IEnumerator ResetEnemy()
    {        
        isAgentResetting = true;

        enemyAgent.ResetPath();
        enemyAgent.SetDestination(target.transform.position);

        yield return new WaitForSeconds(0.5f);

        SetRigidbodyIsKinematic(true);

        isAgentResetting = false;
    }

    private void LookAtTarget()
    {
        Vector3 targetPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        transform.LookAt(targetPosition);
    }

    private void SetRigidbodyIsKinematic(bool state)
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = state;
    }
}
