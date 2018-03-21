using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanTrapArrowController : MonoBehaviour {

    void Start ()
    {
        //Fan Traps (and the arrows) are spawned as sample traps and clones in the TrapContainer
		if(transform.parent.parent.tag == "SampleTrap")
        {
            //if the FanTrapArrow is a child of a child (a FanTrap object) of the SampleTrap, which is tagged "SampleTrap"

            Destroy(this.gameObject);
            //the arrow will essentially not spawn
        }
    }
	
	// Update is called once per frame
	void Update ()
    {    
        if (Input.GetMouseButtonDown(0))
        {
            //if the mouse is held down

            Destroy(this.gameObject);
            //destroy the arrow because upon mouse 1 being clicked, the fan direction is set.
        }
    }
}
