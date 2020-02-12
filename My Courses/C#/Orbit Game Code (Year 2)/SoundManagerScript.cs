using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip GravSwitch, KeyCollect;
    public static AudioSource audioTheme;
    public string theme;
    public static SoundManagerScript instance;

    // this is called to play all sounds. Other scripts call functions from this.

    void Awake()
    {
        
        if (instance == null)       // make sure there is only 1 instance of this obj, else destroy this one.
        {
            instance = this;
            
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        
        
        
    }

    // Start is called before the first frame update
    void Start()
    {
        GravSwitch = Resources.Load<AudioClip>("GravSwitch");
        KeyCollect = Resources.Load<AudioClip>("KeyCollect");
        audioTheme = GetComponent<AudioSource>();
        audioTheme.volume = 0.8f;
        audioTheme.clip = Resources.Load<AudioClip>(theme);
        audioTheme.Play();
    }


    public static void PlaySound(string clip)
    {
        switch (clip) 
        {
            case "GravSwitch":
                audioTheme.PlayOneShot(GravSwitch);
                break;
            case "KeyCollect":
                audioTheme.PlayOneShot(KeyCollect);
                break;
        }
    }
    void Update()
    {
        
        checkScene();
    }

    public void checkScene()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3 || SceneManager.GetActiveScene().buildIndex == 4 || SceneManager.GetActiveScene().buildIndex == 1)    // Win/lose/controls screen
        {
            audioTheme.volume = 0f;
        }
        else
        {
            audioTheme.volume = 0.8f; // game/menu scenes
        }
    }

}