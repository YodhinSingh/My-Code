using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowEffect : MonoBehaviour
{
    Light lt;
    Camera cam;
    Vector2 dist = new Vector2(0f,0f);

    /* This script creates like a spot light on the player. As the player reaches closer to the outer edge of the maze, the spot light gets bigger/lighter. It gets smaller/concentrated
    if the player goes closer to the center
    */

    // Start is called before the first frame update
    void Start()
    {
        lt = GetComponent<Light>();
        lt.type = LightType.Spot;
        cam = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {
        dist = new Vector2(cam.transform.position.x, cam.transform.position.y);
        lt.transform.position = new Vector3(lt.transform.position.x, lt.transform.position.y, (Mathf.Abs(dist.sqrMagnitude) / -1000) - 1);
    }

}
