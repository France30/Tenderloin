using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private GunController playerGun;
    [SerializeField] private TextMeshProUGUI playerName;

    private bool isHealthUISet = false;

    private PlayerDamage playerDamage;
    private GameScreenUI gameScreenUI;

    private float currentHealth = 0f;

    public float CurrentHealth { get { return currentHealth; }
        set 
        {
            currentHealth = value;
        } 
    }
    public float MaxHealth 
    { 
        get { return maxHealth; }
        set 
        {
            maxHealth = value;
            if (maxHealth <= 0) maxHealth = 1;

            GameController.Instance.GameScreenUI.UpdateHealthBar(CurrentHealth, maxHealth);
        } 
    }

    public bool IsPlayerFiring { get; private set; }
    public bool IsPlayerDown { get; private set; }
    public bool IsPlayerReloading { get; set; }
    public bool IsPlayerSprinting { get; set; }   

    public PlayerAnimationController PlayerAnimation { get; private set; }
    public CharacterControllerMovement PlayerMovement { get; private set; }
    public GunController PlayerGun { get { return playerGun; } }
    public string PlayerName { get { return playerName.text; } set { playerName.text = value; } }
    public PhotonView PhotonView { get; private set; }

    private int damageResist = 0;
    public int DamageResist
    {
        get { return damageResist; }
        set
        {
            damageResist = value;
            if (damageResist < 0) damageResist = 0;
        }
    }
    
    public void TakeDamage(int amount)
    {
        if (CurrentHealth <= 0 || !PhotonView.IsMine) return;

        //apply damage resist if any
        if (damageResist >= amount)
            amount = 0;
        else
            amount -= damageResist; 

        CurrentHealth -= amount;
        if (CurrentHealth < 0) CurrentHealth = 0;

        GameController.Instance.GameScreenUI.UpdateHealthBar(CurrentHealth, maxHealth);
        playerDamage.DamageEffect();

        CheckIfPlayerDowned();
    }

    private void CheckIfPlayerDowned()
    {
        if (CurrentHealth > 0) return;

        PhotonView.RPC("RPCPlayerDowned", RpcTarget.All);
    }

    [PunRPC]
    private void RPCPlayerDowned()
    {
        IsPlayerDown = true;
        PlayerAnimation.Play("Downed");
        GameController.Instance.CheckIfAllPlayersDowned();

        if (GameController.Instance.IsGameOver) return;

        PlayerController[] players = FindObjectsOfType<PlayerController>();
        foreach (PlayerController player in players)
        {
            if (player.IsPlayerDown) continue;

            EnemyController[] enemy = FindObjectsOfType<EnemyController>();
            for (int i = 0; i < enemy.Length; i++)
                enemy[i].SetTarget(player.PhotonView.ViewID);

            return;
        }
    }

    [PunRPC]
    private void RPCSetGunAnimationBool(string parameter, bool state)
    {
        playerGun.Animator.SetBool(parameter, state);
    }

    [PunRPC]
    private void RPCShootEffect()
    {
        playerGun.MuzzleFlash.Play();
        playerGun.GunSoundEffect.Play("shoot");
    }

    private void Awake()
    {
        CurrentHealth = maxHealth;

        IsPlayerReloading = false;
        IsPlayerDown = false;
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        PhotonView = gameObject.GetComponent<PhotonView>();

        //Player Damage
        playerDamage = gameObject.GetComponent<PlayerDamage>();

        //Player Movement
        PlayerMovement = gameObject.GetComponent<CharacterControllerMovement>();

        //Player Animation
        if (gameObject.TryGetComponent<PlayerAnimationController>(out PlayerAnimationController animationController))
            PlayerAnimation = animationController;
        else
            PlayerAnimation = gameObject.AddComponent<PlayerAnimationController>();       
    }

    public override void OnEnable()
    {
        base.OnEnable();

        if (!PhotonView.IsMine) return;

        photonView.RPC("RPCSetPlayerName", RpcTarget.OthersBuffered, PhotonNetwork.LocalPlayer.NickName);
        GameController.Instance.Player = this;
    }

    [PunRPC]
    private void RPCSetPlayerName(string name)
    {
        playerName.text = name;
    }

    private void Update()
    {
        if (!PhotonView.IsMine) return;

        InitializeHealthUI(); //only runs once

        CheckIfPlayerIsFiring();
    }

    private void InitializeHealthUI()
    {
        if (isHealthUISet) return;

        try
        {
            gameScreenUI = GameController.Instance.GameScreenUI;
            gameScreenUI.UpdateHealthBar(CurrentHealth, maxHealth);

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
            IsPlayerFiring = true;
        else
            IsPlayerFiring = false;
    }
}
