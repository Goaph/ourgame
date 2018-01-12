using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public Transform player;
   
    public float forwardSpeed = 5.0f;
    public float sideSpeed = 5.0f;
    public float cameraHorizontalSpeed = 30.0f;
    

    void Update () {
        //Moves Character Forward and Backwards, Left and Right
        //Gets the Up and Down Keys, then left and right
        float moveForward = Input.GetAxis("Vertical") * forwardSpeed;
        float moveSide = Input.GetAxis("Horizontal") * sideSpeed;

        //Multiplied by DeltaTime to be consistent across all devices
        moveForward *= Time.deltaTime;
        moveSide *= Time.deltaTime;

        //Player's position is actually translated
        player.Translate(0, 0, moveForward);
        player.Translate(moveSide, 0, 0);

        
        

    
    }
}
