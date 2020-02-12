using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadOnClick : MonoBehaviour
{
    public GameObject loadingImage;

    // Functions to switch scene that can be applied to Unity Canvas

    public void LoadScene(int level)
    {
        SceneManager.LoadScene(level);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
