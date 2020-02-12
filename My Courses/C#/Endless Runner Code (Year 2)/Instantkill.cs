using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantkill : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other) // any objs with this script kills the player on contact
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<player>().health = 0;
        }

    }
}
