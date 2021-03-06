using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastShoot : MonoBehaviour
{
    public int gunDamage = 1;
    public float fireRate = 0.25f;
    public float weaponRange = 500f;
    public float hitForce = 100f;
    public float spreadAmount = 5f;
    public Transform gunEnd;
    public float recoil = 0f,
                 recoilSpeed = 10f,
                 maxRecoil_x = -20f;
    private Quaternion shotAcc;


    public Camera fpsCam;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.05f);
    private AudioSource gunAudio;
    private AudioClip shotClip;
    private LineRenderer laserLine;
    private float nextFire;
    private float resetSpray = 0.8f;

    [Header("FX")]
    public GameObject bulletHole;
    public GameObject flashEffect;
    //public GameObject impactBlood;
    public LineRenderer bulletTrail;
    public ParticleSystem muzzleFlashParticle1;
    public ParticleSystem muzzleFlashParticle2;
    public ParticleSystem muzzleFlashParticle3;
    public ParticleSystem muzzleFlashParticle4;
    public ParticleSystem muzzleFlashParticle5;
    public ParticleSystem muzzleFlashParticle6;

    [Header("Sound Effects")]
    public GameObject fireSoundSource;

    private Recoil Recoil_Script;


    // Start is called before the first frame update
    void Start()
    {
        laserLine = GetComponentInChildren<LineRenderer>();
        //gunAudio = GetComponent<AudioSource>();

        // used to overlap sounds
        //shotClip = gunAudio.clip;
        Recoil_Script = transform.Find("CameraRot/GunRecoil").GetComponent<Recoil>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire) //the gun fires
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        nextFire = Time.time + fireRate;
        StartCoroutine(ShotEffect());
        Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit hit;
        laserLine.SetPosition(0, gunEnd.position);
        if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
        {
            laserLine.SetPosition(1, hit.point);

            SpawnBulletTrail(hit.point);   // spawns bullet vapor trail
            Shootable health = hit.collider.GetComponent<Shootable>();

            if (health != null)
            {
                health.Damage(gunDamage);
                SpawnBloodMist(hit); // spawns blood mist on target
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * hitForce);
            }
            else
            {
                Instantiate(
                    bulletHole,
                    hit.point + (hit.normal * 0.01f),
                    Quaternion.FromToRotation(Vector3.up, hit.normal));
            }

        }
        else
        {
            laserLine.SetPosition(1, rayOrigin + (fpsCam.transform.forward) * weaponRange);
        }

        Recoil_Script.RecoilFire();
    }

    private IEnumerator ShotEffect()
    {
        //gunAudio.Play(); //removed because of SpawnFireSound
        //gunAudio.PlayOneShot(shotClip);

        muzzleFlashParticle1.Emit(1); // shows muzzle flash particle
        muzzleFlashParticle2.Emit(1);
        muzzleFlashParticle3.Emit(1);
        muzzleFlashParticle4.Emit(1);
        muzzleFlashParticle5.Emit(1);
        muzzleFlashParticle6.Emit(1);

        SpawnMuzzleLight(); //spawns a light in the game world

        SpawnFireSound(); //spawns an object in the game  world that makes a noise

        laserLine.enabled = true;

        yield return shotDuration;

        laserLine.enabled = false;

    }

    private void SpawnFireSound()
    {
        GameObject fireSound = Instantiate(fireSoundSource, gunEnd.position, gunEnd.rotation);
    }
    private void SpawnMuzzleLight()
    {
        GameObject flashLight = Instantiate(flashEffect, gunEnd);
        Destroy(flashLight, 0.05f); //Should this really be destroyed here? what if this person dies right when the flash is spawned, then it will exist forever.
    }

    private void SpawnBulletTrail(Vector3 hitPoint)
    {
        GameObject bulletTrailEffect = Instantiate(bulletTrail.gameObject, gunEnd.position, gunEnd.transform.rotation); // what does quaternion identity do? how is it linked to the bullet path?

        LineRenderer lineR = bulletTrailEffect.GetComponent<LineRenderer>();

        lineR.SetPosition(0, gunEnd.position);
        //LineR.SetPosition(1, gunEnd.position)
        lineR.SetPosition(1, hitPoint);

        Destroy(bulletTrailEffect, 3f);
    }

    
    private void SpawnBloodMist(RaycastHit hitPoint)
    {
        //GameObject effect = Instantiate(impactBlood, hitPoint.point + hitPoint.point.normalized*6, gunEnd.);
    }
}
