using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circuit : MonoBehaviour
{

    // like the key to the game that needs to be destroyed

    public int circuitNum;
    bool allowSend = true;


    public void sendConfirm() // Tell the locked door, that this circuit (= key) is solved
    {
        if (allowSend)
        {
            allowSend = false;
            GameObject.Find("lockedDoor").GetComponent<ExitDoor>().CheckCompletion(circuitNum);
            
        }
    }
}
