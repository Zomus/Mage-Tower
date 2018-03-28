using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FanTrapController : TrapController
{
    public float FanForce;
    //magnitude of the pushing force of the fan's wind

    public float cameraAngle;
    //Main Camera's Transform.Rotation x value (degrees). This is manually given to the fan trap prefab.
    //this can be implemented far better than it is currently

    private Vector3 FanForceVector;
    //vector of the pushing force of the fan's wind

    private int spawnStage;
    //0 = fan trap selected, 1 = fan trap placed but rotation is not defined, 2 = fan trap placed and rotation is defined

    private Boolean setBool;
    //false if rotation not set; true if rotation is set

    private float angle;
    //angle of the fan

    private int fanActiveTimerCounter;
    //how many frames since the fan has last been set/reset

    private void Start()
    {

        if ((transform.parent.tag == "SampleTrap" && GameObject.FindGameObjectsWithTag("SampleTrap").Length > 1)
            || GameObject.FindGameObjectsWithTag("FanTrapArrow").Length > 1)
        {


            //if the FanTrap is a child of the SampleTrap, which is tagged "SampleTrap"

            Destroy(this.gameObject);
            //the fan trap will essentially not spawn
        }






        setBool = false;
        //FanTraps spawn without their rotation set
        spawnStage = 0;
        //FanTraps spawn being selected
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

    void OnMouseOver()
    {
        //called every frame the cursor hovers over the fan trap

        if (Input.GetMouseButtonDown(0))
        {
            //if the player left clicks the fan

            spawnStage = 1;
            //the player is now controlling the fan's rotation
        }
    }

    private void Update()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10f);
        //point of the mouse using screen coordinates

        Vector3 mouseOnGame = Camera.main.ScreenToWorldPoint(curScreenPoint);
        //point of the mouse on the plane of the camera

        float cameraAngleRad = cameraAngle * (Mathf.PI / 180f);
        //the main camera's rotation x value in radians from degrees

        mouseOnGame = new Vector3(mouseOnGame.x, 0.0f, (float)(mouseOnGame.z + mouseOnGame.y / Math.Tan(cameraAngleRad)));
        //extends the point of the mouse down onto the xz plane at y = 0
        //currently, the calculations will be wrong if roll and yaw are changed from default/ main camera rotation y or z != 0
        //the greater the difference between the fan's y and 0, the greater the calculations will be incorrect

        if (transform.parent.name != "SampleTrap" && setBool == false)
        {
            //if it is not just a sample trap and its rotation has not been set
  
            angle = 90f - (float)((180f / Math.PI) * Math.Atan2(mouseOnGame.z - transform.position.z, mouseOnGame.x - transform.position.x));
            //this is the standard angle (degrees) between the +z axis and the mouse, with the fan being the origin 

            transform.rotation = Quaternion.Euler(0, angle, 0);
            //set the fan's rotation to the angle calculated above, rotation is about the y axis

            if (Input.GetMouseButtonDown(0))
            {
                //if the user clicks the mouse then the trap will set its rotation

                setBool = true;
                //the trap's rotation has been permanently set

                fanActiveTimerCounter = 0;
                //the fan will now begin to blow
            }
        }
        if (setBool == true)
        {
            FanForce = 1000/((float)Math.Pow((10.0f * fanActiveTimerCounter * Time.deltaTime - 25.0f), 2) +80f);

            //FanForce = 1000/((10t-25)^2+80)
            Debug.Log("FanForce = " + FanForce);
            //FanForce as a function of time

            fanActiveTimerCounter++;
            //increment the counter;


            FanForceVector = new Vector3((float)Math.Sin((double)(angle * Math.PI / 180f)), 0.0f, (float)Math.Cos((double)(angle * Math.PI / 180f)));
            //the fan will now start blowing enemies away in the direction of its set rotation

            FanForceVector *= FanForce;
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

                other.GetComponent<Rigidbody>().AddForce(FanForceVector, ForceMode.Impulse);
                //apply the fan force to the enemy
            }
        }
    }
}
