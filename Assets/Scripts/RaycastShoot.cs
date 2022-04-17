using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastShoot : MonoBehaviour
{
    public int gunDamage = 1;
    public float fireRate = 0.25f;
    public float weaponRange = 500f;
    public float hitForce = 100f;
    public Transform gunEnd;

    private Camera fpsCam;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);
    private AudioSource gunAudio;
    private AudioClip shotClip;
    private LineRenderer laserLine;
    private float nextFire;

    [Header("FX")]
    public GameObject flashEffect;
    //public GameObject flashPosition;
    //public GameObject impactBlood;
    public LineRenderer bulletTrail;
    public ParticleSystem muzzleFlashParticle;

    [Header("Sound Effects")]
    public GameObject fireSoundSource;


    // Start is called before the first frame update
    void Start()
    {
        laserLine = GetComponent<LineRenderer>();
        //gunAudio = GetComponent<AudioSource>();

        // used to overlap sounds
        //shotClip = gunAudio.clip;

        fpsCam = GetComponentInParent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire) //the gun fires
        {
            
            
            nextFire = Time.time + fireRate;

            StartCoroutine(ShotEffect());

            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            RaycastHit hit;

            laserLine.SetPosition(0, gunEnd.position);

            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
            {
                laserLine.SetPosition(1, hit.point);
                SpawnBulletTrail(hit.point);   // spawns bullet trail
                Shootable health = hit.collider.GetComponent<Shootable>();

                if (health != null)
                {
                    health.Damage(gunDamage);
                    //SpawnBloodMist(hit); // spawns blood mist on target
                }

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * hitForce);
                }

            }
            else
            {
                laserLine.SetPosition(1, rayOrigin + (fpsCam.transform.forward) * weaponRange);
            }
        }
    }

    private IEnumerator ShotEffect()
    {
        //gunAudio.Play();
        //gunAudio.PlayOneShot(shotClip);

        SpawnMuzzleLight();

        muzzleFlashParticle.Emit(1); // show muzzle flash

        SpawnFireSound();

        laserLine.enabled = true;

        yield return shotDuration;

        laserLine.enabled = false;

    }

    private void SpawnFireSound()
    {
        GameObject fireSound = Instantiate(fireSoundSource, gunEnd);
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
        lineR.SetPosition(1, hitPoint);

        Destroy(bulletTrailEffect, 3f);
    }

    /*
    private void SpawnBloodMist(RaycastHit hitPoint)
    {
        GameObject effect = Instantiate(impactBlood, hitPoint.point + hitPoint.point.normalized*6, gunEnd.);
    }

    */






    

    
}
