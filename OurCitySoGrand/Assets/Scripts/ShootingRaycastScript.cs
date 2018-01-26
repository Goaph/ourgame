
using UnityEngine;

public class ShootingRaycastScript : MonoBehaviour {

    private float damage = 5f; // How much damage the gun deals
    public float range = 100f; // The range of the bullets/ the distance the raycast travels
    private float impactForce = 0.1f;

    public Camera cam;
    public ParticleSystem muzzleflash;  
    
    

	void Update () {
        if (Input.GetButton("Fire1")) // left mouse button by default
        {
            Shoot(); //calls the shoot Function
        }
	}

    

    //Function that actually shoots the rays/Bullets
    void Shoot()
    {
        muzzleflash.Play(); // plays the muzzleflash Particule animation
        
        RaycastHit hit; //a variable used to store information on the object hit by the ray
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range)) // shoots a ray forward from the position of the camera, stores the target hit info in the "hit" variable, only goes the 'range' distance, returns true if something is hit
        {
            
            Debug.Log("Bullet hit: " + hit.transform.name);
            
            DamageHittableTarget(hit); // deals with Hittable Targets
                       
            MakeChaserLookWhenHit(hit); // Makes the Chaser look when hit


        }
    }

    void DamageHittableTarget(RaycastHit hit)
    {
        LivingCreature hitObj = hit.transform.GetComponent<LivingCreature>(); //checks if the object hit is part of the LivingCreature Class
        if (hitObj != null) // if it is part of the class, take damage
        {

            hitObj.TakeDamage(damage);
            hitObj.KnockBackWhenShot(impactForce);

        }
    }

    //Add this to the LIVINGCREATURE class eventually
    void MakeChaserLookWhenHit (RaycastHit hit)
    {
        
        Chaser hitObj = hit.transform.GetComponent<Chaser>(); // Checks if the target hit is a Chaser
        
        if (hitObj != null)
        {
            
            hitObj.ChaseTransition(); // sets the Behaviour Patrol to false
            
        }
    }
}
