using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // moving player
    public float speed;
    public float jumpForce;
    private float moveInput;
    
    private Rigidbody2D rb;

    // direction player is facing
    public bool facingRight;

    // jumping
    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    private int extraJumps;
    public int extraJumpsValue;

    private Animator animator;

    float minSurviveFall = 2f;  //the time that the player can spend in the air without taking damage
    public float airTime = 0f;
    public float fallVelocity = 0;

    public Image[] hearts;
    public float playerHealth = 10f;

    private SwitchGrav gravityInfo;

    public float keyNum;


    // Use this for initialization
    void Start()
    {
        extraJumps = extraJumpsValue;
        rb = GetComponent<Rigidbody2D>();
        gravityInfo = GetComponent<SwitchGrav>();
        animator = GetComponent<Animator>();
        facingRight = true;
        Debug.Log(playerHealth);
        keyNum = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (gravityInfo.numRotations == 0 || gravityInfo.numRotations == 2) // down/up gravity
        {
            rb.velocity = new Vector2(0, rb.velocity.y);    // figure out velcity to later use for fall damage, etc. For up/down gravity, y value is needed, left/right = x value
            fallVelocity = rb.velocity.y;
        }
        else if (gravityInfo.numRotations == 1 || gravityInfo.numRotations == 3) // left/right gravity
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            fallVelocity = rb.velocity.x;
        }

        if (facingRight == false && moveInput > 0)  // this and the 3 other if/else statements below make sure the sprite is facing right direction
            Flip();
        else if (facingRight == true && moveInput < 0)
            Flip();

        if (transform.position.x >= 0 && facingRight == false && isGrounded == true)
        {
            Flip();
        }
        else if (transform.position.x < 0 && facingRight == true && isGrounded == true)
        {
            Flip();
        }
    }

    private void Update()
    {
        if (isGrounded == false)
        {
            airTime += Time.deltaTime;
        }


        if (isGrounded == true)
        {
            extraJumps = extraJumpsValue;
            if (airTime > minSurviveFall)
            {
                playerHealth = playerHealth - (int)(Mathf.Abs(fallVelocity) / 15);  // fall damage based on player speed
            }
            airTime = 0;
            
        }


        if (playerHealth <= 0){
            SceneManager.LoadScene(3);  // load lose scene if health is 0
        }
        for (int i = 0; i < hearts.Length; i++) // display number of hearts on canvas (health)
        {
            if (i < playerHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }


        animator.SetBool("Ground", isGrounded);

    }
    void Flip() // flip sprite direction
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.CompareTag("InstantKill"))
        {
            playerHealth = 0f;
        }
        if (other.CompareTag("Win"))
        {
            SceneManager.LoadScene(4);  // win scene
        }

    }
}
