using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunController : MonoBehaviour
{
    [SerializeField] private float damage = 1f;
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

    private void Awake()
    {
        gunSoundEffect = GetComponent<GunAudioController>();
        currentAmmo = maxAmmo;        
    }

    // Update is called once per frame
    private void Update()
    {
        if(!isAmmoUISet)
        {
            InitializeAmmoUI();
            return;
        }

        if (isReloading) return;

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
        muzzleFlash.Play();
        gunSoundEffect.Play("shoot");

        currentAmmo--;
        //Debug.Log(currentAmmo);
        gameScreenUI.UpdateAmmoUI(currentAmmo, maxAmmo);

        RaycastHit hit;
        bool hasHit = Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range);
        if (hasHit)
        {
            //Debug.Log(hit.transform.name);

            //EnemyController enemy = hit.transform.GetComponent<EnemyController>();

            if (hit.transform.TryGetComponent<EnemyController>(out EnemyController enemy))
            {
                enemy.TakeDamage(damage);

                if (enemy.gameObject.TryGetComponent<Rigidbody>(out Rigidbody enemyRigidbody))
                    enemyRigidbody.AddForce(-hit.normal * impactForce);
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

        Animator animator = GetComponent<Animator>();
        animator.SetBool("Reloading", true);

        isReloading = true;
        //Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadTime);

        animator.SetBool("Reloading", false);
        isReloading = false;

        currentAmmo = maxAmmo;
        gameScreenUI.UpdateAmmoUI(currentAmmo, maxAmmo);
    }
}
