using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    // bullet class for enemy

    // bullet properties
    private float damageValue;

    // explosion models
    [SerializeField] private GameObject explosion = null;
    private GameObject explosionHolder;
    int sceneNum;

    // Start is called before the first frame update
    void Start()
    {
        // destroy bullet in 5 sec
        Destroy(gameObject, 5f);
        sceneNum = LevelWinCondition.GetSceneNum();
    }

    private void OnCollisionEnter(Collision other)
    {
        // play explosion system
        explosionHolder = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
        Destroy(explosionHolder, explosion.GetComponent<ParticleSystem>().main.duration);

        // for regular levels, if player 1 or 2 hit, apply damage
        if (sceneNum != 3)
        {
            if ((other.gameObject.tag == "PlayerOne" || other.gameObject.tag == "PlayerTwo") && other.gameObject.TryGetComponent(out PlayerHealth e))
            {
                e.RecieveDamage(damageValue, false);
            }
        }
        // for car levels, account for drone models too
        else
        {
            if ((other.gameObject.tag == "PlayerOne" || other.gameObject.tag == "PlayerTwo") && other.gameObject.TryGetComponent(out PlayerHealth e))
            {
                e.RecieveDamage(damageValue, false);
            }
            else if (other.gameObject.tag == "PlayerCarOne" || other.gameObject.tag == "PlayerCarTwo")
            {
                other.gameObject.GetComponentInParent<PlayerHealth>().RecieveDamage(damageValue, false);
            }

        }
        Destroy(gameObject);
    }


    public void SetUp(float damage)
    {
        damageValue = damage;
    }
}
