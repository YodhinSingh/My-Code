using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePlatformSpin : MonoBehaviour
{

    public float speed1; // default speed value
    public float speed2;

    private float speedState;


    public bool allowStandOn;
    public bool rotate90Version = false;    // how rotation happens. Is the game obj flat on ground or standing upright. Set in unity.

    // Start is called before the first frame update
    void Start()
    {
        speedState = speed1;    // give it random starting rotation and set its speed to speed1 (default)
        if (!rotate90Version)
            transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0.0f);
        else
            transform.Rotate(0, Random.Range(0, 90)*4, 0, Space.Self);
    }

    // Update is called once per frame
    void Update()
    {
        if (!rotate90Version)               // have it always rotating
            transform.Rotate(0, speedState * Time.deltaTime, 0, Space.World);
        else
            transform.Rotate(0, speedState * Time.deltaTime, 0, Space.Self);
    }

    public void ChangeRotationSpeed()
    {
        if (speedState == speed1)       // change speed from 1 to 2 or vice versa when player's bullet hits this obj
            speedState = speed2;
        else
            speedState = speed1;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (allowStandOn)   // if this platform is face down, allow the player to keep standing on it when its moving
        {
            if (col.gameObject.CompareTag("Player"))
            {
                col.transform.SetParent(transform);
            }
        }
    }

    private void OnCollisionExit(Collision col)
    {
        if (allowStandOn)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                col.transform.SetParent(null);

            }
        }
    }
}
