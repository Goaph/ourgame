
using UnityEngine;

public class ShootingRaycastScript : MonoBehaviour {

    public float damage = 10f; // How much damage the gun deals
    public float range = 100f; // The range of the bullets/ the distance the raycast travels
    public float impactForce = 30f;

    public Camera cam;
    public ParticleSystem muzzleflash;

    private bool shouldKnockBackInFixedUpdate = false;
    private Rigidbody targetHit;
    

	void Update () {
        if (Input.GetButton("Fire1")) // left mouse button by default
        {
            Shoot(); //calls the shoot Function
        }
	}

    private void FixedUpdate()
    {
        if (shouldKnockBackInFixedUpdate == true)
        {
            targetHit.AddForce(cam.transform.forward * impactForce);
            shouldKnockBackInFixedUpdate = false;
        }
    }

    //Function that actually shoots the rays/Bullets
    void Shoot()
    {
        muzzleflash.Play(); // plays the muzzleflash Particule animation
        
        RaycastHit hit; //a variable used to store information on the object hit by the ray
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range)) // shoots a ray forward from the position of the camera, stores the target hit info in the "hit" variable, only goes the 'range' distance, returns true if something is hit
        {
            
            Debug.Log("Raycast hit: " + hit.transform.name);
            
            DamageHittableTarget(hit); // deals with Hittable Targets

            BulletRigidbodyKnockback(hit); // Knocks back objects with rigidbodies.
            MakeChaserLookWhenHit(hit); // Makes the Chaser look when hit


        }
    }

    void DamageHittableTarget(RaycastHit hit)
    {
        HittableTarget hitObj = hit.transform.GetComponent<HittableTarget>(); //checks if the object hit is part of the HittableTarget Class
        if (hitObj != null) // if it is part of the class, take damage
        {

            hitObj.TakeDamage(damage);

        }
    }

    void BulletRigidbodyKnockback(RaycastHit hit)
    {
       
        if (hit.rigidbody != null) //adds a rigidbody force
        {
            shouldKnockBackInFixedUpdate = true;
            targetHit = hit.rigidbody;            
           
        }
    }

    void MakeChaserLookWhenHit (RaycastHit hit)
    {
        Chaser hitObj = hit.transform.GetComponent<Chaser>(); // Checks if the target hit is a Chaser
        
        if (hitObj != null)
        {
            hitObj.SetBehaviourPatrol(false); // sets the Behaviour Patrol to false
            Debug.Log("Triggering the PlayerFollow.");
        }
    }
}
