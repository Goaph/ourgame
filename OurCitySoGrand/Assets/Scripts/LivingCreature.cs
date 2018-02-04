
using UnityEngine;

public class LivingCreature : MonoBehaviour {

    public float health = 50f;
   
    

    

    public void TakeDamage(float dmgAmt)
    {
        health -= dmgAmt; //health is decreased by damage
       
        
        if (health <= 0)
        {
            KillObject(); // Kills object if health is less than or = to 0
        }
    }

    public void KnockBackWhenShot(float force)
    {
        
        
        GetComponent<Rigidbody>().AddForce(FindObjectOfType<Camera>().transform.forward * force, ForceMode.Impulse);
    }

    public void KillObject()
    {
        var emission = FindObjectOfType<Chaser>().explosion.emission;
        emission.enabled = true;
        FindObjectOfType<Chaser>().transform.DetachChildren();
        FindObjectOfType<Chaser>().explosion.Play();
        Destroy(gameObject); //Destroys the object
    }
}
