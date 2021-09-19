using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killbox : MonoBehaviour
{
    // points to spawn players back at
    public Transform teleportPoint1;
    public Transform teleportPoint2;

    // properties
    public bool isTeleportOnly = false;
    public bool isLevel3 = false;

    private void OnTriggerEnter(Collider other)
    {
        // in regular levels
        if (!isLevel3)
        {
            // teleport player 1 to their specific spawn point
            if (other.tag == "PlayerOne" || (other.tag == "Drone" && other.GetComponentInParent<PlayerScript>().GetPlayerIndex() == 0))
            {
                other.GetComponentInParent<PlayerScript>().transform.position = teleportPoint1.position;
                other.GetComponentInParent<PlayerScript>().transform.rotation = Quaternion.identity;
                // either only teleport them, or also have them take damage for 'dying'
                if (isTeleportOnly)
                {
                    other.GetComponentInParent<PlayerHealth>().TeleportPlayer();
                }
                else
                {
                    other.GetComponentInParent<PlayerHealth>().KillBoxPlayer();
                }

            }
            // for player 2's specific spawn point
            else if (other.tag == "PlayerTwo" || (other.tag == "Drone" && other.GetComponentInParent<PlayerScript>().GetPlayerIndex() == 1))
            {
                other.GetComponentInParent<PlayerScript>().transform.position = teleportPoint2.position;
                other.GetComponentInParent<PlayerScript>().transform.rotation = Quaternion.identity;
                if (isTeleportOnly)
                {
                    other.GetComponentInParent<PlayerHealth>().TeleportPlayer();
                }
                else
                {
                    other.GetComponentInParent<PlayerHealth>().KillBoxPlayer();
                }
            }
        }
        // for level 3 (car)
        else
        {
            // player 1 spawn point
            if (other.tag == "PlayerOne" || (other.GetComponentInChildren<HoverCarController>().gameObject.tag == "PlayerCarOne") || (other.tag == "Drone" && other.GetComponent<PlayerScript>().GetPlayerIndex() == 0))
            {
                other.GetComponentInParent<PlayerScript>().transform.position = teleportPoint1.position;
                other.GetComponentInParent<PlayerScript>().transform.rotation = Quaternion.identity;
                if (isTeleportOnly)
                {
                    other.GetComponentInParent<PlayerHealth>().TeleportPlayer();
                }
                else
                {
                    other.GetComponentInParent<PlayerHealth>().KillBoxPlayer();
                }

                other.GetComponentInChildren<CarColourRandomizer>().ActivateThrusters();

            }
            // player 2 spawn point
            else if (other.tag == "PlayerTwo" || (other.GetComponentInChildren<HoverCarController>().gameObject.tag == "PlayerCarTwo") || (other.tag == "Drone" && other.GetComponent<PlayerScript>().GetPlayerIndex() == 1))
            {
                other.GetComponentInParent<PlayerScript>().transform.position = teleportPoint2.position;
                other.GetComponentInParent<PlayerScript>().transform.rotation = Quaternion.identity;
                if (isTeleportOnly)
                {
                    other.GetComponentInParent<PlayerHealth>().TeleportPlayer();
                }
                else
                {
                    other.GetComponentInParent<PlayerHealth>().KillBoxPlayer();
                }
                other.GetComponentInChildren<CarColourRandomizer>().ActivateThrusters();
            }
        }
    }
}
