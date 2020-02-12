using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


// player controller that uses the PhysicsObject class to control the player. Deals with animations and getting input

public class player : PhysicsObject {

    public float maxSpeed = 7;
    public float jumpSpeed = 7;

    private Vector2 move = Vector2.zero;

    protected bool isRolling;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private float CameraMoveSpeed;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        CameraMoveSpeed = 0.74f;
    }

    protected override void ComputeVelocity()
    {
        move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal") + CameraMoveSpeed;

        if (Input.GetButtonDown("Roll"))
        {
            isRolling = true;
            transform.Rotate(0, 0, -90f);
            GetComponent<BoxCollider2D>().offset = new Vector2(0.48f, -0.1f);
        }
        else if (Input.GetButtonUp("Roll"))
        {
            isRolling = false;
            transform.Rotate(0, 0, 90f);
            GetComponent<BoxCollider2D>().offset = new Vector2(0f, -0.1f);
            
        }

        if (Input.GetButtonDown("Jump") && grounded)
        {
            SoundManagerScript.PlaySound("Jump");
            velocity.y = jumpSpeed;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0 && Input.GetButtonDown("Roll"))
            {
                velocity.y = velocity.y * 0.5f;
            }
        }

        bool flipSprite = (spriteRenderer.flipX ? (move.x > (0f + CameraMoveSpeed)) : (move.x < (0f + CameraMoveSpeed)));
        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        animator.SetBool("Ground", grounded);
        animator.SetFloat("Speed", Mathf.Abs(velocity.x) / maxSpeed);
        animator.SetBool("isRolling", isRolling);

        targetVelocity = move * maxSpeed;
    }

}