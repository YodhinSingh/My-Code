using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGrav : MonoBehaviour {

    // This script that rotates the gravity of the scene to allow for movement (also rotates player so feet are always on ground for that gravity direction)
    // works alongside character controller script

    private Rigidbody2D rb;
    private PlayerController player;
    private Animator animator;
    private bool GravitySwitch;

    public int numRotations = 0;

	// Use this for initialization
	void Start () {
        player = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        numRotations = 0;
        Rotation();
        GravitySwitch = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))   // set gravity to be towards the top of screen (upside down for us)
        {
            GameSound.PlaySound("GravSwitch");
            numRotations = 2;       // this is used in Rotation() to determine which way to change gravity and rotation
            GravitySwitch = true;   // Indicates gravity is being switched
            Rotation();             // this rotates the player and changes the unity gravity
        }
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))   // Gravity is no longer being switched
        {
            GravitySwitch = false;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) // set gravity to be normal
        {
            GameSound.PlaySound("GravSwitch");
            numRotations = 0;
            GravitySwitch = true;
            Rotation();
        }
        if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
        {
            GravitySwitch = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) // set gravity to pull to the left
        {
            GameSound.PlaySound("GravSwitch");
            numRotations = 1;
            GravitySwitch = true;
            Rotation();
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            GravitySwitch = false;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))    // set gravity to pull to the right
        {
            GameSound.PlaySound("GravSwitch");
            numRotations = 3;
            GravitySwitch = true;
            Rotation();
        }
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            GravitySwitch = false;
        }

        animator.SetBool("GravSwitch", GravitySwitch);  // play gravity switch animation (player rolls)
	}

    void Rotation() // apply rotations to player and gravity based on numRotations value
    {
        if (numRotations == 0)
        {
            transform.eulerAngles = Vector3.zero;
            Physics2D.gravity = new Vector2(1f, -9.81f);
        }
        else if (numRotations == 1)
        {
            transform.eulerAngles = new Vector3(0, 0, -90f);
            Physics2D.gravity = new Vector2(-9.81f, 1f);
        }
        else if (numRotations == 2)
        {
            transform.eulerAngles = new Vector3(0, 0, 180f);
            Physics2D.gravity = new Vector2(1f, 9.81f);
        }
        else if (numRotations == 3)
        {
            transform.eulerAngles = new Vector3(0, 0, 90f);
            Physics2D.gravity = new Vector2(9.81f, 1f);
        }

    }
}
