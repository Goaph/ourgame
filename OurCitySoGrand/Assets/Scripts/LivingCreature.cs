
using UnityEngine;

public class LivingCreature : MonoBehaviour {

    public float health = 50f;
    private float impactForce;
    private bool shouldKnockBackInFixedUpdate = false;

    private void FixedUpdate()
    {
        if (shouldKnockBackInFixedUpdate == true)
        {
            GetComponent<Rigidbody>().AddForce(FindObjectOfType<Camera>().transform.forward * impactForce, ForceMode.Impulse);
            shouldKnockBackInFixedUpdate = false;
        }
    }

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
        shouldKnockBackInFixedUpdate = true;
        impactForce = force;
    }

    public void KillObject()
    {       
        Destroy(gameObject); //Destroys the object
    }
}
