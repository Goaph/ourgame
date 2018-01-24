using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chaser : MonoBehaviour {

    public Transform chaser;
    public Transform player;
    public Transform patrolGoal1;
    public Transform patrolGoal2;

    public float speed = 10f;
    public float patrolSwitchDistance = 3f;
    public float slerpRotateSpeed = 0.3f;
    public float searchDistance = 20f;
    public float LOSDistance = 45f;

    
    private float rayDistanceIncrement = 1f;
    private float rayAngleIncrement = 5f;
    private bool searchBreadCrumb = false;
    private Vector3 BreadCrumb;
    

    private NavMeshAgent agent;
    private Transform currentPatrol;

    public enum Behaviours { patrol, chase, search};
    public Behaviours behaviour;


    private void Start()
    {
        
        behaviour = Behaviours.patrol;
        agent = GetComponent<NavMeshAgent>(); //sets the agent variable to a navmesh agent
        currentPatrol = patrolGoal1; // sets the current patrol to patrolgoal1
        agent.SetDestination(currentPatrol.position); // sets the destination to current patrol
    }

    

    void Update ()
    {
       

        if (behaviour == Behaviours.patrol)
        {
            NavMeshPatrolCheck();
            VisionRaycasting();

        } else if (behaviour == Behaviours.chase)
        {
            PlayerChase();
        } else if (behaviour == Behaviours.search)
        {
            SearchBehaviour();
        }
        


       
    }

        
    //PATROLLING BETWEEN TWO GOALS
    void NavMeshPatrolCheck ()
    {
        
        if(agent.remainingDistance < patrolSwitchDistance) // If the remaining distance is less than the switch distance, switch the set destination of the chaser
        {
            if(currentPatrol == patrolGoal1)
            {
                currentPatrol = patrolGoal2;
            } else
            {
                currentPatrol = patrolGoal1;
            }
            agent.SetDestination(currentPatrol.position);
        }
       
    }   
      
       
    //FOLLOW PLAYER

    void PlayerChase()
    {
        chaser.LookAt(player); // makes sure that the chaser looks at the player
        agent.SetDestination(player.position); // sets the follow destination of the Navmesh agent to the player
        ChasingSearchingTransition();
    }


    //SEND OUT RAYCASTS THAT FUNCTION AS 'VISION'
    void VisionRaycasting()
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

            

            if (Physics.Raycast(chaser.position, newAngle, out hit, rayLength)) // Checks if the Ray has hit something
            {
                if (hit.collider.tag == "Player") //Checks if the Navigation rays have hit the player
                {
                    Debug.Log("Player came into our line of sight.");
                    behaviour = Behaviours.chase;
                }
            }
            Debug.DrawRay(chaser.position, newAngle * hit.distance, Color.green); //Draws the rays for debugging purposes - uses hit distance to indicate where the ray has actually hit



        }

    }

    void ChasingSearchingTransition()
    {

        Vector3 LOSangle = chaser.forward * Vector3.Angle(chaser.position, player.position);

        float rayLength = searchDistance;
               

        RaycastHit hit;

        if (Physics.Raycast(chaser.position, LOSangle, out hit, rayLength))

        {
            if(hit.collider.tag != "Player")
            {
                behaviour = Behaviours.search;
            }
            
        }
        Debug.DrawRay(chaser.position, LOSangle, Color.red);

    }

    void SearchBehaviour()
    {
        if(searchBreadCrumb == false)
        {
            searchBreadCrumb = true;
            BreadCrumb = player.position;

        }
        agent.SetDestination(BreadCrumb);
    }
    

    //TO DO: 
    // add comments
    // Make a Looking around behaviour
    // Make a transition back to patrol

}
