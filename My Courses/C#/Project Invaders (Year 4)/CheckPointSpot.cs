using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointSpot : MonoBehaviour
{
    // reference to parent system
    private CheckpointSystem checkpoint;
    private int index;

    // have parent assign unique index to each checkpoint and give access to communicate with it
    public void RecieveIndex(int i, CheckpointSystem c)
    {
        index = i;
        checkpoint = c;
    }

    // on contact with players
    private void OnTriggerEnter(Collider other)
    {
        // let parent system know if a player has gone through it and identify which checkpoint it was 
        if (other.tag == "PlayerOne" || (other.tag == "Drone" && other.GetComponentInParent<PlayerScript>().GetPlayerIndex() == 0))
        {
            checkpoint.UpdatePlayerIndex(1, index);
        }

        else if (other.tag == "PlayerTwo" || (other.tag == "Drone" && other.GetComponentInParent<PlayerScript>().GetPlayerIndex() == 1))
        {
            checkpoint.UpdatePlayerIndex(2, index);
        }
    }
}
