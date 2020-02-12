using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddGroundButton : MonoBehaviour
{
    // button that the player will stand on. Once they do, it will send code to the target obj which will add ground to the area for the player

    public GameObject target;
    public int buttonNum;
    bool ButtonPressAllowed = true;

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player") && ButtonPressAllowed)
        {
            target.GetComponent<AddGround>().ButtonActivated(buttonNum);
            ButtonPressAllowed = false;
        }
    }
}
