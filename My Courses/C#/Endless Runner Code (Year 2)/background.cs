using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background : MonoBehaviour {

    public float speed;
    public float Xend;
    public float Xstart;

    // This sets the position of the backgrounds based on camera movement to get parallax scrolling

    private Transform cam;

    void Awake()
    {
        cam = Camera.main.transform;
    }

    private void Update() {
        cam = Camera.main.transform;
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        if (transform.position.x < Xend + cam.position.x) {
            Vector2 pos = new Vector2(Xstart + cam.position.x, transform.position.y);
            transform.position = pos;
        }
    }
}
