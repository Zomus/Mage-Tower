using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleTrapController : MonoBehaviour {
    /* ROLE:
     * Manage the sample trap (trap that the hand is holding to show where it will be placed).
     * Attached to the sample trap.
     */

    //CLASS VARIABLE

    public GameObject samplePrefab;
    //sample model that is a child of this GameObject

    public int numberOfColliders = 0;
    //number of objects that the sample trap is colliding with

    List<Collider> collidedObjects = new List<Collider>();
    //make a list to track collided objects

    
    //CLASS FUNCTIONS
    public void convertType(int newTrapType)
    {
        /* PARAMETER:
         * newTrapType = trap type that the sample trap should switch to
         * DO:
         * Converts the sample trap to the new type.
         */
        if(samplePrefab != null)
        {
            Destroy(samplePrefab);
        }

        if(newTrapType != 0)
        {
            samplePrefab = Instantiate(GameController.main.trapClasses[newTrapType].prefab, transform) as GameObject;

            GameController.changeChildrenLayers(samplePrefab, 8/*Ignore Raycast*/);
            //change the layers of everything in the sample prefab to the ignore raycast layer
        }
    }

    public void teleport(Vector3 location)
    {
        /* PARAMETER:
         * location = location that the sample trap will move to.
         * DO:
         * Teleports the trap to the location specified in the parameter.
         */
        transform.position = location;
    }


    void OnTriggerEnter(Collider other)
    {
        if (!collidedObjects.Contains(other) && (other.transform.parent != null && other.transform.parent.name != "SampleTrap") && other.tag == "Trap")
        {
            //The list of collided objects does not already have this object AND the other object is not a sample trap AND the collided object is another trap (already placed trap)

            collidedObjects.Add(other);
            //adds traps to list when colliding with it
        }
    }

    void OnTriggerStay(Collider other)
    {
        OnTriggerEnter(other); //same as enter
    }


    void Update()
    {
        numberOfColliders = collidedObjects.Count; // this should give you the number you need
    }

    void FixedUpdate()
    {
        collidedObjects.Clear();
    }
}
