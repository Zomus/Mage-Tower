using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringTrapController : TrapController {
    public override void setMaterial(bool def = false)
    {
        if (def)
        {
            GameController.setMaterialOfChild(transform, "Pad", "unnamed");
            GameController.setMaterialOfChild(transform, "Spring", "unnamed");
            //Revert to standard unnamed material
        }
        else
        {
            GameController.setMaterialOfChild(transform, "Pad", "PlaceError");
            GameController.setMaterialOfChild(transform, "Spring", "PlaceError");
            //Set the material to PlaceError
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(transform.parent.name == "SampleTrap")
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
                //trap is ready to be sprung
                if (!ec.levitated)
                {
                    //if enemy is on the ground

                    ec.lifted();
                    //let the enemy know that it is in the air
                }
                other.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-1f, 1f), 12f, Random.Range(-1f, 1f));
                //fling the enemy up with a force

                ready = false;
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
    }

}
