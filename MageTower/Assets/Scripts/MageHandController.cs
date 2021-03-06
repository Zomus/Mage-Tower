﻿using UnityEngine;
using System.Collections;

public class MageHandController : MonoBehaviour {

	Animator anim;
    //int grabHash = Animator.StringToHash("Grab");

    public Vector3 offset;
    //offset between the coordinate offset of the (0, 0, 0) of the hand and its true center

    public EnemyController heldEnemy;

    // Use this for initialization
    void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);
		//point of the mouse using screen coordinates

		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
        //point of the mouse in game coordinates

        gameObject.transform.position = curPosition + offset;
		//change the gameObject's position to follow the mouse

		if(gameObject.transform.position.y < 0f){
			gameObject.transform.Translate(new Vector3(0f, -gameObject.transform.position.y, 0f));
			anim.SetBool("Walking", true);
		}else{
			anim.SetBool("Walking", false);
		}

		if(Input.GetMouseButtonDown(0)){ //when mouse is pressed
			anim.SetBool("Grabbing", true);
			//change the animation to grabbing state
		}

		if(Input.GetMouseButtonUp(0)){ //when mouse is released
			anim.SetBool("Grabbing", false);
			//change the animation to release state
		}
		//Debug.Log("Grabbing: "+anim.GetBool("Grabbing"));
	}
    // Targets point when mouse left click is held.
}



/*
if (Input.GetMouseButtonDown(0))
	Debug.Log("Pressed left click.");

if (Input.GetMouseButtonDown(1))
	Debug.Log("Pressed right click.");

if (Input.GetMouseButtonDown(2))
	Debug.Log("Pressed middle click.");
*/