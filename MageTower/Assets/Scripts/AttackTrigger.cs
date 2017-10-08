using UnityEngine;
using System.Collections;

public class AttackTrigger : MonoBehaviour {
	/* ROLE:
     * TriggerCollider that allows enemies to begin attacking the wizard.
     * Attached to the goal.
     */
    
    //CLASS FUNCTION
	void OnTriggerEnter (Collider other) {
		if(other.tag == "Enemy"){
            //if an enemy enters this area, it will begin attacking

			other.GetComponent<EnemyController>().attackWizard();
            //begin attack animation
			//EDIT: replace with attack function later
		}

	}
	

}
