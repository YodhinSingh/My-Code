using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverCarController : MonoBehaviour
{
    // references to car parts
    public Transform[] hoverPoints = new Transform[4];
    public Rigidbody rb;
    public GameObject prop;
    public GameObject CM;
    public GameObject Cannon;
    public GameObject CannonBase;
    private float multiplier;
    private bool isHidden = false;
    public Transform playerT;

    // Start is called before the first frame update
    void Start()
    {
        // car properties
        multiplier = 5;
        rb.centerOfMass = CM.transform.localPosition;
        rb.mass = 1 * multiplier;
        rb.drag = 0.3f;
        rb.angularDrag = 0.25f;
        // do not allow car to flip on other axis
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
    }

    public void Move(float turn, float forward, bool m)
    {
        float locVelZ = transform.InverseTransformDirection(rb.velocity).z;

        // lower speeds have faster acceleration
        if (((rb.velocity.magnitude < 20 && rb.velocity.magnitude > 0) || m) && forward > 0)
        {
            rb.AddForceAtPosition(Time.deltaTime * transform.forward * forward * 500f * multiplier, prop.transform.position, ForceMode.Acceleration);
        }
        // reverse speed should always be slower
        else if (forward < 0 && locVelZ < -0.2f)
        {
            rb.AddForceAtPosition(Time.deltaTime * transform.forward * forward * 500f * multiplier, prop.transform.position);
            // reverse direction controls on reverse
            turn = -turn;
        }
        // faster speeds have slower acceleration
        else
        {
            rb.AddForceAtPosition(Time.deltaTime * transform.forward * forward * 1500f * multiplier, prop.transform.position);
        }

        // handle turning
        rb.AddTorque(Time.deltaTime * transform.up * turn * 900f * multiplier);

        // check if any obstacles below each hover thruster and move it up/down accordingly
        for (int i = 0; i < hoverPoints.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(hoverPoints[i].position, -transform.up, out hit, 3f))
            {
                rb.AddForceAtPosition(Time.deltaTime * transform.up * Mathf.Pow(3f - hit.distance, 1.5f) / 3f * 250f * multiplier, hoverPoints[i].position);
            }
        }

        // friction of road slows down car
        Vector3 friction = -Time.deltaTime * transform.TransformVector(Vector3.right) * transform.InverseTransformVector(rb.velocity).x * 50;
        rb.AddTorque(friction);
        
        // keep turning controls more sturdy and less slippery
        if (Mathf.Abs(turn) <= 0.1f)
        {
            rb.angularVelocity = Vector3.zero;
        }
        Vector3 localVelocity = Quaternion.Inverse(transform.rotation) * rb.velocity;
        localVelocity.x *= 0.98f;
        rb.velocity = transform.rotation * localVelocity;

        KeepCarYAxisStable();

    }

    // do not let hover car get too high/low
    private void KeepCarYAxisStable()
    {
        if (playerT.position.y < 5.5f)
        {
            playerT.position = new Vector3(playerT.position.x, 5.7f, playerT.position.z);
        }
    }

    // turn thrusters based on player turning
    public void RotateThrusters(float turn, float forward)
    {
        if (isHidden)
        {
            return;
        }

        for (int i = 0; i < hoverPoints.Length; i++)
        {
            Vector3 rot = hoverPoints[i].localRotation.eulerAngles;
            rot.x = 30 * forward;
            rot.z = 30 * turn;
            hoverPoints[i].localRotation = Quaternion.Euler(rot.x, 0, rot.z);
        }
    }

    // disable/enable car model
    public void HideCar(bool isTrue)
    {
        GetComponent<MeshRenderer>().enabled = !isTrue;
        Cannon.SetActive(!isTrue);
        CannonBase.SetActive(!isTrue);
        for (int i = 0; i < hoverPoints.Length; i++)
        {
            hoverPoints[i].gameObject.SetActive(!isTrue);
        }
        StartThrusters();
    }
    
    // turn on thrusters for first time
    private void StartThrusters()
    {
        if (!hoverPoints[0].gameObject.activeInHierarchy)
        {
            return;
        }

        for (int i = 0; i < hoverPoints.Length; i++)
        {
            hoverPoints[i].GetComponent<ParticleSystem>().Play();
        }
    }
}
