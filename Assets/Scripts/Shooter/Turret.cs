using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform target;

    [Header("Attributes")]
    public int ammo = 100;
    public float range = 15f;
    public float fireRate = 1f;
    public float turnSpeed = 10f;
    public bool locked = false;
    public float damage = 5f;

    [Header("Unity Setup Fields")]

    private float fireCountdown = 0f;
    public Transform firePoint;
    public Ray lockOnLine;
    public RaycastHit hit;

    [Header("Positional Data")]
    public Transform barrelRotate;
    public Transform baseRotate;

    [Header("FX")]
    public GameObject flashLight;
    public GameObject flashPosition;
    public GameObject impactBlood;
    public LineRenderer bulletTrail;
    public ParticleSystem muzzleFlash;
    //public Intelligence enemy;
    


    [Header("Sound Effects")]
    public GameObject fireSoundSource;
    public AudioSource fireSource;
    public string enemyTag = "Enemy";

    private Quaternion barrelLookQuat;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    // Update is called once per frame
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        GameObject bestEnemy = null;
        float distanceToBestEnemy = 0;
        float minScore = 100000000000;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            float angleToEnemy = Mathf.Abs(baseRotate.rotation.eulerAngles.y - Quaternion.LookRotation(enemy.transform.position - transform.position).eulerAngles.y);
            float score = angleToEnemy + distanceToEnemy; //the turret wants to choose the closest enemy, and the enemy it would have to rotate the least for
            Debug.Log("distance: " + distanceToEnemy + " angle: " + angleToEnemy + " score: " + score);
            if (score < minScore) // the turret chooses the lowest score i.e. the easiest target to turn 
            {
                bestEnemy = enemy;
                minScore = score;
                distanceToBestEnemy = distanceToEnemy;
            }
        }

        if (bestEnemy != null && distanceToBestEnemy <= range)
        {
            target = bestEnemy.transform;
        }
    }



    private void Update()
    {
        if (target == null)
            return;

        // Target Lock on
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.RotateTowards(baseRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        barrelLookQuat = Quaternion.LookRotation(target.position - firePoint.position);
        Vector3 barrelLook = barrelLookQuat.eulerAngles;

        // Barrel facing enemy
        lockOnLine = new Ray(firePoint.position, firePoint.forward);
        baseRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        barrelRotate.rotation = Quaternion.Euler((barrelLook.x >= 23 && barrelLook.x < 180) ? 23 : barrelLook.x, rotation.y, 0f);

        GameObject enemy = null;
        //if the gun is pointed at the target locked is made true
        if(Physics.Raycast(lockOnLine, out hit, dir.magnitude))
        {
            if(hit.collider.tag == enemyTag)
            {
                locked = true;
                enemy = hit.transform.gameObject;
            }
        }

        if (fireCountdown <= 0f && locked && ammo > 0)
        {
            Fire(enemy, lockOnLine, hit);
            ammo -= 1;
            fireCountdown = 1f / fireRate;
        }
        locked = false;
        fireCountdown -= Time.deltaTime;
    }

    void Fire(GameObject enemy, Ray firingLine, RaycastHit hitPoint)
    {



        SpawnMuzzleLight();
        muzzleFlash.Emit(1); // show muzzle flash

        //fireSource.pitch = Random.Range(0.75f, 0.9f);
        //fireSource.Play(); //play sound effect
        SpawnFireSound();

        SpawnBloodMist(hitPoint); // spawns blood mist on target
        SpawnBulletTrail(hitPoint.point);   // spawns bullet trail
        GiveDamage(enemy);
    }

    private void SpawnFireSound()
    {
        GameObject fireSound = Instantiate(fireSoundSource, flashPosition.transform.position, barrelLookQuat);
    }
    private void SpawnMuzzleLight()
    {
        GameObject flash = Instantiate(flashLight, flashPosition.transform.position, barrelLookQuat);
        Destroy(flash, 0.05f);
    }

    private void SpawnBloodMist(RaycastHit hitPoint)
    {
        GameObject effect = Instantiate(impactBlood, hitPoint.point + hitPoint.point.normalized*6, barrelLookQuat);
    }
    private void SpawnBulletTrail(Vector3 hitPoint)
    {
        GameObject bulletTrailEffect = Instantiate(bulletTrail.gameObject, firePoint.position, barrelLookQuat); // what does quaternion identity do? how is it linked to the bullet path?

        LineRenderer lineR = bulletTrailEffect.GetComponent<LineRenderer>();

        lineR.SetPosition(0, firePoint.position);
        lineR.SetPosition(1, hitPoint);

        Destroy(bulletTrailEffect, 3f);
    }

    private void GiveDamage(GameObject enemy)
    {
        if (enemy == null)
            return;

        Enemy enemySC = enemy.GetComponent<Enemy>();
        if (enemySC == null)
            return;

        enemySC.TakeDamage(damage);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
