using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

public class TileController : MonoBehaviour {
    /* ROLE:
     * Manages information about a particular tile.
     * Attached to GameObjects of the Tile Prefab.
     */

    //CLASS VARIABLE

    public int trapType;
    //type of trap placed on this block

    //TRAP TYPES - using integers to represent the type of trap placed on the tile
    public const int NO_TRAP = 0;
	public const int SPRING_TRAP = 1;
	public const int SAW_TRAP = 2;
    //NOTE: const variables are always static in C#

	public TrapController trapRef;
	//reference to the trap that has been placed on this tile

	public bool ready;
    //whether trap has been triggered (true = ready to kill another enemy, false = not ready to kill and must be reset)

    Material defaultMat;
    //default material of the tile (stored as soon as it is instantiated)


    //CLASS FUNCTIONS

    void Start () {
		trapType = NO_TRAP;
		//no trap is placed by default

		tag = "Trappable";
		//allow the block to be trappable by giving it the tag

		defaultMat = getChild("Top").GetComponent<MeshRenderer>().material;
        //obtain default material as the material of the top side (used to restore default material after highlighting with another material)

        ready = false;
        //originally not ready
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            //collider in tile hits an enemy

            float landSpeed = other.GetComponent<Rigidbody>().velocity.y;
            //obtain the velocity of the enemy as it hits the ground

            if (landSpeed < 0f)
            {
                //enemy was falling (not rising)

                //Debug.Log("Landed at a speed of: " + landSpeed);
                //print out how fast it landed on the ground

                if (landSpeed < -10f)
                {
                    //velocity was faster than 9 as it hits the ground
                    other.GetComponent<EnemyController>().death();
                    //kill enemy from fall damage
                }
                else
                {
                    other.GetComponent<EnemyController>().landed((int)transform.position.y);
                    //let the enemy know that it has landed
                }
            }
        }
    }

    public void placeTrap(int type)
    {
        /* PARAMETERS:
         * type = type of the trap that is placed
         * DO:
         * Places a trap on this tile
         */

        trapType = type;
        //mark the trap type of the block to the type of the trap

        ready = true;
        //note that the trap is ready to trap an enemy
        Vector3 dropLocation = transform.position;
        //define where the trap is to be placed
        GameObject tempTrap = Instantiate(GameController.main.trapClasses[trapType].prefab, dropLocation, Quaternion.identity, GameController.main.trapContainer.transform) as GameObject;
        //spawn the trap at the dropLocation inside the trapContainer
        
        trapRef = tempTrap.GetComponent<TrapController>();
        //obtain a reference to the trap that is placed on this tile

        //trapRef.setTileReference(this);
        //pass a reference of this TileController object to the placed tile
    }

    public Boolean resetTrap()
    {
        /* DO:
         * Resets a trap placed on this tile.
         * RETURN VALUE:
         * Returns whether a trap is successfully reset (will fail if there is no trap).
         */
        if(trapType != 0)
        {
            //there is a trap on this tile

            ready = true;
            //note that the trap is ready to trap an enemy again
            trapRef.resetTrapAnimation();
            //reset animation for the trap

            return true;
        }
        else
        {
            //no trap on this tile
            return false;
        }
    }

	public void assignNewMaterial(string name, string surface = "Top"){
        /* PARAMETERS:
         * name = name of material to be assigned to a surface of this tile
         * surface = name of surface to be assigned a material (if none is specified, assign to top side)
         * DO:
         * Assigns a material to a specific surface of the tile.
         * If 'name' is "Default", restore the surface to the stored default material.
         */ 
		if(name != "Default"){
            //if 'name' is not "Default"

			Material newMat = Resources.Load("Material/" + name, typeof(Material)) as Material;
			//look up and obtain the newly assigned material in Resources folder

            if(newMat != null)
            {
                //material was found

                getChild(surface).GetComponent<MeshRenderer>().material = newMat;
                //assign new material
            }
            else
            {
                //material was not found

                Debug.Log("ERROR: Material \"" + name + "\" was not found.");
                //print out an error message saying that the material cannot be found
            }
        }
        else{
            //'name' is default

			getChild(surface).GetComponent<MeshRenderer>().material = defaultMat;
			//assign default material
		}
	}

	Transform getChild(string name){
		/* PARAMETERS:
         * name = name of the child object
         * RETURN VALUE:
         * Returns the Transform of a child object with the name 'name'.
         */

		foreach (Transform child in transform){ //for loop searches through the parent GameObject's Transform
			if (child.name == name){ //if names match
				return child;
				//return that the child's reference
			}
		}

		Debug.Log("Error: Child cannot be found.");
		return null;
	}
}