using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour
{
    public int damage = 0;

    public float speed;
    protected bool isHit;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // this script is for the bomb obj that when hit with the player explodes and damages them

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()

    {
        transform.Translate(Vector2.left * speed * Time.deltaTime); // Whole scene is moving, so have the bomb move with it
        animator.SetBool("isHit", isHit);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Apply damage and animation when hit player
        {
            SoundManagerScript.PlaySound("Explode");
            isHit = true;
            animator.SetBool("isHit", isHit);
            other.GetComponent<player>().health = other.GetComponent<player>().health - damage;
            Destroy(gameObject, 0.2f);
            

        }
        

    }
}
