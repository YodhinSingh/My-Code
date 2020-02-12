using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRun : MonoBehaviour
{
    public float speed;

    //Have the camera move across the scene (not related to player)

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}
