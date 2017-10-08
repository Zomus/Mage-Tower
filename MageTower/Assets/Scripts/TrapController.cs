using UnityEngine;
using System.Collections;

public class TrapController : MonoBehaviour {
    /* ROLE:
     * Manage the trap that it is attached to.
     * Attached to any Trap Prefab (i.e. SawTrap, SpringTrap)
     */

    //CLASS VARIABLES

    //COMPONENTS
    Animator anim;

	TileController tileRef;
    //reference to the tile that this trap is attached to

    //CLASS FUNCTIONS
    void Awake () {
        //DEFINE COMPONENTS
        anim = transform.Find("Model").GetComponent<Animator>();
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            //hitting an enemy

            EnemyController ec = other.GetComponent<EnemyController>();
            //obtain reference to the EnemyController component in enemy

            if (ec.dead)
            {
                //enemy is dead

                return;
                //do nothing
            }

            anim.SetBool("Triggered", true);
            //triger trap animation

            if (tileRef.trapType == TileController.SPRING_TRAP)
            {
                //this is a spring trap

                if (tileRef.ready)
                {
                    //trap is ready to be sprung
                    if (!ec.levitated)
                    {
                        //if enemy is on the ground

                        ec.lifted();
                        //let the enemy know that it is in the air
                    }
                    other.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-1f, 1f), 12f, Random.Range(-1f, 1f));
                    //fling the enemy up with a force

                    tileRef.ready = false;
                    //trap is no longer ready once sprung
                }
                else
                {
                    //trap is already sprung
                    if (ec.levitated)
                    {
                        //enemy landing on a trap

                        other.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-1f, 1f), 12f, Random.Range(-1f, 1f));
                        //fling the enemy up with a force

                        anim.SetTrigger("Relaunch");
                        //triger trap animation again
                    }
                }
            }

            if (tileRef.trapType == TileController.SAW_TRAP)
            {
                //this is a saw trap

                ec.death();
                //kills the enemy immediately
                tileRef.ready = false;
                //trap is no longer ready once sprung
            }
        }
    }

    public void setTileReference(TileController tile){
        /* PARAMETERS:
         * tile = reference of the tile that this trap is attached to
         * DO:
         * Receives the reference of the tile that this trap is attached to.
         */
		tileRef = tile;
	}

	public void resetTrapAnimation(){
        /* DO:
         * Resets the trap animation back to its ready state.
         */
		anim.SetBool("Triggered", false);
        //set trap animation back to ready state
	}

	
}
