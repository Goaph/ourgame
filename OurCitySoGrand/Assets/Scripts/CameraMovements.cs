using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovements : MonoBehaviour {

    public float minY = 0.4f;
    public float maxY = -0.7f;
    public float cameraVerticalSpeed = 2.0f;

    private Vector3 euler;

    // Update is called once per frame
    void Update () {
        //Rotating the Camera (not the player) based off mouse pos on the X axis
        float v = -cameraVerticalSpeed * Input.GetAxis("Mouse Y"); //is negative so that it's not inverse
        transform.Rotate(v, 0, 0);

        
        Debug.Log(transform.rotation.x);
        /*if (transform.rotation.x <= maxY)
        {
            Debug.Log("UpperY reached!");
            transform.rotation.x = maxY;
        }

        if (transform.rotation.x >= minY)
        {
            Debug.Log("LowerY reached!");
            transform.rotation.x = minY;
        }
        */
    }
}
