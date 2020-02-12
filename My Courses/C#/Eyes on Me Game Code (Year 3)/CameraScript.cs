using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

public class CameraScript : MonoBehaviour
{

    // this script controls camera movement

    public float CameraMoveSpeed = 120f;
    public GameObject CameraFollowObj;
    Vector3 FollowPOS;
    public float clampAngle = 80f;
    public float inputSensitivity = 150f;
    public GameObject CameraObj;
    public GameObject PlayerObj;
    public float camDistanceXToPlayer;
    public float camDistanceYToPlayer;
    public float camDistanceZToPlayer;
    public float mouseX;
    public float mouseY;
    public float smoothX;
    public float smotthY;
    private float rotY = 0f;
    private float rotX = 0f;
    bool zoomIn;

    void Start()
    {
        rotY = transform.localRotation.eulerAngles.y;
        rotX = transform.localRotation.eulerAngles.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        zoomIn = !PlayerObj.GetComponent<ThirdPersonUserControl>().allowControls;
    }

    void Update()
    {
        // get input from mouse movement and apply it camera rotation
        zoomIn = !PlayerObj.GetComponent<ThirdPersonUserControl>().allowControls;
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");
        rotY += mouseX * inputSensitivity * Time.deltaTime;
        rotX += mouseY * inputSensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;
    }

    void LateUpdate()
    {
        CameraUpdater();
        
    }

    void CameraUpdater ()
    {
        Transform target = CameraFollowObj.transform;   // the player obj that the camera will follow

        float step = CameraMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);

        // when the player views the objective, make the camera FOV closer
        if (zoomIn)
        {
            GetComponentInChildren<Camera>().fieldOfView = Mathf.Lerp(GetComponentInChildren<Camera>().fieldOfView, 40, Time.deltaTime * 5);
        }
        else if (!zoomIn)
        {
            GetComponentInChildren<Camera>().fieldOfView = Mathf.Lerp(GetComponentInChildren<Camera>().fieldOfView, 60, Time.deltaTime * 5);
        }
        
    }

}
