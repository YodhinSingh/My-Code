using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddGround : MonoBehaviour
{
    // Once the player stands on the 2 buttons, the ground is made active so the player doesnt have to jump on platforms anymore

    public GameObject floor;    // ground to be added
    public GameObject[] OldPlatforms = new GameObject[13];      // old platforms to get rid of once the player presses both buttons
    public Text[] count = new Text[4];  // UI in game to show count of buttons pressed. i.e. 1/2 or 0/2 etc.

    int countTotal = 0; // number of buttons pressed
    bool button1On = false; // button 1 is pressed?
    bool button2On = false; // button 2 is pressed?

    void Start()
    {
        count[0].text = "0/2";
        count[1].text = "0/2";
        count[2].text = "0/2";
        count[3].text = "0/2";
    }
    public void ButtonActivated(int ver)
    {
        if (ver == 1)
            button1On = true;
        else if (ver == 2)
            button2On = true;
        countTotal++;
        count[0].text = countTotal + "/2";
        count[1].text = countTotal + "/2";
        count[2].text = countTotal + "/2";
        count[3].text = countTotal + "/2";
        activateGround();
    }

    void activateGround()
    {
        if (button1On && button2On)
        {
            floor.SetActive(true);
            count[0].text = "";
            count[1].text = "";
            count[2].text = "";
            count[3].text = "";
            for (int i = 0; i < OldPlatforms.Length; i++)
            {
                OldPlatforms[i].SetActive(false);
            }
        }
    }


    
}
