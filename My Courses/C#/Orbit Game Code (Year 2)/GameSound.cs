using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSound : MonoBehaviour
{
    public static AudioClip GravSwitch, KeyCollect;
    public static AudioSource audioTheme;
    public string theme;

    // General script to play sound effects. Other scripts call this.

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
}
