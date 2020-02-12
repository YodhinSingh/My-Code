using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool endConditionWin;
    public static bool endConditionLose;
    public bool EscapeMenuIsOpened;
    public GameObject HUD;
    public GameObject EscapeMenu;
    public CanvasGroup HUDGroup;
    public GameObject WinMenuCanvas;
    public GameObject LoseMenuCanvas;
    public GameObject AudioPlayer;

    public AudioClip menu, game;

    // Start is called before the first frame update
    void Start()
    {
        EscapeMenuIsOpened = false;
        endConditionWin = false;
        endConditionLose = false;
        AudioPlayer = GameObject.Find("AudioPlayer");
    }

    // Update is called once per frame
    void Update()
    {
        if (endConditionWin || endConditionLose)
        {
            HUDGroup.interactable = false;
            if (endConditionWin)
            {
                WinMenu();
            }
            else if (endConditionLose)
            {
                LoseMenu();
            }
        }
        else if (Input.GetKeyUp("escape"))
        {
            if (EscapeMenuIsOpened)
            {
                CloseEscapeMenu();
            }
            else if (!EscapeMenuIsOpened)
            {
                OpenEscapeMenu();
            }
        }
    }

    public void OpenEscapeMenu()
    {
        Time.timeScale = 0f;
        EscapeMenuIsOpened = true;
        HUD.SetActive(false);
        EscapeMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseEscapeMenu()
    {
        EscapeMenuIsOpened = false;
        HUD.SetActive(true);
        EscapeMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        AudioPlayer.GetComponent<AudioSource>().Play();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        AudioPlayer.GetComponent<AudioSource>().clip = menu;
        AudioPlayer.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("MainMenu");
    }

    void WinMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        WinMenuCanvas.SetActive(true);
    }

    void LoseMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        LoseMenuCanvas.SetActive(true);
    }
}
