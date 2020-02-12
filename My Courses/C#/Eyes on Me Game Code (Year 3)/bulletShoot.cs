using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

public class bulletShoot : MonoBehaviour
{
    // this script instantiates bullets and shoots them at the aimming reticule

    public GameObject Bullet_Emitter;   // where to instantiate bullet
    public GameObject Bullet;
    public float Bullet_Forward_Force;
    GunAmmoUI ui;


    GameObject Temporary_Bullet_Handler;
    private RaycastHit Ray_Cast_Collision_Data;

    public Image crosshair;
    Ray r;
    int ammoType = 1;
    int [] ammoCountScSpH = {1, 10, 2, 10, 3, 10};  // types of ammo and their counts (the 10s are the counts).
    int currentAmmo;
    public int currentAmmoCount;
    public int ammoMax = 10;
    int InfAmmo = -1;               // which ammo to set infinite, -1 means none

    public Text ammoDisplay;
    public Image [] ammo;
    Vector3 offset;

    bool allowShoot;
    bool isJumping;
    Vector3 changePos;
    Vector3 initPos;
    float fraction = 0;
    float speed = 5f;
    float newScaleValue = 0.035f;

    // Use this for initialization
    void Start()
    {
        ui = GetComponent<GunAmmoUI>();
        currentAmmo = ammoCountScSpH[0];    // ammo type = 1
        currentAmmoCount = ammoCountScSpH[1];   // ammo count = 10
        offset = new Vector3(crosshair.transform.position.x + 15f, crosshair.transform.position.y + 5f, crosshair.transform.position.z );   // aiming reticule location
        allowShoot = GetComponentInParent<ThirdPersonUserControl>().allowControls;
        isJumping = !GetComponentInParent<ThirdPersonCharacter>().m_IsGrounded;
        initPos = GetComponent<Transform>().localScale;     // intial scale of gun
        changePos = new Vector3(newScaleValue, 0.02471398f, newScaleValue);  //big scale of gun
    }

    // Update is called once per frame
    void Update()
    {
        allowShoot = GetComponentInParent<ThirdPersonUserControl>().allowControls;
        isJumping = !GetComponentInParent<ThirdPersonCharacter>().m_IsGrounded;
        
        if (isJumping && fraction < 1)
        {
            fraction += Time.deltaTime * speed;
        }
        if (!isJumping && fraction > 0)
        {
            fraction -= Time.deltaTime * speed;
        }
        GetComponent<Transform>().localScale = Vector3.Lerp(initPos, changePos, fraction);  // if in the air, (lerp) increase the size of the gun, else decrease it

        r = Camera.main.ScreenPointToRay(offset);

        if (allowShoot)
        {
            float direction = Input.GetAxis("Mouse ScrollWheel"); // scroll to change ammo type
            if (direction > 0 && ammoType < 3)
            {
                ammoType = ammoType + 1;
                if (ammoType >= 4)  // scrolling too fast can make the above if statement be missed so this is failsafe check
                    ammoType = 1;
                ui.UpdateAmmoType(ammoType);
                changeAmmoType(ammoType);
            }
            if (direction < 0 && ammoType > 1)
            {
                ammoType = ammoType - 1;
                if (ammoType <= 0)
                    ammoType = 3;
                ui.UpdateAmmoType(ammoType);
                changeAmmoType(ammoType);
            }


            if (Input.GetMouseButtonDown(0) && currentAmmoCount > 0 && Time.timeScale != 0) // instantiate bullet and shoot it at aim
            {
                RaycastHit hit;

                if (Physics.Raycast(r, out hit))
                {

                    Temporary_Bullet_Handler = Instantiate(Bullet, Bullet_Emitter.transform.position, Bullet_Emitter.transform.rotation) as GameObject;
                    Temporary_Bullet_Handler.GetComponent<bullet>().ammoType = ammoType;

                    Rigidbody Temporary_RigidBody = Temporary_Bullet_Handler.GetComponent<Rigidbody>();

                    Temporary_RigidBody.AddForce(r.direction * Bullet_Forward_Force);

                    Temporary_RigidBody.useGravity = false;
                    useShot(currentAmmo);   // subtract ammo count

                }


            }

        }
    }

