
using UnityEngine;

public class ShootingRaycastScript : MonoBehaviour {

    public float damage = 10f; // How much damage the gun deals
    public float range = 100f; // The range of the bullets/ the distance the raycast travels
    public float impactForce = 40f;

    public Camera cam;
    public ParticleSystem muzzleflash;
    

	void Update () {
        if (Input.GetButtonDown("Fire1")) // left mouse button by default
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
            //PUT THIS INSIDE THE OBJECT GETTING HIT!
            Debug.Log("Raycast hit: " + hit.transform.name);
            HittableTarget hitObj = hit.transform.GetComponent<HittableTarget>(); //checks if the object hit is part of the HittableTarget Class
            if(hitObj != null) // if it is part of the class, take damage
            {
                
                hitObj.TakeDamage(damage);

            }
            if(hit.rigidbody != null)
            {
                Debug.Log("Adding Force!");
                hit.rigidbody.AddForce(cam.transform.forward * impactForce);
            }
        }
    }
}
