using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chaser : LivingCreature {

    
    public Transform chaser;
    public Transform player;
    public Transform patrolGoal1;
    public Transform patrolGoal2;

    public GameObject breadcrumb;
    private GameObject bc;

    public Material patrol;
    public Material chase;
    public Material search;

    public float chaserHealth = 200f;
    public float lookRotationSpeed = 50f;
    public float speed = 10f;
    public float patrolSwitchDistance = 3f;
    public float slerpRotateSpeed = 0.3f;
    public float searchDistance = 20f;
    public float LOSDistance = 45f;

    
    
    private float rayDistanceIncrement = 3f;
    private float rayAngleIncrement = 5f;
    private bool searchBreadCrumb = false;
    private bool gotPlayerPos = false;

    private Vector3 lookAtPlayerPosWhenSearchPos = Vector3.forward;




    private NavMeshAgent agent;
    private Transform currentPatrol;

    public enum Behaviours { patrol, chase, search};
    public Behaviours behaviour;


    private void Start()
    {
        health = chaserHealth;
        PatrolTransition();
        agent = GetComponent<NavMeshAgent>(); //sets the agent variable to a navmesh agent
        currentPatrol = patrolGoal1; // sets the current patrol to patrolgoal1
        agent.SetDestination(currentPatrol.position); // sets the destination to current patrol
    }

    

    void Update ()
    {
               
        if (behaviour == Behaviours.patrol)
        {
            NavMeshPatrolCheck();
            VisionRayCasting();

        } else if (behaviour == Behaviours.chase)
        {
            PlayerChase();
        } else if (behaviour == Behaviours.search)
        {
            SearchBehaviour();
            VisionRayCasting();
        }

        ResetSearchVariables();



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
        ChasingLOSCheck();
    }


    //SEND OUT RAYCASTS THAT FUNCTION AS 'VISION'
    void VisionRayCasting()
    {

        float angleOffset = -90 - rayAngleIncrement; //Starts off the angle at the far left, increments by 15 so angleOffset actually starts at -90;

        float rayLength = 1; // sets the Initial Length of the First Ra
        rayDistanceIncrement = Mathf.Abs(rayDistanceIncrement); //makes the Distance Increment Positive so that it starts off correctly


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
                HasHitPlayer(hit);
            }
            Debug.DrawRay(chaser.position, newAngle * rayLength, Color.green); //Draws the rays for debugging purposes - uses hit distance to indicate where the ray has actually hit



        }

    }

    void HasHitPlayer(RaycastHit hit)
    {
        if (hit.collider.tag == "Player") //Checks if the Navigation rays have hit the player
        {
            Debug.Log("Player came into our line of sight.");
            ChaseTransition();
        }
    }
    

    void ChasingLOSCheck()
    {
        //Draws a raycast between the player and the Chaser
        Vector3 LOSangle = chaser.forward * Vector3.Angle(chaser.position, player.position);

        float rayLength = searchDistance;
               

        RaycastHit hit;

        if (Physics.Raycast(chaser.position, LOSangle, out hit, rayLength)) 

        {
            if(hit.collider.tag != "Player") // If there is no more LOS between chaser and player, go into Searching mode
            {
                SearchTransition();
            }
            
        }
        Debug.DrawRay(chaser.position, LOSangle, Color.red);

    }

    void SearchBehaviour() // SEARCHING BEHAVIOUR
    {
        if(searchBreadCrumb == false) // If a breadcrumb has not yet already been made...
        {
            searchBreadCrumb = true;
            bc = (GameObject) Instantiate(breadcrumb, player.position, Quaternion.identity); //Make a new breadcrumb gameobject at the player's position
            agent.SetDestination(bc.transform.position); // Set the Navmesh agent to the breadcrumb's position
            

        }
        if(agent.remainingDistance < 0.5f) // If the remaining distance between agent and player is less than 0.5
        {
            if(gotPlayerPos == false) //Checks to see if the player position has been found yet
            {
                lookAtPlayerPosWhenSearchPos = player.position; // Looks at where the player is standing
                gotPlayerPos = true;
            }

            Debug.Log("Reached the Breacrumb, about to rotate!");

            //Sets up variables so that Chaser can look in the player direction
            Vector3 targetDir = lookAtPlayerPosWhenSearchPos - transform.position;
            float step = lookRotationSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);

            Debug.DrawRay(transform.position, newDir, Color.red);

            transform.rotation = Quaternion.LookRotation(newDir); // rotates the player

            if (Vector3.Angle(targetDir, chaser.forward) < 2f) // once it finally looks at the player direction, return to Patrol
            {
                PatrolTransition();
            }

        }

        
    }

    private void ResetSearchVariables() //Resets all the variables for Search whenever there is a behavioural switch
    {
        if(behaviour != Behaviours.search)
        {            
            Destroy(bc); // Destroys the breadcrumb
            searchBreadCrumb = false; //indicates that the breadcrumb has not yet been made
            gotPlayerPos = false; // indicates that the player position has not yet been found
        }
    }

    public void ChaseTransition()
    {
        behaviour = Behaviours.chase; // Sets the behaviour to chase and makes the material chase material
        gameObject.GetComponent<Renderer>().material = chase;
    }

    public void SearchTransition() // Sets the behaviour to search and makes the material search material
    {
        behaviour = Behaviours.search;
        gameObject.GetComponent<Renderer>().material = search;
    }

    public void PatrolTransition() // Sets the behaviour to patrol and makes the material patrol material
    {
        behaviour = Behaviours.patrol;
        gameObject.GetComponent<Renderer>().material = patrol;
    }


    //TO DO: 
    // add comments
    // Make a Looking around behaviour
    // Make a transition back to patrol

}
