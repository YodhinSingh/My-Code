using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManagerScript : MonoBehaviour
{

    //This contains all the sounds to play and is called in other scripts

    public static AudioClip jumpSound, PowerUpSound, ExplodeSound;
    public static AudioSource audioTheme;
    public string theme;
    public static SoundManagerScript instance;


    void Awake()
    {
        if (instance == null)   // only keep one instance of this obj in every scene, delete if another is already there
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
        jumpSound = Resources.Load<AudioClip>("Jump");
        PowerUpSound = Resources.Load<AudioClip>("PowerUp");
        ExplodeSound = Resources.Load<AudioClip>("Explode");

        audioTheme = GetComponent<AudioSource>();
        audioTheme.clip = Resources.Load<AudioClip>(theme);
        audioTheme.Play();
    }


    public static void PlaySound(string clip)
    {
        switch (clip) 
        {
            case "Jump":
                audioTheme.PlayOneShot(jumpSound);
                break;
            case "PowerUp":
                audioTheme.PlayOneShot(PowerUpSound);
                break;
            case "Explode":
                audioTheme.PlayOneShot(ExplodeSound);
                break;
        }
    }
    void Update()
    {
        checkScene();
    }

    public void checkScene(){
        if (SceneManager.GetActiveScene().buildIndex == 7)
        {
            audioTheme.volume = 0f;
        }
        else
        {
            audioTheme.volume = 0.5f;
        }
    }
}