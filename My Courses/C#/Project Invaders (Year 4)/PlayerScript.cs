using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    // base properties
    private int playerIndex;
    public bool AllowControls = false;

    // movement/looking properties
    private Vector2 moveVector;
    private Vector2 rotateVector;
    private float rotX, rotY;
    private float turnSmooth = 0.1f;
    private float turnSmoothVel;
    private float jumpValue;
    private bool wantToJump;
    private float moveSpeed = 15f;
    private float sprintSpeed;
    private float regularSpeed;
    private float regularGravity;
    private float jumpPower = 10f; 
    private float clampAngle = 25f;
    private float inputSensitivity = 100f;

    // other properties
    private bool isBlocking;
    private bool isQAttacking;
    private bool isHAttacking;
    [HideInInspector] public bool isKnockedOut = false;
    private float airMoveMultiplier = 0.5f;

    // car related
    public GameObject Car;
    public Transform CarShootCannon;

    // components
    CharacterController controller;
    Rigidbody rb;
    Vector3 rbCM;
    private PlayerCombat pCombat;
    private PlayerHealth pHealth;
    private ElementShoot shooter;

    // models
    public Animator anim;
    public SkinnedMeshRenderer body;
    public MeshRenderer sword;

    // camera
    public GameObject camPrefab;
    public GameObject virtualCamPrefab;
    private Transform camT;
    private Vector3 camOffset = new Vector3(2, 1.2f, -15);
    private CameraScript camMovement;
    private GameObject pCover;
    public Transform CameraFollow;

    // gravity
    private float gravity = -50f;
    private Coroutine gravityResetCoroutine;
    private WaitForSeconds gravityReset;

    // stamina
    private Image StaminaBar;
    private bool isSprinting = false;
    private Coroutine StaminaDelayCoroutine;
    private WaitForSeconds staminaDelay;
    private bool isStaminaDelayed = false;
    private bool wasSprinting = false;
    private float sprintConsumptionRate = 0.5f;
    private float sprintRevivalRate = 0.25f;

    private int sceneNum;

    // set up properties that need new updates/references every level/restart
    public void SetUp(int PlayerIndex, Vector3 pos, float camShape, Image StaminaBar, GameObject pCover, TextMeshProUGUI multiplier)
    {
        // get references and set variable values
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        pCombat = GetComponent<PlayerCombat>();
        pHealth = GetComponent<PlayerHealth>();
        shooter = GetComponent<ElementShoot>();
        pHealth.isReady = false;

        // teleport player to starting point
        controller.enabled = false;
        controller.transform.position = pos;
        controller.enabled = true;

        // reset movement inputs
        moveVector = Vector2.zero;
        rotateVector = Vector2.zero;

        // look at starting point
        transform.localRotation = Quaternion.identity;

        // set control properties
        playerIndex = PlayerIndex;
        regularSpeed = moveSpeed;
        sprintSpeed = moveSpeed * 1.5f;
        regularGravity = gravity;
        this.StaminaBar = StaminaBar;
        this.pCover = pCover;
        airMoveMultiplier = 0.5f;

        SetTagAndLayer();

        // determine input type and change sensitivity based on it (keyboard & mouse is more sensitive than controller)
        string device = PlayerInput.GetPlayerByIndex(playerIndex - 1).currentControlScheme;
        inputSensitivity = device.Equals("Keyboard&Mouse") ? inputSensitivity / 20 : inputSensitivity;
        
        // create and setup camera
        GameObject camOBJ = Instantiate(camPrefab, transform.position, Quaternion.identity) as GameObject;
        camT = camOBJ.transform;
        Camera c = camOBJ.GetComponentInChildren<Camera>();

        // vertical split
        //camOBJ.GetComponentInChildren<Camera>().rect = new Rect(0, camShape, 1, 0.5f);
        // horizontal split
        camOffset = new Vector3(2, 1.2f, -15);
        c.rect = new Rect(camShape, 0, 0.5f, 1);
        camMovement = camOBJ.GetComponent<CameraScript>();
        camMovement.SetUp(camOffset, CameraFollow, playerIndex-1, false);

        // give camera reference to other script
        pCombat.cam = c;
        shooter.SetUp(c);
        DamagePopUpUi.cam[playerIndex-1] = c;
        c.gameObject.layer = LayerMask.NameToLayer("P" + playerIndex + "Cam");
        GetComponent<PlayerInput>().camera = camT.GetComponentInChildren<Camera>();
        camT.rotation = Quaternion.Euler(0, 0, 0);    // make look forward

        // make player model colours/values based on player number
        pCombat.playerIndex = GetPlayerIndex();
        PlayerSuitColour suitMat = GetComponent<PlayerSuitColour>();
        suitMat.PutMaterialsOnBody(body, playerIndex - 1);
        suitMat.PutMaterialOnSword(sword, playerIndex - 1);

        // set up UI multiplier
        multiplier.text = "x " + pCombat.GetMultiplier().ToString();

        // reset certain stats that occur for every level
        ResetBaseStats();

        sceneNum = LevelWinCondition.GetSceneNum();
        pCombat.sceneNum = sceneNum;
        pCombat.SetUpSwordColours(GetPlayerIndex());

        // if level 3, convert the model to car
        if (sceneNum == 3)
        {
            ConvertToCar(true);
        }
        else
        {
            ConvertToCar(false);
        }
        // configure shooting point based on which model is being used
        shooter.ConfigureShootPoint();

    }

    // Heading to a menu, so turn player into human model and switch controls for UI
    public void ResetPlayer()
    {
        ConvertToCar(false);
        SwitchToUI();
    }

    private void ConvertToCar(bool toLvlThree)
    {
        // enable/disable models/controls for car vs human
        body.enabled = !toLvlThree;
        sword.enabled = !toLvlThree;
        GetComponent<CapsuleCollider>().enabled = !toLvlThree;
        controller.enabled = !toLvlThree;
        Car.SetActive(toLvlThree);
        GetComponent<PlayerCar>().AllowControls = toLvlThree;

        // modify camera and sprint properties for car if level 3
        if (toLvlThree)
        {
            CameraFollow.localPosition = new Vector3(0, 2.5f, 5);
            camOffset = new Vector3(1, 1.2f, -25);
            camMovement = camT.GetComponent<CameraScript>();
            camMovement.SetUp(camOffset, CameraFollow, playerIndex - 1, true);

            GetComponent<PlayerInput>().SwitchCurrentActionMap("Car");
            sprintConsumptionRate = 1.25f;
            sprintRevivalRate = 0.1f;
            SetTagAndLayerCar();

        }
        else
        {
            CameraFollow.localPosition = new Vector3(0, 1.1f, 0); // 1.37
            camOffset = new Vector3(2, 1.2f, -15);
            camMovement = camT.GetComponent<CameraScript>();
            camMovement.SetUp(camOffset, CameraFollow, playerIndex - 1, false);
            SwitchToPlayer();
            sprintConsumptionRate = 0.5f;
            sprintRevivalRate = 0.25f;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

    }

    // switch control type to UI
    public void SwitchToUI()
    {
        GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
    }

    // switch control type to gameplay
    public void SwitchToPlayer()
    {
        GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
    }

    // give p1 and p2 the right tags
    public void SetTagAndLayer()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");

        if (playerIndex == 2)
        {
            gameObject.tag = "PlayerTwo";
        }
        else
        {
            gameObject.tag = "PlayerOne";
        }
    }

    // give p1 and p2 the right tags for car levels
    public void SetTagAndLayerCar()
    {
        Car.gameObject.layer = LayerMask.NameToLayer("Default");

        if (GetComponentInChildren<HoverCarController>() == null)
        {
            return;
        }

        if (playerIndex == 1)
        {
            GetComponentInChildren<HoverCarController>().gameObject.tag = "PlayerCarOne";
        }
        else
        {
            GetComponentInChildren<HoverCarController>().gameObject.tag = "PlayerCarTwo";
        }
    }

    // turn off car model
    public void DisableCar(bool disable)
    {
        if (GetComponentInChildren<HoverCarController>() == null)
        {
            return;
        }
        GetComponentInChildren<HoverCarController>().HideCar(disable);
    }

    // convert car to drone for car levels
    public void ConvertCarToDrone(bool isTrue)
    {
        // switch controls to drone
        if (isTrue)
        {
            GetComponent<PlayerInput>().SwitchCurrentActionMap("CarDrone");
            GetComponentInChildren<HoverCarController>().gameObject.tag = "Drone";
            GetComponentInChildren<HoverCarController>().gameObject.layer = LayerMask.NameToLayer("Drone");
        }
        else
        {
            GetComponent<PlayerInput>().SwitchCurrentActionMap("Car");
            SetTagAndLayer();
            SetTagAndLayerCar();
        }

        DisableCar(isTrue);

    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        staminaDelay = new WaitForSeconds(1);
        gravityReset = new WaitForSeconds(1.5f);

        if (gravity > 0)
        {
            regularGravity = gravity *= -1;
        }

        rotY = transform.localRotation.eulerAngles.y;
        rotX = transform.localRotation.eulerAngles.x;


    }

    // Update is called once per frame
    void Update()
    {
        if (AllowControls)
        {
            RotateView();
            MoveCharacter();

            CalculateStamina();

        }
    }

    private void MoveCharacter()
    {
        // do not use human controls for car
        if (sceneNum == 3)
        {
            return;
        }

        if (controller.isGrounded)
        {
            jumpValue = 0f;
        }

        // only jump if on ground
        if (wantToJump && controller.isGrounded)
        {
            // play animation first
            anim.SetTrigger("Jump");
            jumpValue += Mathf.Sqrt(jumpPower * -regularGravity);
        }
        // if falling
        if (controller.velocity.y <= 0)
        {
            // lower the jump value based on gravity and time (i.e. longer fall = faster fall)
            // but this gravity can be modified for air combat so not always the case to fall faster right away
            jumpValue += gravity * Time.deltaTime;
        }
        // if in process of jumping
        else
        {
            // slow down force upwards based on gravity
            jumpValue += regularGravity * Time.deltaTime;
        }

        float modSpeed = 1f;
        // cannot move while blocking/attacking/shooting
        if (isBlocking || pCombat.isAttacking || shooter.IsFiring())
        {
            modSpeed = 0f;
        }

        // play animations that were triggered
        anim.SetBool("isBlocking", isBlocking);
        anim.SetBool("OnGround", controller.isGrounded);
        anim.SetFloat("FallSpeed", controller.velocity.y);

        // calculate move values using inputs and modifications
        Vector3 move = (transform.right * moveVector.x + transform.forward * moveVector.y) * modSpeed;
        // determine how much force player is putting on movement (walk vs run) and play that animation
        int speed = MovementAnim(move.magnitude);
        anim.SetInteger("MoveSpeed", speed);

        // apply movement with some modifications to controller
        if (move.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveVector.x, moveVector.y) * Mathf.Rad2Deg + camT.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVel, turnSmooth);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            controller.Move(moveDir.normalized * this.moveSpeed * Time.deltaTime);
            
        }
        // apply jump input to controller
        move = new Vector3(0, jumpValue, 0);
        controller.Move(move * Time.deltaTime);
    }

    // return an integer based on how fast the movement the input is (walk vs run)
    private int MovementAnim(float value)
    {
        if (value > 0.1f) // ignore too small values to avoid jitter
        {
            // sprinting only if enough stamina
            if (moveSpeed == sprintSpeed && GetStamina() > 0.05f)
            {
                return 2;
            }
            // otherwise walking
            return 1;
        }
        // not moving
        return 0;
    }

    // launch the player upwards with force
    public void Launch(float power, float maxJValue)
    {
        jumpValue = 0;
        jumpValue = Mathf.Max(jumpValue + Mathf.Sqrt(jumpPower * -regularGravity * power), maxJValue);
        anim.SetTrigger("Jump");
    }

    // rotate camera based on input
    private void RotateView()
    {
        // get input with respect to sensitivity and time (+ clamp top/down view)
        rotY += rotateVector.x * inputSensitivity * Time.deltaTime;
        rotX -= rotateVector.y * inputSensitivity * Time.deltaTime;
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        camT.rotation = Quaternion.Euler(rotX, rotY, 0);    // rotate on both x and y
        //camT.rotation = Quaternion.Euler(0, rotY, 0);     // rotate only on y axis

    }

    private void ConvertMoveLocalToWorld(float x, float z)
    {
        Vector3 temp = new Vector3(x, 0, z);
        temp = transform.TransformDirection(temp);
        moveVector.x = temp.x;
        moveVector.y = temp.z;
    }


    private void OnMove(InputValue value)   // get movement info from Unity's new input system
    {
        if (!AllowControls)
        {
            return;
        }

        Vector2 move = value.Get<Vector2>();

        if (!controller.isGrounded)
        {
            if (pCombat.isAttacking || shooter.IsFiring()) // stop any forward movement when attack or shooting for remainder of jump
            {
                airMoveMultiplier = 0;
            }

            move *= airMoveMultiplier;   // if in the air, player movement will be slower
        }
        else
        {
            airMoveMultiplier = 0.5f;
        }
        moveVector = move;
    }

    private void OnLook(InputValue value) // get look direction info from Unity's new input system
    {
        rotateVector = value.Get<Vector2>();
    }


    private void OnJump(InputValue value) // get jump info from Unity's new input system
    {
        if (controller.isGrounded) // if not already in air, then play jump booster animation for drone
        {
            pHealth.JumpBooster();
        }
        wantToJump = value.isPressed;
    }
    
    private void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed;
    }

    private void OnBlockCar(InputValue value)
    {
        isBlocking = value.isPressed;
    }

    private void OnBoost(InputValue value)
    {
        isSprinting = value.isPressed;
    }

    private void CalculateStamina() // if player holds special key (retreived from OnSpecialPress) then go faster
    {
        // cannot sprint while blocking
        if (isBlocking)
        {
            moveSpeed = regularSpeed;
            return;
        }

        // if already sprinting
        if (isSprinting)
        {
            // lower the stamina bar at the preset rate
            if (StaminaBar.fillAmount > 0)
            {
                moveSpeed = sprintSpeed;
                StaminaBar.fillAmount -= Time.deltaTime * sprintConsumptionRate;
                wasSprinting = true;
            }
            // ran out of stamina, so stop sprinting
            else
            {
                StaminaBar.fillAmount = 0;
                moveSpeed = regularSpeed;
            }
        }
        // stopped sprinting, so make it back to regular speed
        else
        {
            moveSpeed = regularSpeed;

            if (wasSprinting)
            {
                // have a small delay before refilling stamina bar
                StopStaminaRecharge();
                wasSprinting = false;
                return;
            }

            // refill stamina bar to 1
            if (StaminaBar.fillAmount < 1)
            {
                if (isStaminaDelayed)
                {
                    return;
                }

                StaminaBar.fillAmount += Time.deltaTime * sprintRevivalRate;
            }
            else
            {
                StaminaBar.fillAmount = 1;
            }
        }
    }
    
    // temporarily lower gravity for air combat
    public void LowerGravityForAttack()
    {
        if (controller.isGrounded)
        {
            return;
        }

        // if gravity was already lowered and a cooldown started, reset the cooldown to start right now
        if (gravityResetCoroutine != null && gravity < regularGravity)
        {
            StopCoroutine(gravityResetCoroutine);
        }

        gravity = regularGravity / 50f;
        gravityResetCoroutine = StartCoroutine(ResetGravity());
    }

    private IEnumerator ResetGravity()
    {
        yield return gravityReset;
        gravity = regularGravity;
        StopCoroutine(gravityResetCoroutine);
    }

    private void OnBlock(InputValue value)
    {
        if (controller.isGrounded) // only allow block if on ground
        {
            isBlocking = value.isPressed;
        }
    }

    // player 1's index will be 0, and player 2 will be 1 (for array indexing simplicity)
    public int GetPlayerIndex()
    {
        return playerIndex == 1 ? 0 : 1;
    }

    public float GetStamina()
    {
        return StaminaBar.fillAmount; // returns a value from 0 to 1
    }

    public float SetStamina(float value)
    {
        // using stamina will cause refill function to have a small delay
        if (isStaminaDelayed)
        {
            StopCoroutine(StaminaDelayCoroutine);
        }

        StaminaDelayCoroutine = StartCoroutine(StaminaRechargeDelay());

        return (StaminaBar.fillAmount = Mathf.Max(StaminaBar.fillAmount - value , 0));
    }

    private void StopStaminaRecharge()
    {
        if (StaminaDelayCoroutine != null && isStaminaDelayed)
        {
            StopCoroutine(StaminaDelayCoroutine);
        }

        StaminaDelayCoroutine = StartCoroutine(StaminaRechargeDelay());
    }

    private IEnumerator StaminaRechargeDelay()
    {
        isStaminaDelayed = true;
        yield return staminaDelay;
        isStaminaDelayed = false;
    }

    // basic properties that are reset at every level
    public void ResetBaseStats()
    {
        regularSpeed = moveSpeed = 15f;
        sprintSpeed = moveSpeed * 1.5f;
        regularGravity = gravity = -50f;
        inputSensitivity = 100f;
        StaminaBar.fillAmount = 1;
        string device = PlayerInput.GetPlayerByIndex(playerIndex - 1).currentControlScheme;
        inputSensitivity = device.Equals("Keyboard&Mouse") ? inputSensitivity / 20 : inputSensitivity;
        sprintConsumptionRate = 0.5f;
        sprintRevivalRate = 0.25f;
        isKnockedOut = false;

        pHealth.DisablePlayer(false);
        pCombat.isReady = true;
    }

    // make screen go black for a duration
    public void HideScreen()
    {
        StopMovement();
        StartCoroutine(HideScreenTime());
    }

    // helper method to make screen go black for 4 seconds
    private IEnumerator HideScreenTime()
    {
        pCover.SetActive(true);
        AllowControls = false;
        shooter.isReady = false;
        camT.position = CameraFollow.position;
        yield return new WaitForSeconds(4);
        camT.position = CameraFollow.position;
        pCover.SetActive(false);
        AllowControls = true;
        shooter.isReady = true;
    }

    // for quickly stopping player input and resetting to standing still
    public void StopMovement()
    {
        moveVector = Vector2.zero;
        anim.SetInteger("MoveSpeed", 0);
    }


}
