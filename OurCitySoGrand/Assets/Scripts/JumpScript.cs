using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScript : MonoBehaviour {

    public Chaser chaser;

    private void OnTriggerStay (Collider other)
    {
        if(other.GetComponent<Collider>().tag == "Player")
        {
            Debug.Log("Trigger hit Chaser");

            chaser.shouldJump = true;
        }
        
    }
}
