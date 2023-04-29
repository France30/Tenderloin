using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class GunController : MonoBehaviourPunCallbacks
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float range = 100f;
    [SerializeField] private float fireRate = 20f;
    [SerializeField] private int maxAmmo = 15;
    [SerializeField] private float reloadTime = 1.8f;
    [SerializeField] private float impactForce = 600f;

    [SerializeField] private Camera fpsCam;
    [SerializeField] private ParticleSystem muzzleFlash;

    private GunAudioController gunSoundEffect;

    private float nextTimeToFire = 1f;
    private int currentAmmo;
    private bool isReloading = false;
    private bool isAmmoUISet = false;

    private GameScreenUI gameScreenUI;
    private Animator animator;

    public ParticleSystem MuzzleFlash { get { return muzzleFlash; } }
    public GunAudioController GunSoundEffect { get { return gunSoundEffect; } }
    public Animator Animator { get { return animator; }}
    private bool IsReloading { 
        get { return isReloading; } 
        set 
        {
            isReloading = value;
            GameController.Instance.Player.IsPlayerReloading = isReloading;
        } 
    }

    public int Damage 
    { 
        get { return damage; } 
        set 
        {
            damage = value;
            if (damage <= 0) damage = 1;
        } 
    }
    public float FireRate 
    { 
        get { return fireRate; }
        set
        {
            fireRate = value;
            if (fireRate <= 0) fireRate = 1;
        }
    }
    public int MaxAmmo 
    { 
        get { return maxAmmo; } 
        set 
        {
            maxAmmo = value; 
            currentAmmo = value;

            if (maxAmmo <= 0) maxAmmo = 1;
            if (currentAmmo <= 0) currentAmmo = 1;

            gameScreenUI.UpdateAmmoUI(currentAmmo, maxAmmo);
        } 
    }
    public float ReloadTime
    {
        get { return reloadTime; }
        set
        {
            reloadTime = value;
            if (reloadTime <= 0) reloadTime = 1;
        }
    }

    public PhotonView PhotonView { get; set; }

    private void Awake()
    {
        PhotonView = GetComponentInParent<PhotonView>();
        gunSoundEffect = GetComponent<GunAudioController>();
        animator = GetComponent<Animator>();
        currentAmmo = maxAmmo;        
    }

    // Update is called once per frame
    private void Update()
    {
        if (!PhotonView.IsMine) return;

        InitializeAmmoUI(); //only runs once

        gunSprint();

        if (IsReloading || GameController.Instance.IsGameOver) return;

        if(Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            if (currentAmmo <= 0)
            {
                gunSoundEffect.Play("no ammo");
                return;
            }

            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

        if (Input.GetKeyDown("r"))
        {
            StartCoroutine(Reload());
        }        
    }

    private void InitializeAmmoUI()
    {
        if (isAmmoUISet) return;

        try
        {
            gameScreenUI = GameController.Instance.GameScreenUI;
            gameScreenUI.UpdateAmmoUI(currentAmmo, maxAmmo);

            isAmmoUISet = true;
        }
        catch (NullReferenceException e)
        {
            //do nothing
        }
    }

    private void Shoot()
    {
        PhotonView.RPC("RPCShootEffect", RpcTarget.All);

        currentAmmo--;
        //Debug.Log(currentAmmo);
        gameScreenUI.UpdateAmmoUI(currentAmmo, maxAmmo);

        RaycastHit hit;
        bool hasHit = Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range);
        if (hasHit)
        {
            if (hit.transform.TryGetComponent<EnemyController>(out EnemyController enemy))
            {
                enemy.SetTarget(GameController.Instance.Player.PhotonView.ViewID);
                enemy.TakeDamage(damage);               
            }
        }

        BulletLine bulletLine = GetComponent<BulletLine>();

        //PC bullet line
        bulletLine.SetPositions(new Vector3[] { muzzleFlash.transform.position, hasHit ? hit.point : 
            muzzleFlash.transform.position + fpsCam.transform.forward * range });
    }

    private IEnumerator Reload()
    {
        gunSoundEffect.Play("reload");
        
        PhotonView.RPC("RPCSetGunAnimationBool", RpcTarget.All,"Reloading", true);

        IsReloading = true;
        //Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadTime);

        PhotonView.RPC("RPCSetGunAnimationBool", RpcTarget.All, "Reloading", false);
        IsReloading = false;

        currentAmmo = maxAmmo;
        gameScreenUI.UpdateAmmoUI(currentAmmo, maxAmmo);
    }

    private void gunSprint()
    {
        bool isSprinting = GameController.Instance.Player.IsPlayerSprinting;
        PhotonView.RPC("RPCSetGunAnimationBool", RpcTarget.All, "Sprinting", isSprinting);
    }

}
