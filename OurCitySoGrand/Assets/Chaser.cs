using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : MonoBehaviour {

    public Transform chaser;
    public Transform player;

    public float speed = 10f;
    public float stopDistance = 3f;

	void Update () {

        chaser.LookAt(player);
        if (Vector3.Distance(chaser.position, player.position) > stopDistance)
        {
            
            chaser.Translate(0, 0, speed * Time.deltaTime);
        }else
        {
            Debug.Log("Gotcha!");
        }
        

	}
}
