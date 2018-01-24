using UnityEngine;
using System.Collections;

public class LadderController : MonoBehaviour {
    /* ROLE:
     * Allow for easy accessible properties of a ladder (top and bottom floor).
     * Attached to GameObjects with the tag "Ladder".
     */ 
	public int bottomFloor;
    //floor of the bottom end of the ladder
	public int topFloor;
    //floor of the top end of the ladder
}

//Change to allow pushing to git