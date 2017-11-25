using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	/* ROLE:
	 * Manages game stats (main character stats, spawning) and the various operations in the game
	 * Attached to Empty GameObject
	 * Only one operational GameController active at any time
	 */ 

	//CLASS VARIABLES

	//STATIC CLASS VARIABLES
	public static GameController main;
	//gives static reference to the controller so that the single operational GameController can be accessed from anywhere by referencing this class (without obtaining its instance reference)

	//PUBLIC CLASS VARIABLES

	//IMPORTANT GAME_OBJECTS
	public GameObject pauseMenu;
	//pause menu
	public GameObject enemyContainer;
	//parent GameObject that will contain all spawned enemies
	public GameObject trapContainer;
	//parent GameObject that contains all spawned traps
	public GameObject particleContainer;
	//parent GameObject that contains all spawned particle effects
	public GameObject ladderContainer;
    //parent GameObject that contains all ladders
    
    public GameObject mageHand;
    //mage hand GameObject

    public SampleTrapController sampleTrap;

	//PLAYER STATS
	public int wizardHpMax;
	//wizard's (player's) max HP
	public int wizardHp;
	//wizard's (player's) current HP
	public int finance = 0;
	//coins the player has collected

    
	//STAGE TIMER
	public bool stageTimerRunning;
	//whether the stage timer is counting
	public float stageTimer;
	//counts time when stage begins
	public int stageTime = 100;
	//total seconds until portal opens and player escapes

    
	//ENEMY SPAWNING
	public GameObject enemyPrefab;
	//Prefab (blueprint) of spawned enemies
	public int enemiesLeft;
	//enemies left to be spawned in this level (EDIT: So far to be unused)
	private float lastSpawnTime;
	//time at which the last enemy spawn occured since stage start
	public float spawnRate;
	//time before another enemy spawns

	public GameObject spawnCapsule;
	//capsule in game that specifies where enemies are spawned

	public GameObject goalCapsule;
    //capsule in game that specifies where enemies move towards



    //SPAWNING TRAPS

    public Material sampleMat;
    //default material of the trap as it is dropped

    public int trapType;
    /*Type of trap that is being placed
	 * 1 = Spring Trap
	 * 2 = Saw Trap
	*/
    
    public TrapClass[] trapClasses;
	//0th element is left undefined as 0 means no trap

	//RAYCASTING - for mouse over tile selection
	Ray ray;
	RaycastHit hit;

    [System.Serializable]
    public class TrapClass
    {
        //PURPOSE: Custom class that contains information about that type of trap.

        public GameObject prefab;
        //prefab that the trap is

        public int cost;
        //anima cost of the trap
     
    }

    //CLASS FUNCTIONS

    void Awake () {
		main = this;
        //attach static reference to main so that any object can reference main
        sampleTrap.convertType(trapType);
        //apply new type to the sampleTrap
    }

    void spawnEnemy(float xPos, float yPos, float zPos){
		/* PARAMETERS:
		 * xPos = x position where the tile will be spawned
		 * yPos = y position where the tile will be spawned
		 * zPos = z position where the tile will be spawned
		 * DOES:
		 * Creates a single enemy at (xPos, yPos, zPos) and load it into the game
		 */

		Instantiate(enemyPrefab, new Vector3(xPos, yPos, zPos), Quaternion.identity, enemyContainer.transform);
		//instantiates a new enemy at location specified inside the enemyContainer
	}

	void Update () {
		//KEYBOARD INPUT - allows selection of trap type
		if(Input.GetKeyDown(KeyCode.Alpha0)){
			//if 0 key is pressed
			trapType = 0;
            //no traps will be selected
            sampleTrap.convertType(trapType);
            //apply new type to the sampleTrap
		}
		else if(Input.GetKeyDown(KeyCode.Alpha1)){
			//if 1 key is pressed
			trapType = 1;
            //spring trap will be selected
            sampleTrap.convertType(trapType);
            //apply new type to the sampleTrap
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2)){
			//if 2 key is pressed
			trapType = 2;
            //saw trap will be selected
            sampleTrap.convertType(trapType);
            //apply new type to the sampleTrap
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            //if key 3 is pressed 
            trapType = 3;
            //fan trap will be selected
            sampleTrap.convertType(trapType);
            //apply new type to the sampleTrap
        }

        //LAYERING - allows selection of tile to place traps on
        int mask = ~(1 << 8);
		//Creates a layermask that allows the ray to pass through layer 8 (layer that contains the mage hand as to not select it)

		//CASTING THE RAY - to obtain the object that the player is pointing at
		GameObject castObject = castRayTarget(mask);
        //cast a ray based on mouse location to obtain a GameObject (through the mask)

        Vector3 castPoint = castRayPoint(mask);
        //cast a ray based on mouse location to obtain a point at which it hit something

        //SELECTION
        if (trapType != 0)
        {
            //trap placing mode
            
            if (castObject != null && castPoint != Vector3.negativeInfinity)
            {
                //run the following only if the ray is casted on an object (and that the point is not far off at negative infinity)

                sampleTrap.gameObject.SetActive(true);
                //allow trap to be seen

                sampleTrap.teleport(castPoint);
                //display trap at cast location
                
                if (castObject.tag == "Trappable" && sampleTrap.GetComponent<SampleTrapController>().numberOfColliders == 0)
                {
                    //if the object hit is marked as "Trappable" by a tag AND trap is selected
                    if (sampleTrap.samplePrefab != null)
                    {
                        sampleTrap.samplePrefab.GetComponent<TrapController>().setMaterial(true);
                    }

                    //PLACING TRAP
                    if (Input.GetMouseButtonUp(0))
                    {
                        //run this block upon clicking

                        if (sampleTrap.numberOfColliders == 0)
                        {
                            //colliding with 0 objects

                            if (finance - trapClasses[trapType].cost >= 0)
                            {
                                //if there is enough money to pay for the trap

                                placeTrap(trapType, castPoint);
                                //place a trapType type trap on the tile

                                finance -= trapClasses[trapType].cost;
                                //spend money to buy the trap
                            }
                            
                        }
                    }
                }
                else
                {
                    //not trappable
                    if(sampleTrap.samplePrefab != null)
                    {
                        sampleTrap.samplePrefab.GetComponent<TrapController>().setMaterial();
                    }
                }
            }
            else
            {
                sampleTrap.gameObject.SetActive(false);
            }
        }
        else
        {
            //trap type is 0 (killing enemies mode)
            if (castObject.tag == "Enemy")
            {
                //if the object found is an enemy

                if (Input.GetMouseButtonDown(0))
                {
                    //run this block upon clicking

                    EnemyController pickedEnemy = castObject.GetComponent<EnemyController>();
                    //obtain the EnemyController attached to the enemy

                    pickedEnemy.held();
                    //levitates enemy

                    mageHand.GetComponent<MageHandController>().heldEnemy = pickedEnemy;
                    //store the enemy that has been picked up inside the MageHandController
                }

                if (Input.GetMouseButtonUp(0) && mageHand.GetComponent<MageHandController>().heldEnemy != null)
                {
                    //run this block upon releasing

                    mageHand.GetComponent<MageHandController>().heldEnemy.released();
                    //release the enemy

                    mageHand.GetComponent<MageHandController>().heldEnemy = null;
                    //no enemy is being held again
                }
                
            }
        }


        
        
		//STAGE TIMER
		if(stageTimerRunning){
			//if the timer is counting
			stageTimer += Time.deltaTime;
			//count up the timer
		}

		//ENEMY SPAWNING
		if(stageTimer - lastSpawnTime > spawnRate){
            //time elapsed is enough to spawn another enemy

            Vector3 randomOffset = new Vector3(0f, 0f, 0f);/*Random.Range(-14f, 14f), 2f, -4f);*/
			//random offset to the spawn capsule to allow for random location spawning
			spawnEnemy(spawnCapsule.transform.position.x + randomOffset.x, spawnCapsule.transform.position.y + randomOffset.y, spawnCapsule.transform.position.z + randomOffset.z);
			//spawn the enemy
			lastSpawnTime = stageTimer;
			//set the last spawn time to now
		}

		//PAUSE MENU
		if(Input.GetKeyDown(KeyCode.P)){
			//if the 'p' key is pressed

			Time.timeScale = pauseMenu.activeInHierarchy ? 1 : 0;
			//depending on the pause menu's state, start/stop the flow of time

			pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
			//toggle state of the pauseMenu
		}
	}

	GameObject castRayTarget(int layerMask){
		/* PARAMETERS:
		 * layerMask = bit mask that filters out which layers should be ignored by the RayCast
		 * DOES:
		 * Casts a ray from the main camera to infinity at the mouse location
		 * RETURN VALUE:
		 * Returns the first object that the Ray hits; if no object is hit, returns null
		 */

		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		//casts a ray from camera to the point where the mouse is hovering over

		if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)){
			//Physics.Raycast returns true if hits and return the hit object as a RaycastHit --> something is hit
			return hit.collider.gameObject;
			//return the object that is hit
		}
		else{
			//if nothing was hit
			return null;
			//return null
		}
	}
    Vector3 castRayPoint(int layerMask)
    {
        /* PARAMETERS:
		 * layerMask = bit mask that filters out which layers should be ignored by the RayCast
		 * DOES:
		 * Casts a ray from the main camera to infinity at the mouse location
		 * RETURN VALUE:
		 * Returns the point at where the Ray hits; if no object is hit, returns a Vector3 containing negative infinity
		 */

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //casts a ray from camera to the point where the mouse is hovering over

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            //Physics.Raycast returns true if hits and return the hit object as a RaycastHit --> something is hit
            return hit.point;
            //return the object that is hit
        }
        else
        {
            //if nothing was hit

            return Vector3.negativeInfinity;
            //return a vector with negative infinity x, y, z
        }
    }

    void placeTrap(int trapType, Vector3 dropLocation)
    {
        GameObject tempTrap = Instantiate(trapClasses[trapType].prefab, dropLocation, Quaternion.identity, trapContainer.transform) as GameObject;
    }

    public static bool setMaterialOfChild(Transform obj, string child, string mat)
    {
        /* PARAMETERS:
         * obj = parent object to search through
         * mat = name of material in the Material folder inside the Resources folder in Assets
         * surface = name of the child object
         * DO:
         * Sets the material of all children of 'obj' with the name 'child' to 'mat'.
         * RETURN VALUE:
         * Returns true if at least the material on one object has been changed.
         */

        Transform searchSurface = getChild(obj, child);
        //surface with a name of the child

        if (searchSurface != null)
        {
            //surface to apply the material is found

            if (mat != "Default")
            {
                //if 'name' is not "Default"

                Material newMat = Resources.Load("Material/" + mat, typeof(Material)) as Material;
                //look up and obtain the newly assigned material in Resources folder

                if (newMat != null)
                {
                    //material was found
                    searchSurface.GetComponent<MeshRenderer>().material = newMat;
                    //assign new material to the surface

                    return true;
                    //material applied
                }
                else
                {
                    //material was not found

                    Debug.Log("ERROR: Material \"" + mat + "\" was not found.");
                    //print out an error message saying that the material cannot be found

                    return false;
                    //no material applied
                }
            }
            else
            {
                //'mat' is default

                //getChild(surface).GetComponent<MeshRenderer>().material = defaultMat;
                //assign default material

                return true;
            }
        }
        else
        {
            if (obj.childCount == 0)
            {
                //no children of the transform

                return false;
                //return that material was not applied
            }
            else
            {
                bool rv = false;
                //return value for whether a material was applied to any children

                foreach (Transform deepChild in obj)
                {
                    //for loop searches through the parent GameObject's Transform
                    rv = rv || setMaterialOfChild(deepChild, child, mat);
                    //set rv to true if any recursive calls return true
                    //rv remains true if it was already true, but a recursive call returns false
                }

                return rv;
            }
        }
    }

    public static Transform getChild(Transform obj, string name)
    {
        /* PARAMETERS:
         * name = name of the child object
         * RETURN VALUE:
         * Returns the Transform of a child object with the name 'name'.
         */

        foreach (Transform child in obj)
        { //for loop searches through the parent GameObject's Transform
            if (child.name == name)
            { //if names match
                return child;
                //return that the child's reference
            }
        }

        //Debug.Log("Error: Child cannot be found.");
        return null;
    }
}


