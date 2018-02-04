using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    public Transform target;

    [Header("Attributes")]

    public float range = 15f;
    public float fireRate = 1f;
    private float fireCountdown = 0f;
    public float turnSpeed = 10f;

    [Header("Unity things")]

    public string playerTag = "Player";

    public Transform PartstoRotate;
    

    [Header("Bullet Stuff")]

    public GameObject bulletPrefab;
    public Transform firePoint;
    public float Decay = 2.0f;
    public float bulletSpeed = 10f;


	// Use this for in  itialization
	void Start () {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        
    }

    void UpdateTarget()
    {
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(playerTag);

        foreach (GameObject enemy in enemies)
        
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }
	
	
	void Update () {
        if(target == null)
            return;

        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(PartstoRotate.rotation,lookRotation, Time.deltaTime*turnSpeed).eulerAngles;
        PartstoRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);


        if(fireCountdown <= 0f)
        {

            Shoot();
            fireCountdown = 1f / fireRate; 

        }

        fireCountdown -= Time.deltaTime;
           
		
	}

    void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;

        Destroy(bullet, Decay);
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
       
    }

}

