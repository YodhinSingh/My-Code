using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ElementShoot : MonoBehaviour
{
    // handler properties
    public Animator anim;
    private int elementType;
    private int initialElementType;
    private float damageAmount = 20;
    private float range = 50;
    private float forwardForce = 50;

    private bool canSwitchE = false;

    // shooting point properties
    private Transform shootOrigin = null;
    public Transform shootOriginCar;
    public Transform shootOriginRegular;
    private Vector3 carShootOriginOffset;
    public Image UiTarget = null;

    // models
    public GameObject elementFirePrefab = null;
    public GameObject elementIcePrefab = null;
    public GameObject elementCarBulletPrefab = null;
    public GameObject elementCarBulletHeavyPrefab = null;
    private GameObject element;
    private GameObject elementHandler;

    // gameplay properties
    private bool canShoot = false;
    private bool isShooting = false;
    private bool shootQuickAttack = false;
    private bool shootHeavyAttack = false;
    private bool isBlocking;
    private float attackRate = 2f; // used to be 4
    private float nextAttackTime = 0f;
    private float stamina = 1;
    private float HeavyAttackCost = 0.2f;
    private float damageMultiplier;

    [HideInInspector] public bool isReady = false;

    // references to other classes/components
    private EnergyBar energy;
    private PlayerCombat pCombat;
    private PlayerScript pScript;
    Camera cam;
    Vector3 pos;
    Vector3 offset = new Vector3(15f, 15f, 0);
    Ray ray;
    public Transform carShootCanon;
    private Rigidbody rb;
    private PlayerAudioSources pAudio;
    private Image elementSymbol;
    private Sprite[] elementSprites;

    private int sceneNum;

    // Start is called before the first frame update
    void Start()
    {
        // get references
        pCombat = GetComponent<PlayerCombat>();
        pScript = GetComponent<PlayerScript>();
        rb = GetComponent<Rigidbody>();
        damageMultiplier = pCombat.GetMultiplier();
        pAudio = GetComponent<PlayerAudioSources>();
    }

    // place 3D vector of where 'bullet'/element shoots from based on level
    public void ConfigureShootPoint()
    {
        // if regular level, then place infront of player model
        if (LevelWinCondition.GetSceneNum() != 3)
        {
            shootOrigin = shootOriginRegular;
        }
        // if car chase level, then place on car cannon
        else
        {
            shootOrigin = shootOriginCar;
        }
        sceneNum = LevelWinCondition.GetSceneNum();
    }

    // set all properties of handler for this level and get references
    public void SetUp(Vector3 aimPoint, EnergyBar e, Image symbol, Sprite[] s)
    {
        pos = aimPoint + offset;
        energy = e;
        elementSymbol = symbol;
        elementSprites = s;
        elementSymbol.enabled = true;
        initialElementType = elementType = GetComponent<PlayerScript>().GetPlayerIndex();
        DetermineElement();
        damageMultiplier = pCombat.GetMultiplier();

        sceneNum = LevelWinCondition.GetSceneNum();

        // change force of fire based on car (level 3) vs human
        if (sceneNum == 3)
        {
            forwardForce = 80;
        }
        else
        {
            forwardForce = 50;
        }
        isReady = true;
    }

    public void SetUp(Camera cam)
    {
        this.cam = cam;
    }

    // Update is called once per frame
    void Update()
    {
        if (pos == null || cam == null || energy == null || !isReady)
        {
            return;
        }

        // create a ray on where player is aiming
        ray = cam.ScreenPointToRay(pos);
        if (sceneNum == 3)
        {
            // for car level, modify aim of ray to account for car model interferences
            Vector3 carRay = new Vector3(ray.direction.x, -0.03f, ray.direction.z);
            carShootOriginOffset = new Vector3(transform.position.x, transform.position.y + 4f, transform.position.z);

            carShootCanon.LookAt(carShootOriginOffset + carRay * 10);
        }
        // player/car element shooting
        if (canShoot)
        {
            ShootPower(true);
        }
        // car bullet shooting (bullet =/= element)
        else if (shootQuickAttack)
        {
            ShootWeapon(30, false);
        }
        else if (shootHeavyAttack)
        {
            ShootWeapon(50, true);
        }
    }

    // bullet that car cannon shoots
    private void ShootWeapon(float damageValue, bool isHeavy)
    {
        pAudio.PlayCarGunShoot();
        // modify look and damage of bullet based on type of attack
        GameObject currentShot = isHeavy ? elementCarBulletHeavyPrefab : elementCarBulletPrefab;
        elementHandler = Instantiate(currentShot, shootOrigin.transform.position, shootOrigin.transform.rotation) as GameObject;
        elementHandler.GetComponent<ElementScript>().SetUpCar(damageValue * damageMultiplier, elementType, pCombat);

        // push bullet forward
        Rigidbody Temporary_RigidBody = elementHandler.GetComponent<Rigidbody>();
        Vector3 carRay = carShootCanon.forward + Vector3.up * 0.01f;
        Temporary_RigidBody.AddForce(carRay * (forwardForce + rb.velocity.magnitude));
        Temporary_RigidBody.useGravity = false;

        shootQuickAttack = shootHeavyAttack = false;
    }

    // element that player shoots
    private void ShootPower(bool IsPower)
    {
        // Dont shoot if aiming angle is too far from direction player/cannon is facing
        if (Vector3.Angle(transform.forward, ray.direction) > 80 && sceneNum != 3)
        {
            canShoot = false;
            return;
        }

        // remove suit/car energy for using element
        float energyRemain = energy.RemoveEnergy(5);
        
        // if energy is depleted, then gameover
        if (energyRemain <= 0)
        {
            canShoot = false;
            pScript.AllowControls = false;
            return;
        }

        pAudio.PlayElementShoot();

        isShooting = true;
        canShoot = false;

        // player animation to shoot
        anim.SetTrigger("ShootElement");

        // handle scripts relating to creation of element to after animation, so it does not happen too soon
        Invoke("ResetTimer", 0.15f);
    }

    void ResetTimer()
    {
        // create element based on player power and stats
        elementHandler = Instantiate(element, shootOrigin.transform.position, shootOrigin.transform.rotation) as GameObject;
        elementHandler.GetComponent<ElementScript>().SetUp(damageAmount * damageMultiplier, elementType, pCombat);

        Rigidbody Temporary_RigidBody = elementHandler.GetComponent<Rigidbody>();

        // car element power is faster than human element power
        if (sceneNum != 3)
        {
            Temporary_RigidBody.AddForce(ray.direction * (forwardForce + rb.velocity.magnitude));
        }
        else
        {
            Vector3 carRay = carShootCanon.forward + Vector3.up * 0.01f;
            Temporary_RigidBody.AddForce(carRay * (forwardForce + rb.velocity.magnitude));
        }
        Temporary_RigidBody.useGravity = false;

        Invoke("CanShootAgain", 0.5f);

    }

    void CanShootAgain()
    {
        isShooting = false;
    }

    void OnFire() // get fire info from unity new input system
    {
        if (!isShooting)
        {
            canShoot = true;
        }
    }

    public bool IsFiring()
    {
        return isShooting;
    }

    private void OnFireCar()
    {
        canShoot = true;
    }

    // player 1 is fire, and player 2 is ice (at start), so determine which model to use for element
    private void DetermineElement()
    {
        element = elementType == 0 ? elementFirePrefab : elementIcePrefab;
        if (elementSymbol != null)
        {
            elementSymbol.sprite = elementSprites[elementType];
        }
    }

    // switch element for this player
    private void OnSwitchElement()
    {
        if (canSwitchE)
        {
            elementType = (elementType + 1) % 2;
            DetermineElement();
            energy.RemoveEnergy(5);
        }
    }

    // one player may take another's power, and this will see if they can use it 
    public void CanSwitchElement(bool isTrue, int playerThatTookIt)
    {
        canSwitchE = isTrue;

        if (!isTrue)
        {
            elementType = initialElementType;
        }
        DetermineElement();
        EnableSymbol(isTrue, playerThatTookIt - 1); // subtract 1 since player 1 and 2 have 1 and 2 as numbers, so make it 0 and 1 for indexes
    }

    // UI icon on which element player is using
    private void EnableSymbol(bool isTrue, int playerThatTookIt)
    {
        if (elementSymbol == null)
        {
            return;
        }

        if (isTrue && playerThatTookIt == elementType) // if the other player knocked this one out
        {
            elementSymbol.enabled = false;
        }
        else
        {
            elementSymbol.enabled = true;
        }
    }

    // get player input from input system
    private void OnBlockCar(InputValue value)
    {
        isBlocking = value.isPressed;
    }

    void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray.origin, ray.direction * range);

    }

    // interval between light attacks
    private void OnQuickAttackCar()
    {
        // only allow if not blocking/already shooting/on cooldown
        if (isBlocking || !isReady)
        {
            return;
        }
        // check if cooldown over
        if (Time.time > nextAttackTime)
        {
            shootQuickAttack = true;
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    // same as light attack, but individual counter for heavy
    private void OnHeavyAttackCar()
    {
        if (isBlocking || !isReady)
        {
            return;
        }
        // heavy attacks take stamina and longer cooldown
        stamina = pScript.GetStamina();

        if (Time.time > nextAttackTime && (stamina > HeavyAttackCost))
        {
            shootHeavyAttack = true;
            pScript.SetStamina(HeavyAttackCost);
            nextAttackTime = Time.time + 3f / attackRate;
        }

    }




}
