using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DronePulse : MonoBehaviour
{
    public float timer;
    public float interval;
    public SpriteRenderer droneIcon;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // make drone have a ticking clock symbol that oscillates every number of seconds
        if (GetComponent<RunningScript>().getStayStill())
        {
            timer += Time.deltaTime;
            if (timer > interval)
            {
                timer = 0f;
            }
            Color temp = droneIcon.color;
            temp.a = oscillation(timer, interval, 0.5f) + 0.5f;
            droneIcon.color = temp;
        }
        else
        {
            Color temp = droneIcon.color;
            temp.a = 0f;
            droneIcon.color = temp;
        }
    }

    float oscillation(float time, float speed, float scale)
    {
        return Mathf.Cos(time * speed / Mathf.PI) * scale;
    }
}
