using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public GameObject player;
    public float noticeRange = 15f;
    public NavMeshAgent nav;

    private float health;
    bool allowHit;

    public GameObject healthbar;
    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        allowHit = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Using unity's nav mesh system to get enemy to chase player if in range
        if (Vector3.Distance(transform.position, player.transform.position) < noticeRange) {
            Vector3 dest = new Vector3(player.transform.position.x, player.transform.position.y + 5, player.transform.position.z);
            nav.SetDestination(player.transform.position);
        }

        if (healthbar != null)  // Set enemy's health bar size
        {
            healthbar.GetComponent<RectTransform>().localScale = new Vector3(health/100, healthbar.GetComponent<RectTransform>().localScale.y, healthbar.GetComponent<RectTransform>().localScale.z);
        }

    }

    public void TakeDamageEnemy()
    {
        health -= 20;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && allowHit)
        {
            other.gameObject.GetComponent<RunningScript>().TakeDamage();
            StartCoroutine("PlayDeathSound");
            allowHit = false;
            
        }
    }

    private IEnumerator PlayDeathSound()
    {
        GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);

    }
}
