using UnityEngine;
using System.Collections;

public class TrapController : MonoBehaviour
{
    /* ROLE:
     * Manage the trap that it is attached to.
     * Attached to any Trap Prefab (i.e. SawTrap, SpringTrap)
     */

    //CLASS VARIABLES

    //STATIC CLASS VARIABLES

    //TRAP TYPES - using integers to represent the type of trap
    public const int SPRING_TRAP = 1;
    public const int SAW_TRAP = 2;
    public const int FAN_TRAP = 3;

    //COMPONENTS
    public Animator anim;

    //PRIVATE VARIABLES
    public int value;
    //type of trap (mapped above)

    public bool ready;
    //whether this trap is ready to be set on another enemy

    //CLASS FUNCTIONS
    void Awake()
    {
        //DEFINE COMPONENTS
        anim = transform.Find("Model").GetComponent<Animator>();
    }

    public void resetTrapAnimation()
    {
        /* DO:
         * Resets the trap animation back to its ready state.
         */
        anim.SetBool("Triggered", false);
        //set trap animation back to ready state
    }
    public virtual void setMaterial(bool def = false)
    {
        Debug.Log("WARNING: This function should be overwritten by a sub-class");
    }

}
