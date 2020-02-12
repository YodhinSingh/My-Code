using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBox : MonoBehaviour
{
    public float scale1;    // default scale
    public float scale2;
    public float speed1;    // default speed
    public float speed2;
    public float rangeX;
    public float rangeZ;

    public float scaleState;
    public float speedState;
    public bool hackState;      // defaul hack state is false
    bool allowChange = true;

    public GameObject[] hackObjs = new GameObject[4];
    private Vector2 initialLocation;

    public bool allowStandOn;

    // Start is called before the first frame update
    void Start()
    {
        scaleState = scale1;
        speedState = speed1;
        hackState = false;
        initialLocation = new Vector2(transform.position.x, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {

        float OscillateX = Mathf.PingPong(Time.time * speedState, rangeX) + initialLocation.x;  // make obj move back and forth from ranges if speed value > 0
        float OscillateZ = Mathf.PingPong(Time.time * speedState, rangeZ) + initialLocation.y;
        if (rangeX == 0)
        {
            OscillateX = transform.position.x;
        }
        if (rangeZ == 0)
        {
            OscillateZ = transform.position.z;
        }
        transform.position = new Vector3(OscillateX, transform.position.y, transform.position.z);
        transform.position = new Vector3(transform.position.x, transform.position.y, OscillateZ);
        


    }

    public void changeVar(int num)  // when player bullets hit this, modify this obj's properties based on ammo type
    {
        switch (num)
        {
            case 1:         // scale ammo will change the scale from scale1 to scale2 or vice versa
                if (allowChange)
                {
                    if (scaleState == scale1)
                        scaleState = scale2;
                    else
                        scaleState = scale1;
                    transform.localScale = new Vector3(scaleState, scaleState, scaleState);
                }
                break;
            case 2:             // speed ammo will change the speed from speed1 to speed2 or vice versa
                if (speedState == speed1)
                    speedState = speed2;
                else
                    speedState = speed1;
                break;
            case 3:             // hack ammo will change the hackstate from true to false or vice versa
                if (hackState)
                {
                    for (int i = 0; i < hackObjs.Length; i++)       // any objs in this list will be switched from active/not active based on hack state value
                    {
                        if (hackObjs[i] != null && i % 2 == 0) 
                            hackObjs[i].SetActive(true);
                        if (hackObjs[i] != null && i % 2 != 0)
                            hackObjs[i].SetActive(false);
                    }

                    hackState = false;
                }
                else
                {
                    for (int i = 0; i < hackObjs.Length; i++)
                    {
                        if (hackObjs[i] != null && i % 2 == 0)
                            hackObjs[i].SetActive(false);
                        if (hackObjs[i] != null && i % 2 != 0)
                            hackObjs[i].SetActive(true);
                    }
                    hackState = true;
                }
                break;
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (allowStandOn)   // if this is a moving platform, allow the player to keep standing on it
        {
            if (col.gameObject.CompareTag("Player"))
            {
                allowChange = false;
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
                allowChange = true;
                col.transform.SetParent(null);

            }
        }
    }

}
