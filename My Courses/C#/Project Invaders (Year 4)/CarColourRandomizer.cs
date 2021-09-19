using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarColourRandomizer : MonoBehaviour
{
    // get references to colour materials, etc
    public Material[] colours = new Material[5];
    public int[] indicies = new int[2];
    public bool isPlayer = false;
    public bool isEnemy = false;

    public MeshRenderer m;

    // Start is called before the first frame update
    void Start()
    {
        int index;

        if (m != null)
        {
            // get list of car model's materials
            Material[] mCar = m.materials;

            if (isPlayer)
            {
                // find out if this is player 1 or 2
                index = GetComponentInParent<PlayerScript>().GetPlayerIndex();
                // indicies array specifies which indicies in mCar should be modified
                for (int i = 0; i < indicies.Length; i++)
                {
                    mCar[indicies[i]] = colours[index];
                }
            }
            // if this car model is neither player nor enemy (i.e. environment car model)
            else if (!isEnemy)
            {
                for (int i = 0; i < indicies.Length; i++)
                {
                    // give random colour each time
                    index = Random.Range(0, colours.Length);
                    mCar[indicies[i]] = colours[index];
                }
            }
            // apply the change of materials to the car
            m.sharedMaterials = mCar;
        }
        // if this is a player/enemy car, then activate the hover car thruster particle systems
        if (IsThisCarMoving())
        {
            ActivateThrusters();
        }
        // if its an enemy give parent enemy script a reference to the thrusters
        if (isEnemy)
        {
            GetComponentInParent<EnemyScript>().thrusters = GetThrusters(); ;
        }
    }

    // set colour for car based on enemy stats
    public void EnemyCarColourSetUp(int weakness, bool isBoss)
    {
        if (m == null)
        {
            return;
        }

        Material[] mCar = m.materials;

        // materal index on model is based on whether its a boss or regular enemy model
        int index = isBoss? 1 : 0;

        // colour is set based on elemental weakness
        switch (weakness)
        {
            case 0: // fire weakness creature
                mCar[index] = colours[0];
                break;
            case 1: // ice weakness creature
                mCar[index] = colours[1];
                break;
            default: // normal (no direct weakness)
                mCar[index] = colours[2];
                break;
        }

        // apply modifications to model
        m.sharedMaterials = mCar;
    }

    // only player/enemy car models should be movable
    public bool IsThisCarMoving()
    {
        return isPlayer || isEnemy;
    }

    // turn on particle systems for each thruster
    public void ActivateThrusters()
    {
        ParticleSystem[] p = GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < p.Length; i++)
        {
            p[i].GetComponent<ParticleSystem>().Play();
        }
    }

    // make particle system stronger for thrusters
    public void IncreaseThrusters()
    {
        ParticleSystem[] p = GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < p.Length; i++)
        {
            var main = p[i].GetComponent<ParticleSystem>().main;
            main.startSize = 5;
        }
    }

    // get reference to thrusters
    public Transform[] GetThrusters()
    {
        ParticleSystem[] p = GetComponentsInChildren<ParticleSystem>();
        Transform[] t = new Transform[p.Length];
        for (int i = 0; i < p.Length; i++)
        {
            t[i] = p[i].transform;
        }
        return t;
    }
}
