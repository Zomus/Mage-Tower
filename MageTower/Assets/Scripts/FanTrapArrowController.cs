using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanTrapArrowController : MonoBehaviour {

    // Use this for initialization

    private Renderer rend; 



    void Start () {

		if(transform.parent.name == "SampleTrap")//ITS TAG IS NOT SAMPLETRAP
        {
            rend = GetComponent<Renderer>();
            rend.enabled = false;
        }
        
    }
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetMouseButtonDown(0))
        {

            Destroy(this.gameObject);
        }
    }
}
