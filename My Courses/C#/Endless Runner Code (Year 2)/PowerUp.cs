using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public int score;
    public float speed;

    // on contact with player, find the score tracker obj and increase player score

    scoreManager sm;

    private void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime); // move with platforms so it doesnt end up floating in mid air
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManagerScript.PlaySound("PowerUp");
            sm = GameObject.Find("score").GetComponent<scoreManager>();
            sm.increaseScore(score);
            Destroy(gameObject);


        }


    }
}
