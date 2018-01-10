using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour {

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
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
}
