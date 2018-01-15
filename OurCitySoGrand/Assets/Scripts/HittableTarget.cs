﻿
using UnityEngine;

public class HittableTarget : MonoBehaviour {

    public float health = 50f;
   
   
    public void TakeDamage(float dmgAmt)
    {
        health -= dmgAmt; //health is decreased by damage
        
        
        if (health <= 0)
        {
            KillObject(); // Kills object if health is less than or = to 0
        }
    }

    public void KillObject()
    {
        Debug.Log("ObjectKilled!");
        Destroy(gameObject); //Destroys the object
    }
}