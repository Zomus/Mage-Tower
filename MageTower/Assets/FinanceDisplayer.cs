using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FinanceDisplayer : MonoBehaviour {
    /* ROLE:
     * Updates the financial information display on the top left corner of the canvas.
     */

    //CLASS VARIABLES

    //COMPONENTS
    Text txt;
    
    //IMPORTANT GAME OBJECTS
    GameController gc;
    
    //CLASS FUNCTIONS

	void Start () {
        //DEFINE COMPONENTS
        txt = GetComponent<Text>();
        
        gc = GameController.main;
        //define the GameController (to obtain finacial information)
	}
	
	void Update () {
        txt.text = "Anima: " + gc.finance;
	}
}
