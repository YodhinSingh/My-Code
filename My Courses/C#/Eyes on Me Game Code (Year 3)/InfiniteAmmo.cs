using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteAmmo : MonoBehaviour
{
    public int ammoVer;

    private void Update()
    {
        if (Time.timeScale != 0)
            transform.Rotate(0, 2, 0, Space.World); // have coin always rotating
    }


    private void OnTriggerEnter(Collider col)   // on interacting with player, set their ammo type to be infinite and destroy all remaining infinite ammo coins
    {
        if (col.gameObject.CompareTag("Player"))
        {
            AudioManager.PlaySound("AmmoPickup");
            col.gameObject.GetComponentInChildren<bulletShoot>().SetAmmoInfinite(ammoVer);
            if (ammoVer == 1)
            {
                Destroy(GameObject.Find("AmmoSpeedInf(Clone)"));
                Destroy(GameObject.Find("AmmoHackInf(Clone)"));
            }
            else if (ammoVer == 2)
            {
                Destroy(GameObject.Find("AmmoScaleInf(Clone)"));
                Destroy(GameObject.Find("AmmoHackInf(Clone)"));
            }
            else
            {
                Destroy(GameObject.Find("AmmoScaleInf(Clone)"));
                Destroy(GameObject.Find("AmmoSpeedInf(Clone)"));
            }

            Destroy(gameObject);
        }
    }
}
