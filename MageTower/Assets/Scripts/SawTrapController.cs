using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawTrapController : TrapController {

    public override void setMaterial(bool def = false)
    {
        //EDIT: Saw trap model is still missing
    }

    public override void clickTrap()
    {
        ready = true;
        //reset trap back to ready state
        anim.SetBool("Triggered", false);
        //triger trap animation
    }

    void OnTriggerEnter(Collider other)
    {
        if (transform.parent.name == "SampleTrap")
        {
            //if it is just a sample trap
            return;
            //ignore any collision
        }
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

            if (ready)
            {
                ec.death();
                //kills the enemy immediately
                ready = false;
                //trap is no longer ready once sprung
            }
        }
    }
}
