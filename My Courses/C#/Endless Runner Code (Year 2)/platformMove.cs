using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformMove : MonoBehaviour
{
    // destroy platform after a while so there isnt too many objs in game scene after a while (camera is moving past it so it wont be seen again anyway)
    public float lifetime;

    public float speed;

    void Start()
    {
        Destroy(gameObject, lifetime);
        
    }
}
