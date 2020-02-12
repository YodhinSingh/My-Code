using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    bool allowHit;
    public GameObject bulletShooter;
    // Start is called before the first frame update
    void Start()
    {
        allowHit = true;
        Destroy(gameObject, 3f);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("droneRange")) // as long as colliding object is not a drone range sensor do the following
        {
            GetComponent<MeshRenderer>().enabled = false;   // Disable the bullet so the player wont see the bullet going through the object
            if (other.gameObject.CompareTag("Enemy") && allowHit)   // If its an enemy play the sound effect and apply damage
            {
                StartCoroutine("DestroyBullet");
                other.gameObject.GetComponent<EnemyScript>().TakeDamageEnemy();
                allowHit = false;       // this makes sure that this function isnt accidently called more than once
            }
            else
                Destroy(gameObject);    // otherwise just destroy the bullet
        }
    }

    private IEnumerator DestroyBullet()
    {
        GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);  // Play the hit sound effect before destroying bullet
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);

    }



}
