using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBuildingRandomizer : MonoBehaviour
{
    // properties
    public Transform[] spawnPoints = new Transform[4];
    private Vector3 scale = new Vector3(1.5f, 1.5f, 1.5f);

    // Start is called before the first frame update
    void Start()
    {
        // get random list of building models from building holder script
        GameObject[] buildings = BuildingHolder.instance.GetRandomBuildings(spawnPoints.Length);
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            // place a building on each spawn point of a road tile
            GameObject b = Instantiate(buildings[i], spawnPoints[i].position, Quaternion.identity, transform);
            b.transform.localScale = scale;
        }
    }

}
