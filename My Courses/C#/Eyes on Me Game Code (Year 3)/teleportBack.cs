using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleportBack : MonoBehaviour
{

    public GameObject obj;  // target 
    Vector3 initPos;    // target initial position when game starts 

    // Start is called before the first frame update
    void Start()
    {
        if (obj != null)
        {
            initPos = obj.GetComponent<Transform>().position;
        }
    }

    public void resetObj()
    {
        obj.GetComponent<Transform>().position = initPos;
    }

    
    private void OnCollisionEnter(Collision col)
    {
        if (obj != null && col.gameObject == obj)   // when target collides with this, teleport it back to that start point
        {
            resetObj();
        }
    }
}
