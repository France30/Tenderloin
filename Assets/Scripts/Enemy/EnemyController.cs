using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class EnemyController : MonoBehaviourPunCallbacks
{
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private int moneyDrop = 1;
    [SerializeField] private int damage = 1;
    [SerializeField] private GameObject target;
    [SerializeField] private Weapon weapon;

    private NavMeshAgent enemyAgent;

    private EnemyAnimationController animationController;

    private int currentHealth;
    private float initialSpeed;
    private int initialMoneyDrop;

    private bool isDeath = false;

    public float Speed { get { return enemyAgent.speed;} set { enemyAgent.speed = value; } }
    public int MoneyDrop { get { return moneyDrop;  } set { moneyDrop = value; } }
    public int Damage { get { return damage; } set { damage = value; } }
    public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
    public Weapon Weapon { get { return weapon; } }
    public GameObject Target { get { return target; } set { target = value; } }
    public PhotonView PhotonView { get; set; }

    public void SetMoneyDrop(int value)
    {
        PhotonView.RPC("RPCSetMoneyDrop", RpcTarget.AllBuffered, value);
    }

    public void SetEnemySpeed(float value)
    {
        PhotonView.RPC("RPCSetSpeed", RpcTarget.AllBuffered, value);
    }

    public void SetEnemyMaxHealth(int value)
    {
        PhotonView.RPC("RPCSetMaxHealth", RpcTarget.AllBuffered, value);
    }

    public void SetEnemyDamage(int value)
    {
        PhotonView.RPC("RPCSetDamage", RpcTarget.AllBuffered, value);
    }

    [PunRPC]
    private void RPCSetDamage(int value)
    {
        Damage = value;
    }

    [PunRPC]
    private void RPCSetMaxHealth(int value)
    {
        MaxHealth = value;
    }

    [PunRPC]
    private void RPCSetSpeed(float value)
    {
        enemyAgent.speed = value;
    }

    [PunRPC]
    private void RPCSetMoneyDrop(int value)
    {
        moneyDrop = value;
    }

    public void SetTarget(int targetID)
    {
        PhotonView.RPC("RPCSetTarget", RpcTarget.AllBuffered, targetID);
    }

    [PunRPC]
    private void RPCSetTarget(int targetID)
    {
        PhotonView targetView = PhotonView.Find(targetID);

        if (targetView != null)
            this.Target = targetView.gameObject;
        else
            this.Target = GameController.Instance.Player.gameObject;

        enemyAgent.SetDestination(Target.transform.position);
    } 

    public void PauseMove()
    {
        if (enemyAgent.hasPath)
        {
            enemyAgent.ResetPath();
        }
        else if(currentHealth > 0)
        {
            enemyAgent.SetDestination(Target.transform.position);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth > 0)
        {
            PhotonView.RPC("PlayAnimation", RpcTarget.All, "Hit");
        }
        else if (currentHealth <= 0)
        {
            PhotonView.RPC("PlayAnimation", RpcTarget.All, "Death");
        }
    }

    public void Attack()
    {
        weapon.Damage = damage;
        weapon.Collider.enabled = true;
    }

    public void OnDeath() //function should only be called once when the enemy dies
    {
        if (isDeath || !Target.GetPhotonView().IsMine) return;

        isDeath = true; //this bool prevents the function from being called multiple times

        RPCSyncDeath();
        PhotonView.RPC("RPCSyncDeath", RpcTarget.OthersBuffered);

        PhotonView.RPC("RPCNetworkDestroy", RpcTarget.MasterClient);
    }
    [PunRPC]
    private void RPCSyncDeath()
    {
        GameController.Instance.GameStats.CurrentEnemyCount -= 1;
        weapon.Collider.enabled = false;

        //prevents the game from rewarding both players
        if (Target.GetComponent<PhotonView>().IsMine)
        {
            GameController.Instance.GameStats.CurrentMoney += MoneyDrop;
            GameController.Instance.GameStats.EnemyKills += 1;
        }
    }

    [PunRPC]
    private void RPCNetworkDestroy()
    {
        PhotonNetwork.Destroy(gameObject);
    }

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();

        animationController = GetComponent<EnemyAnimationController>();
        enemyAgent = GetComponent<NavMeshAgent>();

        initialSpeed = this.Speed;
        initialMoneyDrop = this.moneyDrop;
    }

    public override void OnEnable()
    {
        base.OnEnable();

        isDeath = false;
        currentHealth = maxHealth;
        this.MoneyDrop = initialMoneyDrop;

        if(!enemyAgent.enabled)
            enemyAgent.enabled = true;

        this.Speed = initialSpeed;
    }

    // Update is called once per frame
    private void Update()
    {
        if (target == null || !PhotonNetwork.IsMasterClient) return;

        if (!enemyAgent.hasPath) return;

        enemyAgent.SetDestination(target.transform.position);

        if (enemyAgent.isOnOffMeshLink)
        {
            PhotonView.RPC("PlayAnimation", RpcTarget.All, "Jump");
        }

        if (enemyAgent.remainingDistance > enemyAgent.stoppingDistance)
        {
            weapon.Collider.enabled = false;

            PhotonView.RPC("PlayAnimation", RpcTarget.All, "Chase");
            PhotonView.RPC("StopAnimation", RpcTarget.All, "Attack"); // in case enemy was in the attack state previously
            return;
        }

        if (enemyAgent.remainingDistance <= enemyAgent.stoppingDistance)
        {
            LookAtTarget();
            if (enemyAgent.velocity.sqrMagnitude <= 0.1f)
                PhotonView.RPC("PlayAnimation", RpcTarget.All, "Attack");
        }
    }

    private void LookAtTarget()
    {
        Vector3 targetPosition = new Vector3(Target.transform.position.x, transform.position.y, Target.transform.position.z);
        transform.LookAt(targetPosition);
    }

    [PunRPC]
    private void PlayAnimation(string animation)
    {
        animationController.Play(animation);
    }

    [PunRPC]
    private void StopAnimation(string animation)
    {
        animationController.Stop(animation);
    }
}
