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
            samplePrefab = Instantiate(GameController.main.trapClasses[newTrapType].prefab, transform);
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
        if (!collidedObjects.Contains(other) && other.tag == "TrapObstacle")
        {
            collidedObjects.Add(other);
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
