using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanTrapController : TrapController
{

    public float FanForce;

    private Vector3 FanForceVector;

    private void Start()
    {
        FanForceVector = new Vector3(0.0f, 0.0f, -FanForce);
    }

    public override void setMaterial(bool def = false)
    {
        if (def)
        {
            //GameController.setMaterialOfChild(transform, "Pad", "unnamed");
            //GameController.setMaterialOfChild(transform, "Spring", "unnamed");
            //Revert to standard unnamed material
        }
        else
        {
            //GameController.setMaterialOfChild(transform, "Pad", "PlaceError");
            //GameController.setMaterialOfChild(transform, "Spring", "PlaceError");
            //Set the material to PlaceError
        }
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
                //trap is ready to be sprung

                //apply a force in the -z direction on the enemy to push it back
                other.GetComponent<Rigidbody>().AddForce(FanForceVector, ForceMode.Impulse);

            }

        }
    }

}
