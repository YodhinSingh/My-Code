using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementScript : MonoBehaviour
{
    // script that handles element behaviour

    // element object properties
    private float damageValue;
    private int elementType;
    private PlayerCombat pCombat;

    // model of explosion
    public GameObject explosion = null;
    private GameObject explosionHolder;

    // fire/ice vs car laser 'bullet'
    private bool isElement;

    private ParticleSystem p;

    // Start is called before the first frame update
    void Start()
    {
        // only last for 3 seconds max
        Destroy(gameObject, 3f);
    }

    private void OnCollisionEnter(Collision other)
    {
        // play explosion animation
        explosionHolder = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
        Destroy(explosionHolder, explosion.GetComponent<ParticleSystem>().main.duration);
        
        // if the hit target was an enemy, then get damage info from them and send to player responsible
        if (other.gameObject.tag == "Enemy" && other.gameObject.TryGetComponent(out EnemyScript e))
        {
            float value;
            // elemental damage reacts different compared to others and so player must know which attack this was
            if (isElement)
            {
                value = e.ReceiveElementalDamage(elementType, damageValue);
                pCombat.DisplayDamageUI(value, transform.position, true);
            }
            else
            {
                value = e.ReceiveMeleeDamage(damageValue, elementType, false);
                pCombat.DisplayDamageUI(value, transform.position, false);
            }

        }
        // if it hit the other player
        bool condOne = elementType == 1? other.gameObject.tag == "PlayerOne" : other.gameObject.tag == "PlayerTwo";
        if (condOne)
        {
            // player resitance is different than enemy for element vs other
            if (isElement)
            {
                other.gameObject.GetComponent<PlayerHealth>().RecieveDamage(damageValue, true);
                pCombat.DisplayDamageUI(damageValue, transform.position, true);
            }
            else // car 'bullet' blast
            {
                other.gameObject.GetComponent<PlayerHealth>().RecieveDamage(damageValue/2, true);
                pCombat.DisplayDamageUI(damageValue/2, transform.position, false);
            }
        }

        Destroy(gameObject, 0.1f);
    }


    public void SetUp(float damage, int element, PlayerCombat p) // regular element properties
    {
        damageValue = damage;
        elementType = element;
        pCombat = p;
        isElement = true;
    }

    public void SetUpCar(float damage, int element, PlayerCombat pCom) // car 'bullet' properties
    {
        damageValue = damage;
        elementType = element;
        pCombat = pCom;
        isElement = false;
    }
}
