using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle2 : MonoBehaviour
{
    public float speedZ;
    public float force;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + new Vector3(0, 0, -1) * speedZ);   // moves towards circuit unless something is blocking its path
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name.Equals("barrier2")) // distroy barrier around circuit upon collision so player can shoot circuit
        {
            col.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
