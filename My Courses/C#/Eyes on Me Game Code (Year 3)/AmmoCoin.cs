using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCoin : MonoBehaviour
{
    public int ammoVer;

    bool AllowMod = true;
    public bool Restart = false;

    GameObject spawnGenerator;

    private void Start()
    {
        spawnGenerator = GameObject.Find("SpawnPointsGenerator");
    }

    private void Update()
    {
        if (Time.timeScale != 0)
            transform.Rotate(0, 2, 0, Space.World); // have the coin keep rotating like old video games
    }


    private void OnTriggerEnter(Collider col)
    {
     if (col.gameObject.CompareTag("Player") && AllowMod)   // give player more ammo
        {
            AudioManager.PlaySound("AmmoPickup");
            col.gameObject.GetComponentInChildren<bulletShoot>().AddAmmo(ammoVer);
            AllowMod = false;
            if (Restart)
                spawnGenerator.GetComponent<SpawnPointGenerate>().ResetTag(0);      // tell spawn manager that this spot is free to respawn ammo
            Destroy(gameObject);
        }   
    }
}
