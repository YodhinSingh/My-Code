using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformJump : MonoBehaviour
{
    // paltform properties
    public float power = 3;
    public bool isLauncher = false;
    public float maxJumpValue = 60;

    private bool allow = true;

    private void OnTriggerEnter(Collider col)
    {
        // if its a launcher, and a player lands on it, launch them into the air with force
        if (isLauncher && allow && (col.tag == "PlayerOne" || col.tag == "PlayerTwo" || col.tag == "Drone"))
        {
            allow = false;
            col.GetComponentInParent<PlayerScript>().Launch(power, maxJumpValue);
            // start cooldown so it does it activate too fast
            StartCoroutine(Wait());
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        allow = true;
    }

}
