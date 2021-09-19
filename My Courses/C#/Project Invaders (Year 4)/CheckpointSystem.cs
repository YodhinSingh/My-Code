using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{
    // record progess of each player
    private int p1Progress;
    private int p2Progress;

    // have re-spawn points for players incase they die
    public Transform[] p1SpawnPoints;
    public Transform[] p2SpawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        p1Progress = p2Progress = 0;
        GiveIndex();
    }

    // assign a unique index for each checkpoint so it can be identified later
    private void GiveIndex()
    {
        // checkpoints are order based on sequence in level (so higher number means later checkpoint)
        for (int i = 0; i < p1SpawnPoints.Length; i++)
        {
            // give a reference to this so they can call this script
            p1SpawnPoints[i].GetComponent<CheckPointSpot>().RecieveIndex(i, this);
            p2SpawnPoints[i].GetComponent<CheckPointSpot>().RecieveIndex(i, this);
        }
    }

    // special trigger if players fall off level
    private void OnTriggerEnter(Collider other)
    {
        // put player in last known spawn point that they passed
        if (other.tag == "PlayerOne" || (other.tag == "Drone" && other.GetComponentInParent<PlayerScript>().GetPlayerIndex() == 0))
        {
            other.GetComponentInParent<PlayerScript>().transform.position = p1SpawnPoints[p1Progress].position;
            other.GetComponentInParent<PlayerScript>().transform.rotation = Quaternion.identity;
            other.GetComponentInParent<PlayerHealth>().KillBoxPlayer();
        }

        else if (other.tag == "PlayerTwo" || (other.tag == "Drone" && other.GetComponentInParent<PlayerScript>().GetPlayerIndex() == 1))
        {
            other.GetComponentInParent<PlayerScript>().transform.position = p2SpawnPoints[p2Progress].position;
            other.GetComponentInParent<PlayerScript>().transform.rotation = Quaternion.identity;
            other.GetComponentInParent<PlayerHealth>().KillBoxPlayer();
        }
    }

    // called whenever player passes through checkpoint
    public void UpdatePlayerIndex(int player, int newIndex)
    {
        // update each player individually, and only update if its a point thats newer (dont update if player backtracks)
        if (player == 1 && p1Progress < newIndex)
        {
            p1Progress = newIndex;
        }
        else if (player == 2 && p2Progress < newIndex)
        {
            p2Progress = newIndex;
        }
    }
}
