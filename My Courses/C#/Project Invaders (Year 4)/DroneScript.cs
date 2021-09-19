using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneScript : MonoBehaviour
{
    // references to models
    public GameObject Drone;
    public GameObject CarDrone;

    // drone properties
    private float speed = 0.05f;
    public float hackRange;

    private Transform otherPlayer;

    // references to UI
    public Image hackBar;
    public Image hackBarCar;
    private Image curHackBar;
    private PlayerHealth pHealth;

    // references to drone parts
    public Transform[] droneSpinners;

    public MeshRenderer[] droneColours;
    public MeshRenderer[] droneCarColours;
    public Material ice;

    private int sceneNum;

    public void SetUp(int playerIndex)
    {
        // choose drone model based on level
        sceneNum = LevelWinCondition.GetSceneNum();

        // level 3 has a car drone
        if (sceneNum == 3)
        {
            curHackBar = hackBarCar;
            hackBar.enabled = false;
            hackBarCar.enabled = true;
        }
        // all other levels have regular flying drone
        else
        {
            curHackBar = hackBar;
            hackBarCar.enabled = false;
            hackBar.enabled = true;
        }


        if (playerIndex != 2) // if p1 then no need to change anything
        {
            return;
        }

        // change colours of drone for p2
        ChangeColours(droneColours);
        ChangeColours(droneCarColours);
    }

    // enable model of car drone
    public void EnableCarDrone(bool isTrue)
    {
        Drone.SetActive(!isTrue);
        CarDrone.SetActive(isTrue);
    }

    // enable model of drone
    public void EnableDrone(bool isTrue)
    {
        CarDrone.SetActive(!isTrue);
        Drone.SetActive(isTrue);
    }

    // give p2 blue colours instead of red
    private void ChangeColours(MeshRenderer[] bodyToChange)
    {

        for (int i = 0; i < bodyToChange.Length; i++)
        {
            Material[] mColours = bodyToChange[i].materials;

            for (int j = 0; j < mColours.Length; j++)
            {
                if (mColours[j].name.Equals("Neon Red Building (Instance)"))
                {
                    mColours[j] = ice;
                }
            }

            bodyToChange[i].sharedMaterials = mColours;

        }
    }

    // set properties of drone at start which are always reset
    private void OnEnable()
    {
        // reset hack value and UI
        hackBar.fillAmount = 0;
        hackBarCar.fillAmount = 0;
        int playerIndex = GetComponentInParent<PlayerScript>().GetPlayerIndex();
        pHealth = GetComponentInParent<PlayerHealth>();

        // each player must have a reference to the other
        if (playerIndex == 0)
        {
            otherPlayer = GameObject.FindGameObjectWithTag("PlayerTwo").transform;
        }
        else
        {
            otherPlayer = GameObject.FindGameObjectWithTag("PlayerOne").transform;
        }

        // car drone level has different stats then regular drone
        sceneNum = LevelWinCondition.GetSceneNum();
        if (sceneNum == 3)
        {
            hackRange = 65f;
            EnableCarDrone(true);
        }
        else
        {
            hackRange = 20f;
            EnableCarDrone(false);
        }
        
    }

    private void Update()
    {
        if (sceneNum != 3)
        {
            // rotate the helicopter drone blades
            for (int i = 0; i < droneSpinners.Length; i++)
            {
                droneSpinners[i].Rotate(0, 500f * Time.deltaTime, 0);
                Vector3 temp = droneSpinners[i].localRotation.eulerAngles;
                float ZRot = -Mathf.PingPong(Time.time * 10f, 15);
                if (i > 1)
                {
                    ZRot *= -1;
                }

                droneSpinners[i].localRotation = Quaternion.Euler(0, temp.y, ZRot);
            }
        }



        if (otherPlayer == null || curHackBar == null)
        {
            return;
        }

        // check distance from drone to other player
        if (Vector3.Distance(transform.position, otherPlayer.position) < hackRange)
        {
            // if close enough, then show that hacking is working through UI icon
            curHackBar.fillAmount += Time.deltaTime * speed;
            if (curHackBar.fillAmount >= 1)
            {
                curHackBar.fillAmount = 1;
                pHealth.DisablePlayer(false);
            }
        }
    }
}
