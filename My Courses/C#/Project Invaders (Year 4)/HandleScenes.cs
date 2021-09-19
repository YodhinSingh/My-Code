using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandleScenes : MonoBehaviour
{
    // handler class that holds all relevent info for a level

    public GameObject winScreen;
    public GameObject loseScreen;

    public Transform playerOneAimPoint = null;
    public Transform playerTwoAimPoint = null;
    public Transform playerOneSpawnPoint = null;
    public Transform playerTwoSpawnPoint = null;
    public EnergyBar energybar = null;
    public Image P1Stamina;
    public Image P2Stamina;
    public TextMeshProUGUI P1Health;
    public TextMeshProUGUI P2Health;
    public Image P1Healthbar;
    public Image P2Healthbar;
    public TextMeshProUGUI P1Multiplier;
    public TextMeshProUGUI P2Multiplier;
    public GameObject pauseScreen;
    public Image P1Cover;
    public Image P2Cover;
    public RectTransform p1Pointer;
    public RectTransform p2Pointer;
    public Image p1ElementSymbol;
    public Image p2ElementSymbol;
    public Sprite[] elementSprites;
    public Slider volume;
    public Image volumeSymbol;
    public Sprite[] volSprites;

    public Transform boss;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        winScreen.SetActive(false);
        loseScreen.SetActive(false);
        GiveWinLoseScreens();
        LevelWinCondition.Reset();

        InputManager m = InputManager.instance;
        if (m != null)
        {
            // give reference to manager
            GameObject[] p = m.gameObject.GetComponent<InputManager>().GetReferenceToPlayers();
            GiveReferencesToPlayers(p[0], p[1]);
            m.energy = energybar;

            m.gameObject.GetComponent<AudioLoopScript>().SetupMusic();
            volume.value = m.GetComponent<AudioLoopScript>().GetVolume();
            ChangeSymbol();

        }

    }

    public void NextLevel()
    {
        LevelWinCondition.LoadScene(6);
    }

    public void ReloadScene()
    {
        int lvl = InputManager.instance.GetSceneNum();
        LevelWinCondition.LoadScene(lvl);
    }

    public void LoadScene(int scene)
    {
        LevelWinCondition.LoadScene(scene);
    }

    public void QuitGame()
    {
        LevelWinCondition.LoadScene(-1);
    }

    public void ReturnToMenu()
    {
        LevelWinCondition.LoadScene(0);
    }

    public void Resume()
    {
        InputManager.instance.SetPause(false);
    }

    public void GiveWinLoseScreens()
    {
        LevelWinCondition.winScreen = winScreen;
        LevelWinCondition.loseScreen = loseScreen;
    }


    public void GiveReferencesToPlayers(GameObject playerOne, GameObject playerTwo)
    {
        if (playerOne == null || playerTwo == null)
        {
            return;
        }

        bool islvlThree = LevelWinCondition.GetSceneNum() == 3;


        // give all needed references of this level to each player
        playerOne.GetComponent<PlayerScript>().SetUp(1, playerOneSpawnPoint.position, 0, P1Stamina, P1Cover.gameObject, P1Multiplier);
        playerOneAimPoint.gameObject.SetActive(true);
        playerOne.GetComponent<PlayerHealth>().SetUp(1, energybar, P1Health, P1Healthbar);
        playerOne.GetComponent<ElementShoot>().SetUp(playerOneAimPoint.position, energybar, p1ElementSymbol, elementSprites);
        playerOne.GetComponent<PlayerUI>().GiveReferences(pauseScreen, p1Pointer);
        playerOne.GetComponent<FindObjective>().SetUpReferences(playerTwo.transform, boss, 1, islvlThree);

        playerTwo.GetComponent<PlayerScript>().SetUp(2, playerTwoSpawnPoint.position, 0.5f, P2Stamina, P2Cover.gameObject, P2Multiplier);
        playerTwoAimPoint.gameObject.SetActive(true);
        playerTwo.GetComponent<PlayerHealth>().SetUp(2, energybar, P2Health, P2Healthbar);
        playerTwo.GetComponent<ElementShoot>().SetUp(playerTwoAimPoint.position, energybar, p2ElementSymbol, elementSprites);
        playerTwo.GetComponent<PlayerUI>().GiveReferences(pauseScreen, p2Pointer);
        playerTwo.GetComponent<FindObjective>().SetUpReferences(playerOne.transform, boss, 2, islvlThree);

        playerOne.GetComponent<PlayerScript>().AllowControls = true;
        playerTwo.GetComponent<PlayerScript>().AllowControls = true;

    }


    public void ChangeVolume()
    {
        InputManager.instance.GetComponent<AudioLoopScript>().ChangeVolume(volume.value);
        ChangeSymbol();
    }

    private void ChangeSymbol()
    {
        if (volume.value <= 0)
        {
            volumeSymbol.sprite = volSprites[0];
        }
        else
        {
            volumeSymbol.sprite = volSprites[1];
        }
    }
}
