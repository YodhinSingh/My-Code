using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunAim : MonoBehaviour
{
    float rotY = 0f;
    float rotX = 0f;
    float clampAngle = 5f;
    float mouseSensitivity = 150f;
    public Transform target;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        rotY = transform.localRotation.eulerAngles.y;
        rotX = transform.localRotation.eulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");
        

        rotY += mouseX * mouseSensitivity * Time.deltaTime;
        rotX += mouseY * mouseSensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle*3.5f, clampAngle*3f);

        Quaternion localRotation = Quaternion.Euler(rotX, rotY + 0f, 0f);   // set rotation of aim point based on mouse input
        transform.rotation = localRotation;

    }
}
