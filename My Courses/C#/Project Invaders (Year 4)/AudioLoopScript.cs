using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoopScript : MonoBehaviour
{
    // declare types of music
    public AudioClip MenuMusic;
    public AudioClip GameMusic;
    public AudioSource BossMusic;

    // custom loop time for each music for perfect loop
    private float menuLoopTime = 51.3f;
    private float gameLoopTime = 14.8f;
    private float bossLoopTime = 22.355f;

    // current music source, time, speed and volume for meny/gameplay
    private AudioSource music;
    private float loopTime;
    private float speed = 0.05f;
    public float maxVolume = 0.8f;

    // additional variables
    private float musicLength = 0f;
    private AudioClip musicClip;
    private float bossLength = 0f;
    private AudioClip bossClip;
    private bool keepFadingIn;
    private bool keepFadingOut;
    private bool isGameplayLevel;
    private int sceneNum;

    // reference to players and game manager
    InputManager manager;
    private GameObject[] players;



    // Start is called before the first frame update
    void Start()
    {
        // intantiate variables
        music = GetComponent<AudioSource>();
        manager = InputManager.instance;
        bossClip = BossMusic.clip;
        bossLength = bossClip.length;
    }

    public void SetupMusic()
    {
        // get information from level handler on which level/scene this is
        sceneNum = LevelWinCondition.GetSceneNum();
        isGameplayLevel = (sceneNum > 0) && (sceneNum < 6);

        // menu or credits
        if (sceneNum == 0 || sceneNum == 7) 
        {
            music.clip = MenuMusic;
            loopTime = menuLoopTime;
        }
        // regular level
        else 
        {
            music.clip = GameMusic;
            loopTime = gameLoopTime;
        }

        // handle looping manually
        musicClip = music.clip;
        musicLength = musicClip.length;
        music.Play();
        music.loop = false;

        // set volume based on scene
        if (sceneNum == 6 && maxVolume != 0)
        {
            music.volume = 0.15f;
            music.loop = true;
        }
        else if (sceneNum == 7)
        {
            music.volume = 0.6f;
        }
        else
        {
            music.volume = maxVolume;
        }
    }

    // switch from main music to boss
    IEnumerator FadeIn()
    {
        keepFadingIn = true;
        keepFadingOut = false;

        BossMusic.Play();

        float volBoss = BossMusic.volume;
        float volMain = music.volume;
        BossMusic.volume = maxVolume;

        // gradually increase volume of boss, while decreasing main 
        while (volBoss < maxVolume && keepFadingIn)
        {
            volBoss += speed;
            volMain -= speed;
            music.volume = volMain;
            yield return new WaitForSeconds(0.1f);

        }

        BossMusic.volume = maxVolume;
        music.volume = 0;
        music.Stop();
    }

    // switch from boss music to main
    IEnumerator FadeOut()
    {
        keepFadingIn = false;
        keepFadingOut = true;

        music.Play();
        float volBoss = BossMusic.volume;
        float volMain = music.volume;

        // gradually decrease volume of boss, while increasing main 
        while (BossMusic.volume >= speed && keepFadingOut)
        {
            volBoss -= speed;
            volMain += speed;
            BossMusic.volume = volBoss;
            music.volume = volMain;
            yield return new WaitForSeconds(0.1f);

        }
        BossMusic.volume = 0;
        music.volume = maxVolume;
        BossMusic.Stop();
    }


    // Update is called once per frame
    void Update()
    {
        // change music dynamically based on player status
        CustomLoop();

        // boss only on gameplay levels so not needed to check in menu
        if (isGameplayLevel)
        {
            if (manager.IsBossNearby())
            {
                if (BossMusic.isPlaying)
                {
                    return;
                }
                // smoothly change music to boss if not already playing
                StartCoroutine(FadeIn());
            }
            else
            {
                if (music.isPlaying)
                {
                    return;
                }
                // smoothly change music to regular if not already playing
                StartCoroutine(FadeOut());
            }
        }
    }

    // modify music variables based on where players are in the level
    private void CustomLoop()
    {
        // safety measure
        if (music == null)
        {
            return;
        }

        // if players are not near level boss or are in menu, set music variables according to preset values
        if (music.isPlaying)
        {
            if (music.time >= musicLength)
            {
                music.Play();
                music.time = loopTime;
            }
        }
        // use boss preset values if near boss
        if (BossMusic.isPlaying)
        {
            if (BossMusic.time >= bossLength)
            {
                BossMusic.Play();
                BossMusic.time = bossLoopTime;
            }
        }

    }

    // modify the volume of music that is currently playing
    public void ChangeVolume(float value)
    {
        maxVolume = value;

        if (music.isPlaying)
        {
            music.volume = maxVolume;
        }
        else
        {
            BossMusic.volume = maxVolume;
        }
    }

    // return volume level of music that is currently playing
    public float GetVolume()
    {
        if (music.isPlaying)
        {
            return music.volume;
        }
        else
        {
            return BossMusic.volume;
        }
    }
}
