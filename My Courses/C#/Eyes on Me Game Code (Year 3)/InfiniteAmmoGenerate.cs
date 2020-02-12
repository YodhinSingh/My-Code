using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteAmmoGenerate : MonoBehaviour
{
    public GameObject[] InfSpawnAmmo;
    public GameObject[] InfSpawnAmmoPoints;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < InfSpawnAmmoPoints.Length; i++) // instantiates infinite ammo coins at given locations
        {
            Instantiate(InfSpawnAmmo[i], InfSpawnAmmoPoints[i].transform.position, Quaternion.identity);
            InfSpawnAmmoPoints[i].tag = "UsedSpawnAmmo";
        }
    }

}
