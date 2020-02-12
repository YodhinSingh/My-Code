using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    // general script for other scripts to call to play sounds

    public static AudioClip AmmoSwitch, AmmoPickup, LaserShoot;
    public static AudioSource audioTheme;
    public string theme;

    public GameObject PauseMenuUI;


    void Awake()
    {   
        AmmoSwitch = Resources.Load<AudioClip>("AmmoSwitch");
        AmmoPickup = Resources.Load<AudioClip>("CoinCollect");
        LaserShoot = Resources.Load<AudioClip>("LaserShoot");


        audioTheme = GetComponent<AudioSource>();
        audioTheme.clip = Resources.Load<AudioClip>(theme);
        audioTheme.Play();
        audioTheme.spatialBlend = 0;

        checkScene();
    }

    private void Update()
    {
        checkScene();
    }


    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "AmmoPickup":
                audioTheme.PlayOneShot(AmmoPickup);
                break;
            case "AmmoSwitch":
                audioTheme.PlayOneShot(AmmoSwitch);
                break;
            case "LaserShoot":
                audioTheme.PlayOneShot(LaserShoot);
                break;
        }
    }

    public void checkScene()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)      //Game
        {
            audioTheme.loop = true;
            if (PauseMenuUI != null && PauseMenuUI.activeInHierarchy)
            {
                audioTheme.volume = 0.05f;
            }
            else
            {
                audioTheme.volume = 0.12f;
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 0)     //Menu
        {
            audioTheme.loop = true;
            audioTheme.volume = 1f;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)     //Game Over
        {
            audioTheme.volume = 0.1f;
            audioTheme.loop = false;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3)          //Win
        {
            audioTheme.volume = 0.5f;
            audioTheme.loop = false;
        }
    }
}
