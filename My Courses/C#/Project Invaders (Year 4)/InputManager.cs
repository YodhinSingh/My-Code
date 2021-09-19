using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    // manager for players

    private GameObject[] players = new GameObject[2];

    PlayerInputManager manager;
    private int playerNum = 1;

    public static InputManager instance;

    GameObject startScreen, helpScreen;

    [HideInInspector] public EnergyBar energy;
    private int sceneNum = 0;

    public int StartGameAtLevel; // change this in inspector for which level to start game on

    private int[] TotalScores = { 0, 0 };

    private RectTransform[] pointers = new RectTransform[2];

    private Button play, help, quit;

    private Transform boss;

    private float[] damageMultiplier = new float[2];

    private bool lvl4Done = false;
    
    // Start is called before the first frame update
    void Awake()
    {

        if (instance == null)    // Only keep this instance of this object alive. Dont destroy it after changing scenes and destroy any duplicates
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);


        manager = GetComponent<PlayerInputManager>();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // safety measure to not start at non-existant levels
        if (StartGameAtLevel > 6 || StartGameAtLevel < 1)
        {
            StartGameAtLevel = 6;
        }

        lvl4Done = false;
    }

    // modify player damage multipliers
    public void SetMultiplier(int pIndex, float val)
    {
        damageMultiplier[pIndex] += val;
        damageMultiplier[pIndex] = (float) Math.Round(damageMultiplier[pIndex], 2, MidpointRounding.AwayFromZero);
    }

    // get player damage multipliers
    public float GetMultiplier(int index)
    {
        return damageMultiplier[index];
    }

    // create player
    void OnPlayerJoined(PlayerInput player)
    {
        if (playerNum == 1)
        {
            // give references to other classes for this player
            LevelWinCondition.p1 = player.gameObject;

            // modify UI to show this player joined
            players[0] = player.gameObject;
            startScreen.transform.Find("Start").gameObject.SetActive(false);
            startScreen.transform.Find("p1joined").GetComponent<TextMeshProUGUI>().text = "BLAZE JOINED!";
            pointers[0].gameObject.SetActive(true);
            help.gameObject.SetActive(true);
            quit.gameObject.SetActive(true);
            // give player references
            player.GetComponent<PlayerUI>().pointer = pointers[0];
            player.GetComponent<PlayerUI>().SetUp(0, play, quit, help, startScreen, helpScreen);
        }
        // do same for player 2
        else
        {
            LevelWinCondition.p2 = player.gameObject;

            players[1] = player.gameObject;
            startScreen.transform.Find("p2joined").GetComponent<TextMeshProUGUI>().text = "SHIVER JOINED!";
            pointers[1].gameObject.SetActive(true);
            play.gameObject.SetActive(true);
            player.GetComponent<PlayerUI>().pointer = pointers[1];
            player.GetComponent<PlayerUI>().SetUp(1, play, quit, help, startScreen, helpScreen);

            lvl4Done = false;
            // only 2 players can join
            manager.DisableJoining();

        }
        // keep track of player count
        playerNum++;
    }

    public void StartGame()
    {
        LevelWinCondition.LoadScene(StartGameAtLevel);
    }

    public GameObject[] GetReferenceToPlayers()
    {
        sceneNum = LevelWinCondition.GetSceneNum();
        return players;
    }

    public GameObject[] GetPlayersCutscene()
    {
        return players;
    }

    public void SetBossLocation(Transform b)
    {
        boss = b;
    }

    // helper method to call to see how far each player is to boss
    public bool IsBossNearby()
    {
        if (players[0] == null || players[1] == null || boss == null)
        {
            return false;
        }
        if ((sceneNum == 4 && lvl4Done) || (sceneNum != 4))
        {
            bool condOne = Vector3.Distance(players[0].transform.position, boss.position) <= 50;
            bool condTwo = Vector3.Distance(players[1].transform.position, boss.position) <= 50;

            if (condOne || condTwo)
            {
                return true;
            }
        }

        return false;
    }

    public void Lvl4ObjectiveComplete()
    {
        lvl4Done = true;
    }

    public void ResetGame()
    {
        playerNum = 1;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null)
            {
                Destroy(players[i]);
            }
        }
        players[0] = null;
        players[1] = null;

        DamagePopUpUi.ClearList();

    }

    // get references to UI at menu screen
    public void GiveReferenceToStart(GameObject startScreen, GameObject helpScreen, RectTransform p1Point, RectTransform p2Point)
    {
        sceneNum = 0;
        this.startScreen = startScreen;
        this.helpScreen = helpScreen;
        pointers[0] = p1Point;
        pointers[1] = p2Point;

        play = startScreen.transform.Find("Play").gameObject.GetComponent<Button>();
        help = startScreen.transform.Find("How to play").gameObject.GetComponent<Button>();
        quit = startScreen.transform.Find("Quit").gameObject.GetComponent<Button>();

        manager.EnableJoining();
    }

    // give energy to UI bar
    public void AddEnergy(int v)
    {
        int value = v * sceneNum;
        energy.AddEnergy(value);
    }

    public int GetSceneNum()
    {
        return sceneNum;
    }

    public int[] getTotalScores()
    {
        return TotalScores;
    }

    public void setTotalScore(int index, int value)
    {
        TotalScores[index] += value;
    }

    // pause/unpause game and controls
    public void SetPause(bool isTrue)
    {
        if (isTrue)
        {
            LevelWinCondition.SetGamePaused(true);
            players[0].GetComponent<PlayerUI>().PauseGame();
            players[1].GetComponent<PlayerUI>().PauseGame();
        }
        else
        {
            LevelWinCondition.SetGamePaused(false);
            players[0].GetComponent<PlayerUI>().ResumeGame();
            players[1].GetComponent<PlayerUI>().ResumeGame();
        }
    }

    // switch player controls to UI
    public void SetPlayerUI()
    {
        players[0].GetComponent<PlayerUI>().SwitchToUIForWinLose();
        players[1].GetComponent<PlayerUI>().SwitchToUIForWinLose();
    }
    // allows/denies 1 player to access both elements
    public void AllowSwitchElement(bool allow, int playerThatCalledThis)
    {
        players[0].GetComponent<ElementShoot>().CanSwitchElement(allow, playerThatCalledThis);
        players[1].GetComponent<ElementShoot>().CanSwitchElement(allow, playerThatCalledThis);
    }
}