    public void AddAmmo(int ver)    // used for ammo pickups
    {
        switch (ver)        // 1 is scale ammo, 2 is speed, and 3 is hack (for this function and the rest in the script)
        {
            case 1:
                ammoCountScSpH[1] = 10;
                if (currentAmmo == ammoCountScSpH[0])
                {
                    currentAmmoCount = ammoCountScSpH[1];
                    updateBullets(1);
                }
                break;
            case 2:         
                ammoCountScSpH[3] = 10;
                if (currentAmmo == ammoCountScSpH[2])
                {
                    currentAmmoCount = ammoCountScSpH[3];
                    updateBullets(2);
                }
                break;
            case 3:
                ammoCountScSpH[5] = 10;
                if (currentAmmo == ammoCountScSpH[4])
                {
                    currentAmmoCount = ammoCountScSpH[5];
                    updateBullets(3);
                }
                break;
        }
    }

    public void SetAmmoInfinite(int ver)
    {
        AddAmmo(ver);
        InfAmmo = ver;
        updateBullets(currentAmmo);
    }

    public int[] GetAllAmmoCounts()
    {
        int[] bulletCounts = {ammoCountScSpH[1], ammoCountScSpH[3], ammoCountScSpH[5] };
        return bulletCounts;
    }

    void changeAmmoType(int num)    // 1 is scale ammo, 2 is speed ammo, 3 is hack ammo
    {
        AudioManager.PlaySound("AmmoSwitch");
        switch (num)
        {
            case 1:
                currentAmmo = ammoCountScSpH[0];
                currentAmmoCount = ammoCountScSpH[1];
                updateBullets(1);

                ammoDisplay.color = Color.red;
                break;
            case 2:
                currentAmmo = ammoCountScSpH[2];
                currentAmmoCount = ammoCountScSpH[3];
                updateBullets(2);
                ammoDisplay.color = Color.yellow;
                break;
            case 3:
                currentAmmo = ammoCountScSpH[4];
                currentAmmoCount = ammoCountScSpH[5];
                updateBullets(3);
                ammoDisplay.color = Color.blue;
                break;
        }
    }

    void useShot(int num)   // subtract ammo count when shot
    {
        AudioManager.PlaySound("LaserShoot");
        switch (num)
        {
            case 1:
                if (InfAmmo != 1)
                {
                    currentAmmoCount--;
                    ammoCountScSpH[1]--;
                    updateBullets(1);
                }
                break;
            case 2:
                if (InfAmmo != 2)
                {
                    currentAmmoCount--;
                    ammoCountScSpH[3]--;
                    updateBullets(2);
                }
                break;
                    
            case 3:
                if (InfAmmo != 3)
                {
                    currentAmmoCount--;
                    ammoCountScSpH[5]--;
                    updateBullets(3);
                }
                break;
        }

    }

    void updateBullets(int ver) // UI bullet count on screen, each ammo type has its own count of bullets
    {
        if (ver == 1)
        {
            if (InfAmmo != 1)   // if its not infinite, show number of bullets on screen
            {
                for (int i = 0; i < ammo.Length; i++)
                {
                    ammo[i].color = Color.red;
                    if (i < currentAmmoCount)
                        ammo[i].enabled = true;
                    else
                        ammo[i].enabled = false;
                }
                ammoDisplay.text = "";
            }
            else
            {
                for (int i = 0; i < ammo.Length; i++)           // Else there is no need of a count, set value to INF
                {
                    ammo[i].color = Color.red;
                    ammo[i].enabled = false;
                }
                ammoDisplay.text = "INF";
            }
        }
        else if (ver == 2)
        {
            if (InfAmmo != 2)
            {
                for (int i = 0; i < ammo.Length; i++)
                {
                    ammo[i].color = Color.yellow;
                    if (i < currentAmmoCount)
                        ammo[i].enabled = true;
                    else
                        ammo[i].enabled = false;
                }
                ammoDisplay.text = "";
            }
            else
            {
                for (int i = 0; i < ammo.Length; i++)
                {
                    ammo[i].color = Color.red;
                    ammo[i].enabled = false;
                }
                ammoDisplay.text = "INF";
            }
        }
        else
        {
            if (InfAmmo != 3)
            {
                for (int i = 0; i < ammo.Length; i++)
                {
                    ammo[i].color = Color.blue;
                    if (i < currentAmmoCount)
                        ammo[i].enabled = true;
                    else
                        ammo[i].enabled = false;
                }
                ammoDisplay.text = "";
            }
            else
            {
                for (int i = 0; i < ammo.Length; i++)
                {
                    ammo[i].color = Color.red;
                    ammo[i].enabled = false;
                }
                ammoDisplay.text = "INF";
            }
        }
        
    }



    void OnDrawGizmos() // debug - show aiming line on unity editor
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(r);

    }
}
