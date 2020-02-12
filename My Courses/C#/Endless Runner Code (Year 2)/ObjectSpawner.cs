using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{

    public GameObject[] obj;
    public float spawnMin = 20f;
    public float spawnMax = 30f;

    // Spawns obstacles like bombs in the given time range (min and max)

    void Start()
    {
        Spawn();
    }
    void Spawn()
    {
        Instantiate(obj[Random.Range(0,obj.GetLength(0))], transform.position, Quaternion.identity);
        Invoke("Spawn", Random.Range(spawnMin, spawnMax));
    }
}
