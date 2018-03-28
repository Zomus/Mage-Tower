using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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
     * 3 = Fan Trap
	*/
    
    public TrapClass[] trapClasses;
    //0th element is left undefined as 0 means no trap


    //UI Elements
    public Button sellButton;
    //button that allows the selected trap to be sold

    public GameObject lastHighlighted;
    //last highlighted game object (not selected but hovered over to show that it can be selected)
    
    public GameObject lastSelected;
    //last selected game object (usually a trap that can be sold)

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
            ChangeTrapType(0);

        }
		else if(Input.GetKeyDown(KeyCode.Alpha1)){
            //if 1 key is pressed
            ChangeTrapType(1);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2)){
            //if 2 key is pressed
            ChangeTrapType(2);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            //if key 3 is pressed 
            ChangeTrapType(3);
        }

        //LAYERING - allows selection of tile to place traps on
        int mask = ~(1 << 8);
        //Creates a layermask that allows the ray to pass through layer 8 (layer that contains the mage hand as to not select it)
        //CASTING THE RAY - to obtain the object that the player is pointing at

        GameObject castObject = null;
        //object that the raycast hits
        Vector3 castPoint = Vector3.negativeInfinity;
        //world space point that the raycast hits

        //they are given default values if the mouse is hovered over a UI element

        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            //If mouse is not hovering over a UI element
            castObject = castRayTarget(mask);
            //cast a ray based on mouse location to obtain a GameObject (through the mask)

            castPoint = castRayPoint(mask);
            //cast a ray based on mouse location to obtain a point at which it hit something
        }


        
        unhighlightLast();
        //unhighlight whatever trap was hovered over in last frame

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

                //Debug.Log(castObject.name);
                //Prints out the object that the ray  is hitting

                if (castObject.tag == "Ground" && sampleTrap.GetComponent<SampleTrapController>().numberOfColliders == 0)
                {
                    //if the object hit is marked as "Ground" by a tag AND trap is selected
                    if (sampleTrap.samplePrefab != null)
                    {
                        //sampleTrap.samplePrefab.GetComponent<TrapController>().setMaterial(true);

                        sampleTrap.samplePrefab.GetComponent<TrapController>().changeBaseColor(Color.green);
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

                        sampleTrap.samplePrefab.GetComponent<TrapController>().changeBaseColor(Color.red);
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
            //trap type is 0 (neutral mode - not placing down traps)

            if(castObject != null)
            {
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

                else if (castObject.tag == "Trap")
                {
                    highlightAsLast(castObject);
                    if (Input.GetMouseButtonUp(0))
                    {
                        selectAsLast(castObject);
                    }
                }
                else
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        diselectLast();
                        //not selecting anything, so diselect all
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonUp(0))
                {
                    diselectLast();
                    //not selecting anything, so diselect all
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

    public void ChangeTrapType(int newTrapType)
    {
        /* PARAMETERS:
         * newTrapType = trap type to be switched to
         * DO:
         * Changes the current trap being placed to the new type (where 0 means hand is selecting).
         */
        main.trapType = newTrapType;
        main.sampleTrap.convertType(newTrapType);
        //apply new type to the sampleTrap
    }

    GameObject castRayTarget(int layerMask){
        /* PARAMETERS:
		 * layerMask = bit mask that filters out which layers should be ignored by the RayCast
		 * DOES:
		 * Casts a ray from the main camera to infinity at the mouse location
		 * RETURN VALUE:
		 * Returns the first object that the Ray hits; if no object is hit, returns null
		 */
        Vector3 mp = Input.mousePosition;

        ray = Camera.main.ScreenPointToRay(mp);
        //casts a ray from camera to the point where the mouse is hovering over
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red);
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

        tempTrap.GetComponent<TrapController>().value = trapClasses[trapType].cost;
        finance -= trapClasses[trapType].cost;
        //spend money to buy the trap

        selectAsLast(tempTrap);
        //select the last placed trap as last

        if (trapType == 3)
        {
            //if the trap that was just placed was a fan trap
            trapType = 0;
            //no traps will be selected
            sampleTrap.convertType(trapType);
            //apply new type to the sampleTrap
        }
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

    private void unhighlightLast()
    {
        if (lastHighlighted == null) return;
        if(lastHighlighted.GetComponent<TrapController>() != null && lastHighlighted != lastSelected)
        {
            lastHighlighted.GetComponent<TrapController>().changeBaseColor(Color.clear);
        }
        lastHighlighted = null;
    }

    public void highlightAsLast(GameObject newHighlight)
    {
        unhighlightLast();
        lastHighlighted = newHighlight;
        if (lastHighlighted.GetComponent<TrapController>() != null && lastHighlighted != lastSelected)
        {
            lastHighlighted.GetComponent<TrapController>().changeBaseColor(Color.magenta);
        }
    }

    private void diselectLast()
    {
        if (lastSelected == null) return;
        if (lastSelected.GetComponent<TrapController>() != null)
        {
            lastSelected.GetComponent<TrapController>().changeBaseColor(Color.clear);
        }
        lastSelected = null;

        sellButton.gameObject.SetActive(false);
        //enable the sell button so that the trap can be sold
    }

    public void selectAsLast(GameObject newSelect)
    {
        diselectLast();
        lastSelected = newSelect;
        if(lastSelected.GetComponent<TrapController>() != null)
        {
            lastSelected.GetComponent<TrapController>().clickTrap();
            lastSelected.GetComponent<TrapController>().changeBaseColor(Color.yellow);
        }

        if (trapType == 0)
        {
            //not placing traps
            sellButton.gameObject.SetActive(true);
            //enable the sell button so that the trap can be sold
        }
    }

    public static void sellSelected()
    {
        TrapController tempTrap = main.lastSelected.GetComponent<TrapController>();
        if (tempTrap != null)
        {
            tempTrap.sell();
        }
       
    }


    public static void changeChildrenLayers(GameObject parentObject, int newLayer)
    {
        /* PARAMETERS:
         * gameObject = parent parentObject whose children is to be searched
         * newLayer = layer to switch all children gameobjects to
         * DO:
         * Changes all the gameobjects inside a parent object
         */
        
        Transform[] arr = parentObject.GetComponentsInChildren<Transform>();
        //obtains an array of parent objects

        for (int i = 0; i < arr.Length; i++)
        {
            arr[i].gameObject.layer = newLayer;
            //change the layer of the currently selected object
        }
    }
}


