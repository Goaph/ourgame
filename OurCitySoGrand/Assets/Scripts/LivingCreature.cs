
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
        Destroy(gameObject); //Destroys the object
    }
}
