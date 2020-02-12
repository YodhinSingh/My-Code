using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bulletShoot : MonoBehaviour
{
    public GameObject Bullet_Emitter;
    public GameObject Bullet;
    public float Bullet_Forward_Force;

    GameObject Temporary_Bullet_Handler;
    Ray r;

    [SerializeField] private float CoolDownTimer = 10f;
    private float timer;
    private int bulletCount;
    [SerializeField] private int MaxBullets = 10;
    private bool allowShoot;

    public GameObject Ammobar;

    // Use this for initialization
    void Start()
    {
        allowShoot = true;
        timer = 0f;
        bulletCount = MaxBullets;
    }

    // Update is called once per frame
    void Update()
    {

        r = new Ray(Bullet_Emitter.transform.position, transform.forward);

        if (Input.GetMouseButtonDown(0) && bulletCount > 0 && Time.timeScale != 0)  // Instantiate a bullet and shoot it

        {

            GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);

            Temporary_Bullet_Handler = Instantiate(Bullet, Bullet_Emitter.transform.position, transform.rotation) as GameObject;

            Rigidbody Temporary_RigidBody = Temporary_Bullet_Handler.GetComponent<Rigidbody>();
            Temporary_Bullet_Handler.transform.Rotate(Vector3.left * 90);   // Face right direction
            Temporary_RigidBody.AddForce(transform.forward * Bullet_Forward_Force);
            
            Temporary_RigidBody.useGravity = false;

            bulletCount--;
            if (bulletCount <= MaxBullets)
            {
                StartCoroutine("RefillBullets");
            }

        }

        if (Ammobar != null)    // set ammo bar length to indicate number of bullets
        {
            Ammobar.GetComponent<RectTransform>().localScale = new Vector3(bulletCount*1.0f / MaxBullets, Ammobar.GetComponent<RectTransform>().localScale.y, Ammobar.GetComponent<RectTransform>().localScale.z);
        }

    }

    private IEnumerator RefillBullets() // refill bullets after 3 seconds cooldown
    {
        yield return new WaitForSeconds(3);
        bulletCount++;

    }




    void OnDrawGizmos() // show ray in editor
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(r);

    }
}
