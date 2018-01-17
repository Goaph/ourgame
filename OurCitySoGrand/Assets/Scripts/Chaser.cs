using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : MonoBehaviour {

    public Transform chaser;
    public Transform player;

    public float speed = 10f;
    public float stopDistance = 3f;

    
    private float rayDistanceIncrement = 1f;
    private float rayAngleIncrement = 5f;
    

    

    //13 nav rays
    //when a counter reaches seven, flip Ray distanceincrement to a negative;

	void Update ()
    {
        UseRaycastsForNavigation();

        PlayerFollow();
    }

        

       
      
       
    

    void PlayerFollow()
    {
  
        //BASIC PLAYER FOLLOW
        chaser.LookAt(player); // Makes the chaser look at the player
        if (Vector3.Distance(chaser.position, player.position) > stopDistance) // Moves the chaser as long as they are out of the distance range to stop
        {

            chaser.Translate(0, 0, speed * Time.deltaTime); // Translating the Cube forward by its speed multiplied by deltaTime
        }
        else
        {
            Debug.Log("HitPlayer"); //player has been hit
        }
    }

    void UseRaycastsForNavigation()
    {

        float angleOffset = -90 - rayAngleIncrement; //Starts off the angle at the far left, increments by 15 so angleOffset actually starts at -90;

        float rayLength = 2; // sets the Initial Length of the First Ra
        rayDistanceIncrement = Mathf.Abs(rayAngleIncrement); //makes the Distance Increment Positive so that it starts off correctly


        while (angleOffset != 90) // all the rays from -90 to 90 are procedurally generated
        {
            if (angleOffset == 0) // If the ray is pointing forward, make the raydistance increment negative so that the rays start getting shorter again
            {
                rayDistanceIncrement = -rayDistanceIncrement;
            }

            rayLength += rayDistanceIncrement; //increase / decrease the ray by the distance Increment

            angleOffset += rayAngleIncrement; //Incrememnts the angleOffset to further generate all the rays

            Quaternion spreadAngle = Quaternion.AngleAxis(angleOffset, new Vector3(0, 1, 0)); // makes an angleOffset based off the Origin of the Y axis multiplied by the current offset
            Vector3 newAngle = spreadAngle * chaser.forward; //creates a new angle that rotates spreadAngleOffset so that it matches the current rotation of the chaser

            RaycastHit hit;            //creating a hit variable to store the collision data of the ray in

            Debug.DrawRay(chaser.position, newAngle * rayLength, Color.green); //Draws the rays for debugging purposes

            if (Physics.Raycast(chaser.position, newAngle, out hit, rayLength)) // Checks if the Ray has hit something
            {
                if (hit.collider.tag == "Terrain") //Checks if the Navigation rays have hit terrain
                {
                    Navigate(newAngle); // passes the angle of the raycast into the Navigate Function
                }
            }



        }

    }

    void Navigate (Vector3 rotationAngle)
    {
        Debug.Log("Navigating.");

    }

}
