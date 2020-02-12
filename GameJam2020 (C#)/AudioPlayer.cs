using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioPlayer : MonoBehaviour
{
    // This script makes sure there is atleast 1 audio player object in the Unity game scene so all the sounds can be played
    public static AudioPlayer instance = null;

    public static AudioPlayer Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)   // If there are 2 audio player objs then delete this one
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;            //else this is the main instance
        }

        DontDestroyOnLoad(this.gameObject);
    }

}
