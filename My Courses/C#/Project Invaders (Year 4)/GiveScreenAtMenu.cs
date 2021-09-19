using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiveScreenAtMenu : MonoBehaviour
{
    // helper class to hold UI objects that can be sent to other classes

    public GameObject startScreen;
    public GameObject helpScreen;

    public RectTransform p1Pointer;
    public RectTransform p2Pointer;

    public Slider volume;
    public Image volumeSymbol;
    public Sprite[] volSprites;

    // Start is called before the first frame update
    void Start()
    {
        // send to manager class
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        InputManager.instance.GiveReferenceToStart(startScreen, helpScreen, p1Pointer, p2Pointer);


        InputManager.instance.GetComponent<AudioLoopScript>().SetupMusic();
        volume.value = InputManager.instance.GetComponent<AudioLoopScript>().GetVolume();
        ChangeSymbol();
    }

    public void StartGame()
    {
        InputManager.instance.StartGame();
    }

    public void ChangeVolume()
    {
        InputManager.instance.GetComponent<AudioLoopScript>().ChangeVolume(volume.value);
        ChangeSymbol();
    }

    private void ChangeSymbol()
    {
        // change volume symbol for whether its mute or on
        if (volume.value <= 0)
        {
            volumeSymbol.sprite = volSprites[0];
        }
        else
        {
            volumeSymbol.sprite = volSprites[1];
        }
    }

    public void QuitGame()
    {
        LevelWinCondition.LoadScene(-1);
    }
}
