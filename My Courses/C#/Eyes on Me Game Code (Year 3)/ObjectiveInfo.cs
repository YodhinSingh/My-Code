using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveInfo : MonoBehaviour
{

    // what to write on player's objective menu UI based on where the player is

    public Text Goal;
    public Text ExtraInfo;

    public GameObject[] triggers = new GameObject[12];



    private void OnTriggerEnter(Collider col)
    {
        GameObject other = col.gameObject;  // triggers placed in scene. On collision, the triggers will change the message.
        if (other == triggers[0])
        {
            Goal.text = "Reach Exit Door";
            Goal.fontSize = 100;
            Goal.rectTransform.localPosition = new Vector3(0, 0f, 0);
            ExtraInfo.text = "";
        }
        else if (other == triggers[1] || other == triggers[7] || other == triggers[8] || other == triggers[9] || other == triggers[10] || other == triggers[11])
        {
            Goal.text = "Destroy All 3\nCircuits to\nUnlock Door";
            Goal.fontSize = 100;
            Goal.rectTransform.localPosition = new Vector3(0, 0f, 0);
            ExtraInfo.text = "";
        }
        else if (other == triggers[2] || other == triggers[3] || other == triggers[4] || other == triggers[5] || other == triggers[6])
        {
            Goal.text = "Find and Disable\nBarrier to destroy\nCircuit";
            Goal.fontSize = 100;
            Goal.rectTransform.localPosition = new Vector3(0, 0f, 0);
            ExtraInfo.text = "Use Hack Shot to destroy Circuit";
        }
    }
}
