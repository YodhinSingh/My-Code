using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CamIntro : MonoBehaviour
{

    // This script sets the camera position and movement for the opening cutscene. values changed based on timer.

    float timer = 0;

    public float timeTillScene0;
    public float timeTillScene1;
    public float timeTillScene2;
    public float timeTillScene3;
    public float timeTillScene4;

    public Vector3 Scene0Pos;
    public Vector3 Scene0Rot;

    public Vector3 Scene1Pos;
    public Vector3 Scene1Rot;

    public Vector3 Scene2Pos;
    public Vector3 Scene2Rot;

    public Vector3 Scene3Pos;
    public Vector3 Scene3Rot;

    public Vector3 Scene4Pos;

    public GameObject holoDoor;
    public GameObject techGun;
    public GameObject alarm;
    public GameObject player;
    public GameObject shutdown;
    public GameObject audioSound;


    bool newStartRot = false;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Camera>().fieldOfView = 40;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer < timeTillScene0)
        {
            transform.localPosition = Scene0Pos;
            transform.localRotation = Quaternion.Euler(Scene0Rot.x, Scene0Rot.y, Scene0Rot.z);
            
        }
        else if (timer < timeTillScene1)
        {
            transform.localPosition = Scene1Pos;
            if (!newStartRot)
            {
                transform.localRotation = Quaternion.Euler(0, 305, 0);
                newStartRot = true;
            }
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(Scene1Rot.x, Scene1Rot.y, Scene1Rot.z), 0.99f * Time.deltaTime);

            if (alarm.activeInHierarchy && timer > timeTillScene1 - 5)
                alarm.SetActive(false);
            if (holoDoor.activeInHierarchy && timer > (timeTillScene1 - 4))
            {
                holoDoor.SetActive(false);
                audioSound.SetActive(true);
            }
            if (!shutdown.activeInHierarchy && timer > (timeTillScene1 - 5))
                shutdown.SetActive(true);
        }
        else if (timer < timeTillScene2)
        {
            transform.localPosition = Scene2Pos;
            transform.localRotation = Quaternion.Euler(Scene2Rot.x, Scene2Rot.y, Scene2Rot.z);
            if (!techGun.activeInHierarchy)
                techGun.SetActive(true);
        }
        else if (timer < timeTillScene3)
        {
            techGun.transform.SetParent(player.transform);
            techGun.transform.localPosition = new Vector3(0,0.93f,0);
            techGun.transform.localRotation = Quaternion.Euler(0,0,0);

            GetComponent<Camera>().fieldOfView = 30;
            transform.localPosition = Scene3Pos;
            transform.localRotation = Quaternion.Euler(Scene3Rot.x, Scene3Rot.y, Scene3Rot.z);
        }
        else if (timer < timeTillScene4)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Scene4Pos, 1f * Time.deltaTime);
            GetComponent<Camera>().fieldOfView = Mathf.Lerp(30, 40, 1f * Time.deltaTime);
            audioSound.GetComponent<AudioSource>().volume = 0.03f;
        }
        else if (timer > timeTillScene4)
        {
            SceneManager.LoadScene(1);
        }
    }
}
