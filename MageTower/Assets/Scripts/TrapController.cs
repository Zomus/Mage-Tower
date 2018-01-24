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

    public SpriteRenderer areaBase;
    //area marked by trap
    /* Trap states:
     * green = can be set
     * grey = cannot be set due to lack of cash
     * red = cannot be set due to location restrictions
     * yellow = selecting an already set trap
     */

    //PUBLIC VARIABLES
    public bool ready;
    //whether this trap is ready to be set on another enemy
    
    public int value;
    //type of trap (mapped above)

    

    //CLASS FUNCTIONS
    void Awake()
    {
        //DEFINE COMPONENTS
        anim = transform.Find("Model").GetComponent<Animator>();
        areaBase = transform.Find("Base").GetComponent<SpriteRenderer>();
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

    public void changeBaseColor(Color clr)
    {
        areaBase.color = clr;
    }
}
