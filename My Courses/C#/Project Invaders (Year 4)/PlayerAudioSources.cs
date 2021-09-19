using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioSources : MonoBehaviour
{
    // Handle audio sources for player model

    AudioSource playerAudio;

    // holds all clips that are used for SFX
    public AudioClip[] clips;
    // clip properties
    private float normalVol = 0.1f;
    private float highVol = 0.9f;


    private void Awake()
    {
        playerAudio = GetComponent<AudioSource>();
    }

    public void PlaySwordAttack()
    {
        // choose random audio clip source for melee attacks
        int rand = Random.Range(3, 5);

        playerAudio.PlayOneShot(clips[rand], normalVol);
    }

    public void PlayCarGunShoot()
    {
        // choose random audio clip source for gun shots
        int rand = Random.Range(1, 3);

        playerAudio.PlayOneShot(clips[rand], normalVol);
    }

    public void PlayRespawn()
    {
        playerAudio.PlayOneShot(clips[5], highVol);
    }


    public void PlayElementShoot()
    {
        playerAudio.PlayOneShot(clips[7], normalVol);
    }


    public void PlayCarEngine(bool isTrue)
    {
        // for car levels, play engine reving sound with loop
        if (isTrue)
        {
            if (playerAudio.isPlaying)
            {
                return;
            }

            playerAudio.volume = 0.1f;
            playerAudio.Play();
            playerAudio.loop = true;
        }
        // otherwise do not
        else
        {
            if (!playerAudio.isPlaying)
            {
                return;
            }
            playerAudio.loop = false;
            playerAudio.Stop();
            playerAudio.volume = 1f;
        }

    }
}
