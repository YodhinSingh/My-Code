using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnParticles : MonoBehaviour
{
    // VFX
    public ParticleSystem[] respawns;
    public GameObject circles;
    float timer;
    
    // models
    private SkinnedMeshRenderer body;
    private MeshRenderer sword;
    private GameObject face;
    private GameObject bodyG;


    private void Awake()
    {
        // set VFX timer based on animation length
        timer = respawns[0].main.duration;
    }

    // plays re-spawn animation
    public void PlayAnim(SkinnedMeshRenderer b, MeshRenderer s, GameObject f)
    {
        // turns off player models
        if (b != null)
        {
            b.enabled = false;
            s.gameObject.SetActive(false);
            f.SetActive(false);
            body = b;
            sword = s;
            face = f;
        }
        // play a VFX and then turn on player models after
        ExecuteParticles();
        Invoke("ShowPlayer", timer / 2);
    }

    // re-spawn animation for cars
    public void PlayAnimObject(GameObject g)
    {
        // turn car model off
        if (g != null)
        {
            g.SetActive(false);
            bodyG = g;
        }
        // play VFX and then turn back on car model
        ExecuteParticles();
        Invoke("ShowPlayerCar", timer / 2);
    }

    // Particle system for respawns
    private void ExecuteParticles()
    {
        circles.SetActive(true);
        respawns[0].Emit(1);
        respawns[1].Emit(1);

        Invoke("CloseAnim", timer);
    }

    // re-show player after respawn
    private void ShowPlayer()
    {
        // re-enable body
        if (body != null)
        {
            body.enabled = true;
            sword.gameObject.SetActive(true);
            face.SetActive(true);
        }
        // set refernce to null again so future calls wont assume it's the player body thats needed to be disabled
        bodyG = null;
        body = null;
        sword = null;
    }

    // re-show car model respawn
    private void ShowPlayerCar()
    {
        // re-enable car
        if (bodyG != null)
        {
            bodyG.SetActive(true);
        }
        // set refernce to null again so future calls wont assume it's car body thats needed to be disabled
        bodyG = null;
        body = null;
        sword = null;
    }


    // turn off respawn VFX
    private void CloseAnim()
    {
        CancelInvoke();
        circles.gameObject.SetActive(false);

    }
}
