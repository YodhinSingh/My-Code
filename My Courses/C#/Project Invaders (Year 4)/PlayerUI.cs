using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    // references to other scripts
    private CutsceneScript cutscene;
    private PlayerScript pScript;
    private PlayerInput pInput;

    // level and player properties
    private int sceneNum;
    private int playerNum;
    public string controls;

    // UI visuals
    public RectTransform pointer;

    // UI control properties
    bool isKAndM = false;
    Vector3 pos;
    private float sensitivity = 30;
    private float sensitivityOriginal = 30;
    private bool isUIMode;

    // title page buttons
    Button helpButton, playButton, quitButton, backButton, nextLayoutButton, prevLayoutButton;
    private RectTransform helpT, playT, quitT, backT, nextLayoutT, prevLayoutT;
    private GameObject helpMenu, startMenu;

    // in-game buttons
    private GameObject pause, winMenuObj, loseMenuObj;
    private Button pauseQuit, pauseMenu, winNextLevel, winMenu, loseRetry, loseMenu;
    private RectTransform pauseQuitT, pauseMenuT, winNextLevelT, winMenuT, loseRetryT, loseMenuT;

    // common buttons
    private Slider volume;
    private RectTransform volumeT;

    // screen properties
    private float screenW;
    private float screenH;

    private void Start()
    {
        sceneNum = LevelWinCondition.GetSceneNum();
        controls = "UI";
        screenW = 1920;// Screen.currentResolution.width;
        screenH = 1080;// Screen.currentResolution.height;

    }

    private void Update()
    {
        MoveCursor();
    }

    private void MoveCursor()
    {
        if (pointer == null || !pointer.gameObject.activeInHierarchy)
        {
            return;
        }

        if (isUIMode)
        {
            // update pointer from input
            pointer.localPosition += pos;
            // do not let pointer leave screen view
            StayOnScreen();
        }
    }

    // keep pointer inside the screen
    private void StayOnScreen()
    {
        // tried to go too far right
        if (pointer.localPosition.x > screenW/2)
        {
            // move pointer at maximum right coordinate
            pointer.localPosition = new Vector3(screenW / 2, pointer.localPosition.y, -50);
        }
        // too far left
        else if (pointer.localPosition.x < -screenW / 2)
        {
            pointer.localPosition = new Vector3(-screenW / 2, pointer.localPosition.y, -50);
        }
        // too far down
        if (pointer.localPosition.y > screenH/2)
        {
            pointer.localPosition = new Vector3(pointer.localPosition.x, screenH / 2, -50);
        }
        // too far up
        else if (pointer.localPosition.y < -screenH / 2)
        {
            pointer.localPosition = new Vector3(pointer.localPosition.x, -screenH / 2, -50);
        }
    }

    // gets controls ready for UI
    public void SetCutsceneMode(CutsceneScript c)
    {
        cutscene = c;
        sceneNum = LevelWinCondition.GetSceneNum();
        SwitchToUI();
    }

    // called to get refernces to objects in the main menu
    public void SetUp(int index, Button play, Button quit, Button help, GameObject startScreen, GameObject helpScreen)
    {
        sceneNum = LevelWinCondition.GetSceneNum();
        pScript = GetComponent<PlayerScript>();
        pInput = GetComponent<PlayerInput>();
        SwitchToUI();
        playerNum = index;

        isUIMode = true;

        startMenu = startScreen;
        helpMenu = helpScreen;

        // most buttons are children of above menus

        // get references to these children buttons and their transforms
        backButton = helpScreen.transform.Find("Go Back").gameObject.GetComponent<Button>();
        backT = backButton.GetComponent<RectTransform>();

        nextLayoutButton = helpScreen.transform.Find("Next Layout").gameObject.GetComponent<Button>();
        nextLayoutT = nextLayoutButton.GetComponent<RectTransform>();
        prevLayoutButton = helpScreen.transform.Find("Previous Layout").gameObject.GetComponent<Button>();
        prevLayoutT = prevLayoutButton.GetComponent<RectTransform>();

        volume = helpScreen.transform.Find("Volume").gameObject.GetComponent<Slider>();
        volumeT = volume.GetComponent<RectTransform>();

        playButton = play;
        playT = play.GetComponent<RectTransform>();
        quitButton = quit;
        quitT = quit.GetComponent<RectTransform>();
        helpButton = help;
        helpT = help.GetComponent<RectTransform>();

        // determine input type (controller vs keyboard) and adjust sensitvity
        string device = PlayerInput.GetPlayerByIndex(playerNum).currentControlScheme;
        isKAndM = device.Equals("Keyboard&Mouse");
        AdjustSensitivity();
    }

    // called to get refernces to objects in the regular levels
    public void GiveReferences(GameObject pauseM, RectTransform p) 
    {
        sceneNum = LevelWinCondition.GetSceneNum();

        // not a menu, but need access to buttons when/if player opens a menu in future
        isUIMode = false;

        pointer = p;
        pointer.gameObject.SetActive(false);

        pause = pauseM;
        winMenuObj = LevelWinCondition.winScreen;
        loseMenuObj = LevelWinCondition.loseScreen;

        // most buttons are children of above menus

        // get references to these children buttons and their transforms
        pauseQuit = pause.transform.Find("Quit").GetComponent<Button>();
        pauseMenu = pause.transform.Find("Menu").GetComponent<Button>();
        winNextLevel = winMenuObj.transform.Find("Next Level").GetComponent<Button>();
        winMenu = winMenuObj.transform.Find("Menu").GetComponent<Button>();
        loseRetry = loseMenuObj.transform.Find("Play Again").GetComponent<Button>();
        loseMenu = loseMenuObj.transform.Find("Menu").GetComponent<Button>();

        volume = pause.transform.Find("Volume").gameObject.GetComponent<Slider>();
        volumeT = volume.GetComponent<RectTransform>();

        pauseQuitT = pauseQuit.GetComponent<RectTransform>();
        pauseMenuT = pauseMenu.GetComponent<RectTransform>();
        winNextLevelT = winNextLevel.GetComponent<RectTransform>();
        winMenuT = winMenu.GetComponent<RectTransform>();
        loseRetryT = loseRetry.GetComponent<RectTransform>();
        loseMenuT = loseMenu.GetComponent<RectTransform>();

        // determine input type (controller vs keyboard) and adjust sensitvity
        string device = PlayerInput.GetPlayerByIndex(playerNum).currentControlScheme;
        isKAndM = device.Equals("Keyboard&Mouse");
        AdjustSensitivity();
    }

    private void AdjustSensitivity()
    {
        // make main menu controls a little easier
        if (sceneNum == 0)
        {
            sensitivity = sensitivityOriginal / 2;
        }
        else
        {
            sensitivity = sensitivityOriginal;
        }
    }


    // get input of cursor/analog stick movement
    void OnNavigate(InputValue input)
    {
        Vector2 temp = input.Get<Vector2>();

        // keyboard and mouse movement is already fast so update as is
        if (isKAndM)
        {
            pos = new Vector3(temp.x, temp.y, 0);
        }
        // modify analog stick movement for cursor
        else
        {
            pos = new Vector3(temp.x * sensitivity, temp.y * sensitivity, 0);
        }

    }

    /*
    void OnScroll(InputValue input)
    {
        if (scroller == null)
        {
            return;
        }

        float temp = input.Get<float>();

        if (isKAndM)
        {
            scroller.value += temp / 1000f;
        }
        else
        {
            scroller.value += temp / 100f;
        }

    }
    */

    // confirmation button
    void OnEnter()
    {
        // for cutscenes, advances to next dialogue
        if (sceneNum == 6 && cutscene != null)
        {
            cutscene.NextDialogue();
        }
        // for everything else, activates the button that was pressed
        else
        {
            OnMenuButtonSelect();
        }
    }

    void OnMenuButtonSelect()
    {
        // for main menu
        if (sceneNum == 0)
        {
            // activate the right button's onClick function
            // checks if cursor was on the button position
            if (helpButton.isActiveAndEnabled && IsOnButton(helpT))
            {
                helpButton.onClick.Invoke();
            }
            else if (quitButton.isActiveAndEnabled && IsOnButton(quitT))
            {
                quitButton.onClick.Invoke();
            }
            else if (playButton.isActiveAndEnabled && IsOnButton(playT))
            {
                playButton.onClick.Invoke();
            }
            else if (backButton.isActiveAndEnabled && IsOnButton(backT))
            {
                backButton.onClick.Invoke();
            }
            else if (nextLayoutButton.isActiveAndEnabled && IsOnButton(nextLayoutT))
            {
                nextLayoutButton.onClick.Invoke();
            }
            else if (prevLayoutButton.isActiveAndEnabled && IsOnButton(prevLayoutT))
            {
                prevLayoutButton.onClick.Invoke();
            }
            else if (volume.isActiveAndEnabled && IsOnButton(volumeT))
            {
                ChangeSliderVolume(volumeT);
            }
        }
        // for regular level's menus such as pause menu
        else
        {
            if (pauseQuit.isActiveAndEnabled && IsOnButton(pauseQuitT))
            {
                pauseQuit.onClick.Invoke();
            }
            else if (pauseMenu.isActiveAndEnabled && IsOnButton(pauseMenuT))
            {
                pauseMenu.onClick.Invoke();
            }
            else if (winNextLevel.isActiveAndEnabled && IsOnButton(winNextLevelT))
            {
                winNextLevel.onClick.Invoke();
            }
            else if (winMenu.isActiveAndEnabled && IsOnButton(winMenuT))
            {
                winMenu.onClick.Invoke();
            }
            else if (loseRetry.isActiveAndEnabled && IsOnButton(loseRetryT))
            {
                loseRetry.onClick.Invoke();
            }
            else if (loseMenu.isActiveAndEnabled && IsOnButton(loseMenuT))
            {
                loseMenu.onClick.Invoke();
            }
            else if (volume.isActiveAndEnabled && IsOnButton(volumeT))
            {
                ChangeSliderVolume(volumeT);
            }
        }
    }

    // modify volume and UI accordingly
    private void ChangeSliderVolume(RectTransform s)
    {
        float totalSize;
        float halfSize;
        float pointerValue;

        // main menu has different UI for slide so must modify differently
        if (sceneNum == 0)
        {
            totalSize = s.rect.height;
            halfSize = totalSize / 2f;
            pointerValue = pointer.localPosition.y - (s.localPosition.y - halfSize);
        }
        else
        {
            totalSize = s.rect.width;
            halfSize = totalSize / 2f;
            pointerValue = pointer.localPosition.x - (s.localPosition.x - halfSize);
        }

        // update volume and UI
        float result = pointerValue / totalSize;
        float modResult = Mathf.Round(result * 10f) / 10f; 

        volume.value = modResult;
    }

    // helper method to check if cursor is on the button that is being pressed
    private bool IsOnButton(RectTransform buttonPos)
    {
        // check button coordiates and size, and see if cursor fits inside that range
        float width = buttonPos.rect.width/2;
        float height = buttonPos.rect.height/2;
        if (Mathf.Abs(pointer.localPosition.y - buttonPos.localPosition.y) < height && Mathf.Abs(pointer.localPosition.x - buttonPos.localPosition.x) < width)
        {
            return true;
        }
        return false;
    }

    // return from a menu
    void OnBack()
    {
        // only applies to main menu
        if (sceneNum == 0)
        {
            // go from help menu (child) to start menu (parent)
            if (helpMenu.activeInHierarchy)
            {
                startMenu.SetActive(true);
                helpMenu.SetActive(false);
            }
        }
    }

    // switch from UI to gameplay controls on regular levels
    public void ResumeGame()
    {
        pInput.SwitchCurrentActionMap(controls);
        pointer.gameObject.SetActive(false);
        isUIMode = false;
        pos = Vector3.zero;

        // reset cursor locations for next time based on player 
        if (playerNum == 0)
        {
            // p1 is on the left
            pointer.localPosition = new Vector3(-300, 0, 0);
        }
        else
        {
            // p2 is on the right
            pointer.localPosition = new Vector3(300, 0, 0);
        }

        // turn off menu and resume time
        pause.SetActive(false);
        Time.timeScale = 1;
    }

    // switch from gameplay to UI controls on regular levels (pause the game)
    public void PauseGame()
    {
        pInput.SwitchCurrentActionMap("UI");
        pointer.gameObject.SetActive(true);
        isUIMode = true;
        // stop time so enemies dont attack
        Time.timeScale = 0;
        pause.SetActive(true);
    }

    // get input from unity's input system
    private void OnPause()
    {
        if (pause == null || pScript.AllowControls == false)
        {
            return;
        }

        // keep track of whether to play/pause based on if pause screen is currently turned on

        if (pause.activeInHierarchy) // play game again
        {
            InputManager.instance.SetPause(false);

        }
        else // show pause screen
        {
            InputManager.instance.SetPause(true);
        }

    }

    public void SwitchToUI()
    {
        pInput.SwitchCurrentActionMap("UI");
    }

    // for regular levels at the end menu of each level, turn to UI mode
    public void SwitchToUIForWinLose()
    {
        SwitchToUI();
        pointer.gameObject.SetActive(true);
        isUIMode = true;
    }

}
