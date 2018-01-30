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

    

   
    public float lookRotationSpeed = 50f;
    public float speed = 10f;
    public float patrolSwitchDistance = 3f;
    public float slerpRotateSpeed = 0.3f;
    public float searchDistance = 20f;
   

    public float visionAngle = 20f;
    public float visionLength = 10f;

    public bool shouldJump = false;
    
    
    private bool searchBreadCrumb = false;
    private bool gotPlayerPos = false;

    private Vector3 lookAtPlayerPosWhenSearchPos = Vector3.forward;
    
    private NavMeshAgent agent;
    private Transform currentPatrol;

    public enum Behaviours { patrol, chase, search};
    public Behaviours behaviour;


    private void Start()
    {
        
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

    private void FixedUpdate()
    {
        if(shouldJump == true)
        {
            shouldJump = false;
            Jump();
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
        ChasingLOSCheck();
    }


    //CHASER 'VISION'

    void VisionRayCasting()
    {
        
        Vector3 chaserPlayerAngle = chaser.forward * Vector3.Angle(chaser.position, player.position); // Generates angle between player and Chaser

        Quaternion spreadRight = Quaternion.AngleAxis(visionAngle, new Vector3(0, 1, 0)); //Creates the right and left angles for the Range of Chaser's LOS
        Quaternion spreadLeft = Quaternion.AngleAxis(-visionAngle, new Vector3(0, 1, 0));

        //Generating a normalised value of the LOS Angles as a Vector3 with no Magnitude
        Vector3 LOSAngleRight = spreadRight * chaserPlayerAngle;
        float disRight = LOSAngleRight.magnitude;
        LOSAngleRight = LOSAngleRight / disRight;


        Vector3 LOSAngleLeft = spreadLeft * chaserPlayerAngle;
        float disLeft = LOSAngleLeft.magnitude;
        LOSAngleLeft = LOSAngleLeft / disLeft;

        //Gets the angle of the Player
        float playerAngle = Vector3.Angle(chaser.transform.forward, player.position - transform.position);

        if (playerAngle < visionAngle && playerAngle > -visionAngle) // If player is within bounds of LOS
        {
            
            RaycastHit hit; //Creates a hit variable for the Raycast
            // Normalise the direction

            Vector3 angleToPlayer = player.position - transform.position;
            float distance = angleToPlayer.magnitude;
            Vector3 direction = angleToPlayer / distance;



          

            if (Physics.Raycast(chaser.position, direction, out hit, visionLength)) // Checks if the Ray has hit something
            {
                HasHitPlayer(hit); // Checks if the player has been hit
                Debug.DrawRay(chaser.position, direction * hit.distance, Color.green);
            } else
            {
                Debug.DrawRay(chaser.position, direction * visionLength, Color.green);
            }
        }

       //Draw the LOS rays
        Debug.DrawRay(chaser.position, LOSAngleRight * visionLength, Color.red);
        Debug.DrawRay(chaser.position, LOSAngleLeft * visionLength, Color.red);

    }

    

    void HasHitPlayer(RaycastHit hit)
    {
        if (hit.collider.tag == "Player") //Checks if the Navigation rays have hit the player
        {
            Debug.Log("Player came into our line of sight.");
            ChaseTransition(); // Transitions to Chase
        }
    }
    

    void ChasingLOSCheck()
    {
        //Normalise the Angle to Player
        Vector3 angleToPlayer = player.position - transform.position;
        float distance = angleToPlayer.magnitude;
        Vector3 direction = angleToPlayer / distance;


        RaycastHit hit;

        if (Physics.Raycast(chaser.position, direction, out hit, searchDistance)) 

        {
            if(hit.collider.tag != "Player") // If there is no more LOS between chaser and player, go into Searching mode
            {
                SearchTransition();
            }
            
        } else
        {
            SearchTransition();
        }
        Debug.DrawRay(chaser.position, direction * searchDistance, Color.red);

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

    public void Jump()
    {
        chaser.GetComponent<Rigidbody>().AddForce(300, 400, 300, ForceMode.Impulse);
        Debug.Log("Jumping");
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
