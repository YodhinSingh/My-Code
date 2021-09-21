using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleKey : MonoBehaviour
{
    // key properties
    public int keyNumber;
    private AudioSource a;
    private bool hasTaken = false;

    // references to puzzles
    public PuzzleOne puz1;
    public PuzzleTwo puz2;
    public PuzzleThree puz3;
    public PuzzleKeyBoss handler;

    private void Awake()
    {
        a = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // only allow player models (not even drones)
        if ((other.gameObject.tag == "PlayerOne" || other.gameObject.tag == "PlayerTwo" ))
        {
            // on collecting this key, tell handler method that this key has been collected
            PuzzleHandler();
        }
    }

    // sends report to corresponding puzzle that it has been collected
    private void PuzzleHandler()
    {
        // safety measure to not send report twice
        if (hasTaken)
        {
            return;
        }

        if (a == null)
        {
            a = GetComponent<AudioSource>();
        }

        // play SFX on collection
        a.Play();

        // each puzzle has different requirements for the keys
        switch (keyNumber)
        {
            case 1:
                puz1.PuzzleComplete();
                break;
            case 2:
                puz2.KeyTaken();
                break;
            case 3:
                puz3.KeyTaken();
                break;
        }

        // tell puzzle handler class that this key has been collected as well
        hasTaken = true;
        handler.KeyUnlocked(keyNumber);

        // turn off key model
        GetComponent<MeshRenderer>().enabled = false;
        Invoke("RemoveKey", a.clip.length);

    }

    private void RemoveKey()
    {
        gameObject.SetActive(false);
        GetComponent<MeshRenderer>().enabled = true;
    }
}
