using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScript : MonoBehaviour
{

    public Chaser chaser;
    public Rigidbody rb;
    public bool shouldJump = false;
    public float jumpVelocity = 10f;

   

    private void FixedUpdate()
    {
        if (shouldJump == true)
        {
            
            Jump();
        }
    }
    
    public void Jump()
    {
        rb.AddForce(0, jumpVelocity, 0);

        Debug.Log("Jumping");
    }

    /*
    * private void JumpInput()
   {
       if (Input.GetKeyDown(KeyCode.Space))
       {
           Debug.Log("JumpKey clicked");
           shouldJump = true;
       }
   }
   private void OnTriggerStay(Collider other)
   {
       if (other.GetComponent<Collider>().tag == "Player")
       {
           Debug.Log("Trigger hit Chaser");

           shouldJump = true;
       }

   }
   */

}
