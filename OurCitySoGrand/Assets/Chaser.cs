using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : MonoBehaviour {

    public Transform chaser;
    public Transform player;

    public float speed = 10f;
    public float stopDistance = 3f;

    public float rayDistance = 5f;
    private float rayAngleIncrement = 15f;

	void Update () {

        //BASIC PLAYER FOLLOW
        chaser.LookAt(player); // Makes the chaser look at the player
        if (Vector3.Distance(chaser.position, player.position) > stopDistance) // Moves the chaser as long as they are out of the distance range to stop
        {
            
            chaser.Translate(0, 0, speed * Time.deltaTime); // Translating the Cube forward by its speed multiplied by deltaTime
        } else
        {
            Debug.Log("HitPlayer"); //player has been hit
        }

       //NAVIGATION RAYCASTING:
      
        
        float angleOffset = -105f; //Starts off the angle at the far left, increments by 15 so angleOffset actually starts at -90;
           
        for(int i = 0; i < 13; i++) // all the rays from -90 to 90 are procedurally generated
        {
            
            Quaternion spreadAngle = Quaternion.AngleAxis(angleOffset, new Vector3(0, 1, 0)); // makes an angleOffset based off the Origin of the Y axis multiplied by the current offset
            Vector3 newAngle = spreadAngle * chaser.forward; //creates a new angle that rotates spreadAngleOffset so that it matches the current rotation of the chaser
           
            RaycastHit hit;            //creating a hit variable to store the collision data of the ray in

            Debug.DrawRay(chaser.position, newAngle * rayDistance, Color.green); //Draws the rays for debugging purposes

            if (Physics.Raycast(chaser.position, newAngle, out hit, rayDistance)) // Checks if the Ray has hit something
            {
                if (hit.collider.tag == "Terrain") //Checks if the Navigation rays have hit terrain
                {
                    Debug.Log("We hit terrain!");
                }
            }
            angleOffset += rayAngleIncrement; //Incrememnts the angleOffset to further generate all the rays
            Debug.Log(angleOffset);
                      

        }

    }
}
