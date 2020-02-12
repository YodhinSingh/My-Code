using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShooter : MonoBehaviour
{
    // Shoots bullet. similar to bullet shooter script

    public GameObject Bullet_Emitter;
    public GameObject Bullet;
    public float Bullet_Forward_Force;

    GameObject Temporary_Bullet_Handler;
    private RaycastHit Ray_Cast_Collision_Data;

    float timer = 0;
    public float timeTillNextShot = 4;
    public bool keepShooting = true;

    Ray r;
    AudioSource a;

    // Use this for initialization
    void Start()
    {
        if (transform.localRotation.y == 0) // determine direction of turret and where to point ray
            r = new Ray(this.transform.position, Vector3.forward);
        else
            r = new Ray(this.transform.position, Vector3.forward * -1);
        a = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (keepShooting)
            timer -= Time.deltaTime;

        if (timer <= 0 && Time.timeScale != 0)      // every time seconds, instantiate a bullet and shoot it forward
        {

            Temporary_Bullet_Handler = Instantiate(Bullet, Bullet_Emitter.transform.position, Bullet_Emitter.transform.rotation) as GameObject;
            Rigidbody Temporary_RigidBody = Temporary_Bullet_Handler.GetComponent<Rigidbody>();
            Temporary_RigidBody.AddForce(r.direction * Bullet_Forward_Force);
            a.PlayOneShot(a.clip);
            timer = timeTillNextShot;
        }

    }





    

    


}
