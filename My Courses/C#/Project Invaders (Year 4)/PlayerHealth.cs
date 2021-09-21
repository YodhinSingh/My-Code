using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    // UI
    private TextMeshProUGUI healthUI;
    private Image healthbar;
    private EnergyBar energy;

    // player properties
    private float health;
    private float stamina;
    [HideInInspector] public bool isReady = false;
    [HideInInspector] public bool isDrone = false;
    private int playerIndex;
    WaitForSeconds vulnerable;
    private bool isInvincable = false;

    // references to other scripts
    private PlayerCombat pCombat;
    private PlayerScript pScript;
    private PlayerInput pInput;

    // models
    public GameObject Drone;
    public GameObject CarDrone;
    public DroneScript droneScript;

    // car shield related
    public GameObject shieldVisual;
    private bool shield;
    private float shieldFactor;

    // particle systems
    public ParticleSystem boost;
    public RespawnParticles respawn;
    public RespawnParticles respawnCar;

    // references to models
    public SkinnedMeshRenderer body;
    public MeshRenderer sword;
    public GameObject face;
    public GameObject carBody;

    private int sceneNum;

    private PlayerAudioSources pAudio;

    // drone/human physics properties
    private Vector3 droneCenter = new Vector3(0, 0.6f, 0.5f); // hard coded values to allow for proper collision as human vs drone
    private Vector3 humanCenter = new Vector3(0, 0.6f, 0);
    private float droneRadius = 1.5f;
    private float humanRadius = 0.6f;

    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        vulnerable = new WaitForSeconds(0.1f);

        pCombat = GetComponent<PlayerCombat>();
        pScript = GetComponent<PlayerScript>();
        pInput = GetComponent<PlayerInput>();
        pAudio = GetComponent<PlayerAudioSources>();

        shield = false;
        shieldFactor = 0.05f; // max out of 1, min is 0
        stamina = 1;

    }

    // set up every level with updated values and new references to current level objects
    public void SetUp(int playerIndex, EnergyBar e, TextMeshProUGUI healthUIBar, Image healthbar)
    {
        this.playerIndex = playerIndex;
        energy = e;
        healthUI = healthUIBar;
        this.healthbar = healthbar;
        sceneNum = LevelWinCondition.GetSceneNum();
        droneScript.SetUp(playerIndex);
        isReady = true;

        // disable human model/controls for car levels
        if (sceneNum == 3)
        {
            body.enabled = false;
            sword.gameObject.SetActive(false);
            face.SetActive(false);
            GetComponent<PlayerUI>().controls = "Car";
        }
        // disable car model/controls for human levels
        else
        {
            body.enabled = true;
            sword.gameObject.SetActive(true);
            face.SetActive(true);
            GetComponent<PlayerUI>().controls = "Player";
            GetComponent<CharacterController>().center = humanCenter;
            GetComponent<CharacterController>().radius = humanRadius;
        }

        health = 100;
        CalculateHealth();
    }

    public float GetHealth()
    {
        return health;
    }

    // get inputs from unity system
    private void OnBlock(InputValue value)
    {
        shield = value.isPressed;
    }

    private void OnBlockCar(InputValue value)
    {
        stamina = pScript.GetStamina();
        // shield only allowed if the player has stamina/power
        if (stamina > 0)
        {
            shield = value.isPressed;
            ShowShield(value.isPressed);
        }
        else
        {
            shield = false;
            ShowShield(false);
        }
    }

    // display visual model for shield
    private void ShowShield(bool turnOn)
    {
        if (sceneNum == 3)
        {
            shieldVisual.SetActive(turnOn);
        }
    }

    // take damage and lower health of player
    public float RecieveDamage(float value, bool isPlayer)
    {
        // cannot harm player in specific scenarios
        if (isInvincable || !isReady)
        {
            return 0;
        }

        stamina = pScript.GetStamina();

        // shield lowers/absorbs damage taken
        shieldFactor = value * 0.01f; 

        if (shield && stamina > 0)
        {
            // lower stamina based on damage taken
            stamina = pScript.SetStamina(shieldFactor);
            // if stamina exists, the shield exists
            if (stamina > 0)
            {
                return 0;
            }
        }

        shield = false;
        ShowShield(false);
        
        // once hit, a brief invulnerable time 
        isInvincable = true;
        StartCoroutine(VulnerableTime());

        // drain health
        health -= value;
        CalculateHealth();

        // player death conditions
        PlayerDie(isPlayer);

        return value;
    }

    private void CalculateHealth()
    {
        if (!isReady)
        {
            return;
        }
        // round and update health
        health = Mathf.Round(health);
        healthUI.text = health.ToString();
        healthbar.fillAmount = health / 100;
    }

    private IEnumerator VulnerableTime()
    {
        yield return vulnerable;
        isInvincable = false;
    }

    // player fell of map, so turn screen black and treat as normal death case 
    public void KillBoxPlayer()
    {
        if (isInvincable || !isReady)
        {
            return;
        }
        
        isInvincable = true;
        health = 0;
        PlayerDie(false);
        pScript.HideScreen();
        StartCoroutine(VulnerableTime());
    }

    // turn screen black while player is moved (handled in pScript)
    public void TeleportPlayer()
    {
        if (isInvincable || !isReady)
        {
            return;
        }

        isInvincable = true;
        pScript.HideScreen();
        StartCoroutine(VulnerableTime());
    }

    // the player can die either by enemies or by the other player. Which case it is determines the fate of player
    private void PlayerDie(bool isPlayer)
    {
        if (health > 0)
        {
            return;
        }

        // if it was the other player, you get 'knocked out' and temporarily spawn as a drone
        if (isPlayer)
        {
            // spawn mini bot 
            health = 0;
            CalculateHealth();
            DisablePlayer(true);
        }
        // otherwise respawn player and drain energy bar
        else
        {
            if (LevelWinCondition.GetSceneNum() != 3)
            {
                // player model has unique respawn particles
                respawn.PlayAnim(body, sword, face);
            }
            else
            {
                // car model has different particles
                respawnCar.PlayAnimObject(carBody);
            }

            // drain 25 energy from the bar for respawning
            float energyRemain = energy.RemoveEnergy(25);
            // if energy is empty, then game over
            if (energyRemain <= 0)
            {
                pScript.AllowControls = false;
                pCombat.isReady = false;
                return;
            }
            // play respawn SFX
            if (pAudio != null)
            {
                pAudio.PlayRespawn();
            }
            health = 100;
            CalculateHealth();
        }

    }

    // turns player into a drone or vice versa
    public void DisablePlayer(bool isTrue)
    {
        // other player that knocked this one out gets control of this one's element while this one is a drone
        InputManager.instance.AllowSwitchElement(isTrue, playerIndex);

        // convert to drone
        if (isTrue)
        {
            gameObject.tag = "Drone";
            gameObject.layer = LayerMask.NameToLayer("Drone");
            pScript.isKnockedOut = true;
            isDrone = true;

            // if car level, convert to special car drone
            if (LevelWinCondition.GetSceneNum() == 3)
            {
                pScript.ConvertCarToDrone(true);
                droneScript.EnableCarDrone(true);
                Drone.SetActive(true);
                GetComponent<PlayerUI>().controls = "CarDrone";
                return;
            }
            // otherwise convert to regular drone
            else
            {
                pInput.SwitchCurrentActionMap("Drone");
                GetComponent<PlayerUI>().controls = "Drone";
                Drone.GetComponent<BoxCollider>().enabled = true;
                GetComponent<CharacterController>().center = droneCenter;
                GetComponent<CharacterController>().radius = droneRadius;
            }
        }
        // convert back to human/car (respawned)
        else
        {
            pScript.isKnockedOut = false;
            health = 100;
            CalculateHealth();
            isDrone = false;
            if (pAudio != null)
            {
                pAudio.PlayRespawn();
            }
            // convert to car for level 3
            if (LevelWinCondition.GetSceneNum() == 3)
            {
                respawnCar.PlayAnimObject(null);
                pScript.ConvertCarToDrone(false);
                droneScript.EnableCarDrone(false);
                Drone.SetActive(false);
                GetComponent<PlayerUI>().controls = "Car";
                return;
            }
            // else convert to human
            else
            {
                respawn.PlayAnimObject(null);
                pInput.SwitchCurrentActionMap("Player");
                GetComponent<PlayerUI>().controls = "Player";
                pScript.SetTagAndLayer();
                GetComponent<CharacterController>().center = humanCenter;
                GetComponent<CharacterController>().radius = humanRadius;
            }

        }

        // turn on/off models based on situation
        Drone.SetActive(isTrue);
        body.enabled = !isTrue;
        sword.gameObject.SetActive(!isTrue);
        face.SetActive(!isTrue);
        GetComponent<CapsuleCollider>().enabled = !isTrue;

    }

    // drone booster particle system
    public void JumpBooster()
    {
        // play whenever a drone jumps
        if (isDrone)
        {
            boost.Play();
        }
    }

}
