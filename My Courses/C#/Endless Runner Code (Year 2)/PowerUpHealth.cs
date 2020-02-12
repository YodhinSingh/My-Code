using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpHealth : MonoBehaviour
{

    // same as powerUp class but this is for increasing health

    public int heal;
    public float speed;

    private void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManagerScript.PlaySound("PowerUp");
            other.GetComponent<player>().health = other.GetComponent<player>().health + heal;
            Destroy(gameObject);


        }


    }
}
