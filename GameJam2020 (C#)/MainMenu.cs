using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenControlMenu()
    {
        SceneManager.LoadScene("Controls");
    }

    public void PlayGame()
    {
        Destroy(GameObject.Find("AudioPlayer"));
        SceneManager.LoadScene("Game");
    }

}
