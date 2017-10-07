using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{   /* ROLE:
	 * Manages stats and creates functionality for enemies
	 * Attached to GameObjects of the Prefab Enemy
	 */

    //CLASS VARIABLES

    //COMPONENTS
    Rigidbody rb;
    CapsuleCollider collider;
    UnityEngine.AI.NavMeshAgent agent;
    Animator anim;
    //attached to child called Model

    //PREFABS
    public GameObject deathsplosion;
    //prefab of explosion

    const float heightOffset = -0.5f;
    //value used to offset the height of enemy when it finishes climbing a ladder

    //ENEMY STATES
    public bool dead = false;
    //whether this enemy is dead

    public bool levitated = true;
    //whether this enemy is in the air (not touching the ground/navigating navmesh)

    //NAVIGATION
    List<GameObject> destList = new List<GameObject>();
    //list of destinations that this character must traverse to reach its goal

    int nextDestIndex = 0;
    //the index of the character's next destination, stored in destList

    GameObject goal;
    //goal that this character wants to reach

    //CLIMBING
    public float climbSpeed;
    //speed at which the character climbs

	int currentFloor;
	//floor that this character is currently on

	int targetFloor;
	//floor that this character wants to reach

	int climbing = 0;
	//is this enemy currently climbing the ladder
	/*
	 * -1 = climbing down
	 *  0 = not climbing
	 *  1 = climbing up
	 */
    
	float climbTargetElevation;
    // y value to which the enemy should be at when it finishes climbing a ladder


    //CLASS FUNCTIONS

    void Start ()
	{
        //DEFINE COMPONENTS
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		rb = GetComponent<Rigidbody>();
		collider = GetComponent<CapsuleCollider>();
        anim = transform.Find("Badguy").GetComponent<Animator>();

        currentFloor = (int)transform.position.y;
        //define the floor that it spawned on by its y position

        agent.enabled = false;
		//disable the agent (assume it is spawned in the air, and does not navigate until it hits the ground)

		goal = GameController.main.goalCapsule;
        //set goal as dictated by the scene controller to the goal of this enemy

        targetFloor = (int)goal.transform.position.y;
        //set the floor of the final destination
    }
	
	void FixedUpdate ()
	{
		if(agent.isActiveAndEnabled && destReached(agent) && nextDestIndex < destList.Count){
            //agent is active AND agent is finished navigating AND has another destination (i.e. at a ladder)

            GameObject nextDestination = destList[nextDestIndex];
            //obtain the next destination in the destination list

            if (targetFloor != currentFloor)
            {
                rb.isKinematic = true;
                //not on the same floor as the goal
                if (targetFloor > currentFloor)
                {
                    //if the goal is above the enemy

                    climbing = 1;
                    //climb up

                    if (nextDestination.tag == "Ladder")
                    {
                        //if the next destination is a ladder
                        climbTargetElevation = nextDestination.GetComponent<LadderController>().topFloor;
                        //set the target elevation to the top of the ladder
                    }
                }
                else
                {
                    //if the goal is below the enemy

                    climbing = -1;
                    //climb down

                    if (nextDestination.tag == "Ladder")
                    {
                        //if the next destination is a ladder
                        climbTargetElevation = nextDestination.GetComponent<LadderController>().bottomFloor;
                        //set the target elevation to the top of the ladder
                    }
                }

                anim.SetInteger("State", 3);
                //set animation to climbing

                rb.useGravity = false;
                //turn off gravity

                agent.enabled = false;
                //turn off navigation
            }

        }

		if(climbing != 0){ //Climbing
            transform.Translate(0f, climbing * climbSpeed * Time.deltaTime, 0f);

			if((climbing > 0 && gameObject.transform.position.y + heightOffset >= climbTargetElevation) ||(climbing < 0 && gameObject.transform.position.y + heightOffset <= climbTargetElevation)){
                //climbing up AND at or above targetElevation (including offset) OR climbing down AND at or below targetElevation (including offset)

				climbing = 0;
				//not climbing

				currentFloor = (int)climbTargetElevation;
				//change the currentFloor after climbing a ladder

				agent.enabled = true;
				//turn on navagent component

				nextDestIndex++;
				//set next destination to the next one on the list
				agent.destination = destList[nextDestIndex].transform.position;
				//set the agent's destination to the upcoming destination

				anim.SetInteger("State", 1);
                //set animation to walking
            }
        }

		if(transform.position.y < -1){
            //if position is below the ground
			death();
            //kill this enemy
		}
	}

    void setNewDest(GameObject newGoal){
        /* PARAMETERS:
         * newGoal = new goal that the enemy is trying to get to
         * DO:
         * Sets the position of parameter newGoal as the new final destination of this enemy.
         * If there are several paths to the newGoal, selects a random path to navigate.
         */

		targetFloor = (int)newGoal.transform.position.y;
		//set the new targetfloor to the same floor as the new goal

		List<List<GameObject>> allPaths = generateAllPaths(currentFloor, targetFloor, newGoal);
		//obtain all the paths that can be taken to the goal

		if (allPaths.Count == 0) {
			//if there are no paths

			agent.destination = transform.position;
			//set its destination to where it already is (stops the agent from moving)

			return;
			//don't run other code that searches and sets new paths
		}

		int selectedIndex = (int)(Random.value * allPaths.Count);
		//generates a random number for the index of a path to select it

		destList = allPaths[selectedIndex];
		//select a random path from all paths generated

		nextDestIndex = 0;
		//index of next destination in the selected path (destList)

		agent.updatePosition = true;
        //allow the enemy to change position as it navigates

		agent.updateRotation = true;
        //allow the enemy to change rotation as it navigates

        agent.destination = destList[0].transform.position;
		//set the next destination to the next position on the destination list
	}

	List<List<GameObject>> generateAllPaths(int selectedFloor, int finalFloor, GameObject finalGoal){
		/*	PARAMETERS:
         *	selectedFloor = floor at beginning of navigation
		 * 	finalFloor = floor that contains the goal
		 * 	finalGoal = the goal
         * 	DO:
         * 	Generates a list of possible paths to the goal.
         * 	RETURN VALUE:
         * 	Returns a list that contains ladders and a goal at the end of the list.
         * 	If no possible path was found, returns null.
         * 	
         * 	NOTE:
         * 	This is a recursive function that generates all paths to the next floor, to which then connects all paths generated on the next floor.
         * 	
         * 	ERRORS:
         * 	This function only searches above when the goal is above, and only searches below when the goal is below.
         * 	Thus, if there is a path that requires climbing down, then climbing up, it will not be generated.
         * 	
         * 	There is a logic error where an infinite path can be generated when a ladder connects above and below the goal.
         * 	An infinite path will be generated as the character will continuously go up and down the same ladder, which will most likely crash the program.
		 */

		List<List<GameObject>> possiblePaths = new List<List<GameObject>>();
		//initializes a list of possible paths to be sent back as the return value

		if (targetFloor > selectedFloor){
			//goal is on a higher floor than the starting floor

			//SEARCH FOR LADDERS ON THS FLOOR THAT GO UP
			GameObject ladders = GameController.main.ladderContainer;
            //obtain reference of the ladder container
			List <GameObject> possibleLadders = new List<GameObject>();
			//initializes a list of possible ladders that go up

			foreach(Transform child in ladders.transform){
                //obtains every ladder in the ladder container
				if(child.tag == "Ladder"){
                    //if the object found is a ladder (has the tag "Ladder")
					if(child.GetComponent<LadderController>().bottomFloor == selectedFloor){
						//if the selected ladder's bottom is on this floor
						possibleLadders.Add(child.gameObject);
                        //add the ladder as a possible ladder to take upwards
					}
				}
			}
            
			foreach(GameObject ladder in possibleLadders){
                //loop through every ladder that goes up on this floor

				List<List<GameObject>> allPossiblePathsAhead = generateAllPaths(ladder.GetComponent<LadderController>().topFloor, finalFloor, finalGoal);
                //recursively generate all paths at the top of the ladder that lead to the goal

				foreach(List<GameObject> path in allPossiblePathsAhead){
                    //repeats for every possible path at the top of the ladder

					List<GameObject> possiblePath = new List<GameObject>();
                    //initializes a single possible path

					possiblePath.Add(ladder);
					possiblePath.AddRange(path);
                    //add the ladder then the possible path at the top of the ladder together to form a single possible path from this floor

					possiblePaths.Add(possiblePath);
                    //add the combined possible path to all possible paths from this floor
				}
			}
		}

		else if (targetFloor < selectedFloor){
            //goal is on a lower floor than the starting floor

            //SEARCH FOR LADDERS ON THS FLOOR THAT GO DOWN
            GameObject ladders = GameController.main.ladderContainer;
            //obtain reference of the ladder container
            List<GameObject> possibleLadders = new List<GameObject>();
            //initializes a list of possible ladders that go down

            foreach (Transform child in ladders.transform)
            {
                //obtains every ladder in the ladder container
                if (child.tag == "Ladder")
                {
                    //if the object found is a ladder (has the tag "Ladder")
                    if (child.GetComponent<LadderController>().topFloor == selectedFloor)
                    {
                        //if the selected ladder's top is on this floor
                        possibleLadders.Add(child.gameObject);
                        //add the ladder as a possible ladder to take downwards
                    }
                }
            }

            //For each possible ladder path, add the ladder to the front of the path and add all combined paths to the possiblePaths
            foreach (GameObject ladder in possibleLadders){
                //loop through every ladder that goes down on this floor
                List<List<GameObject>> allPossiblePathsAhead = generateAllPaths(ladder.GetComponent<LadderController>().bottomFloor, finalFloor, finalGoal);
                //recursively generate all paths at the bottom of the ladder that lead to the goal

                foreach (List<GameObject> path in allPossiblePathsAhead){
                    //repeats for every possible path at the top of the ladder

                    List<GameObject> possiblePath = new List<GameObject>();
                    //initializes a single possible path

                    possiblePath.Add(ladder);
					possiblePath.AddRange(path);
                    //add the ladder then the possible path at the top of the ladder together to form a single possible path from this floor

                    possiblePaths.Add(possiblePath);
                    //add the combined possible path to all possible paths from this floor
                }
            }
		}
		else{
            //goal is on this floor
			List<GameObject> possiblePath = new List<GameObject>();
            //initializes a single possible path

			possiblePath.Add(finalGoal);
            //add only the goal to this path

			possiblePaths.Add(possiblePath);
			//just add the goal instead of looking for ladders
		}

		return possiblePaths;
        //return the path
	}

	bool destReached(UnityEngine.AI.NavMeshAgent mNavMeshAgent){
        /* PARAMETERS:
         * mNavMeshAgent = agent that this function is concerned with
         * RETURN VALUE:
         * Returns whether mNavMeshAgent has finished navigating to its goal.
         * 
         * EDIT:
         * Will be later changed to a function with no parameters, or a public function in a separate class so it can be called anywhere.
         */
		if (!mNavMeshAgent.pathPending){
			if (mNavMeshAgent.remainingDistance <= mNavMeshAgent.stoppingDistance){
				if (!mNavMeshAgent.hasPath || mNavMeshAgent.velocity.sqrMagnitude == 0f){
					return true;
				}
			}
		}
		return false;
	}

	public void lifted(){
		/* DO:
         * Called when this enemy is lifted off the ground (either by traps or by the magehand).
         * Changes enemy states so physics work correctly in the air.
         */

		levitated = true;
        //marked as levitated

		anim.SetInteger("State", 0);
		//change animation to idle (EDIT: change to flailing)

		rb.useGravity = true;
		//apply gravity

		agent.enabled = false;
		//stop navigating
	}

	public void landed(int elevation){
        /* PARAMETER:
         * elevation = height of the location where this enemy landed
         * DO:
         * Called when enemy lands on a tile.
         * Change enemy states so physics work correctly on the ground.
         */ 
		if(levitated){
            //if it was in the air

			levitated = false;
            //mark as not levitated

			anim.SetInteger("State", 1);
			//set animation to walking

			rb.useGravity = false;
			//stop applying gravity

			agent.enabled = true;
			//begin navigating

			currentFloor = elevation;
            //set the floor that it landed on as the currentFloor

			setNewDest(goal);
			//set new destination for the NavMeshAgent
		}
	}

	public void death(){
        /* DO:
         * Called as the enemy dies.
         * Changes states and activates explosion effect.
         */ 
		dead = true;
		//mark as dead

		agent.enabled = false;
		//stop navigating

		anim.SetInteger("State", -1);
		//play fall animation

		StartCoroutine(disableCollider(0.2f));
        //disable the collider after 0.2 seconds

		StartCoroutine(waitForExplode(1f));
		//explode after 1 second
	}

	private IEnumerator disableCollider(float delay){
        /* PARAMETER:
         * delay = amount of times it waits before executing the function
         * DO:
         * Disables the collider such that other objects do not interact with dead enemy.
         */ 
		yield return new WaitForSeconds(delay);
        //wait for delay seconds

		rb.constraints = RigidbodyConstraints.FreezePosition;
		//freeze position (cannot move)

		collider.enabled = false;
		//disable collider
	}

	private IEnumerator waitForExplode(float delay){
        /* PARAMETER:
         * delay = amount of times it waits before executing the function
         * DO:
         * Creates a particle effect for an explosion when the enemy is destroyed.
         */
        yield return new WaitForSeconds(delay);
        //wait for delay seconds

		GameController.main.finance += 3;
        //add 3 to the player's money

		rb.constraints = RigidbodyConstraints.FreezePosition;
		//freeze position

		GameObject tempExplosion = Instantiate (deathsplosion, transform.position, Quaternion.Euler(-90f, 0f, 0f), GameController.main.particleContainer.transform) as GameObject;
        //create a new explosion
        tempExplosion.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
        //scale down the explosion to half
		Destroy (this.gameObject);
        //destroy this enemy as it is explodes
	}

	public void attackWizard(){
        /* DO:
         * Called when the goal is reached.
         * Begin attacking the wizard when it gets in range.
         */
        agent.enabled = false;
		//stop navigating

		anim.SetInteger("State", 2);
		//change animation to attacking
	}
	
}
	

