using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    //This is placed in Unity game scene as where the obstacles like bombs spawn from
    public GameObject obstacle;
    private void Start()
    {
        Instantiate(obstacle, transform.position, Quaternion.identity);
    }
}