using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    const int scaleShot = 1;    // the ammo type of the bullet, scale makes objs grow/shrink
    const int speedShot = 2;    // makes obj fast/slow
    const int hackShot = 3;     // modifies obj state
    public int ammoType = 1;    // what ammo type this bullet will be set in other script when instantiated


    private void OnCollisionEnter(Collision col)    // bullets interact with various objs differently and modify their properties (size, speed, is it active, etc)
    {
        if (col.gameObject.CompareTag("Interact"))  // Change an interactable object's properties
        {
            col.gameObject.GetComponent<InteractableBox>().changeVar(ammoType);
        }
        else if (col.gameObject.CompareTag("Puz1Button") && ammoType == 3)  // change a puzzle 1 obj properties
        {
            col.gameObject.GetComponent<Puzzle1Button>().changePlatforms();    
        }
        else if (col.gameObject.CompareTag("Circuit") && ammoType == 3)     // destroy a circuit and send confirmation that 1/3 has been destroyed
        {
            col.gameObject.GetComponent<Circuit>().sendConfirm();
        }
        else if (col.gameObject.CompareTag("ResetPuzzle3B") && ammoType == 3)       // reset the 3rd puzzle code
        {
            col.gameObject.GetComponent<ResetPuz3Button>().ResetPuz = true;
        }
        else if (col.gameObject.CompareTag("InteractSpin") && ammoType == 2)    // change rotation speed of platform
        {
            col.gameObject.GetComponent<InteractablePlatformSpin>().ChangeRotationSpeed();
        }

        Destroy(gameObject);

    }

}
