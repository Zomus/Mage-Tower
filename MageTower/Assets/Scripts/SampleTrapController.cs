using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleTrapController : MonoBehaviour {

    public int colliding = 0;

    public GameObject samplePrefab;
    //sample model that is a child of this GameObject
	
	public void convertType(int newTrapType)
    {
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
        transform.position = location;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "TrapObstacle")
        {
            colliding++;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "TrapObstacle")
        {
            colliding--;
        }
    }
}
