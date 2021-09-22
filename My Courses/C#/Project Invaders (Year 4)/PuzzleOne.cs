using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleOne : MonoBehaviour
{
    // puzzle pieces
    public PuzzleOneButton[] buttons;
    public Image[] patternObj;
    public EnemySpawner spawner;
    public GameObject key;

    // puzzle code
    private int[] orderPressed;
    private int curIndex;

    // completion check
    private bool waitingToReset = false;
    private bool puzzleDone = false;

    // Start is called before the first frame update
    void Start()
    {
        // reset variables and create a procedurally generated order to the puzzle
        curIndex = 0;
        GeneratePattern();
    }

    // checks which button was pressed and determines if it was the right one in the sequence
    public void PressedButton(int index)
    {
        // ignore any presses if the puzzle is done / being reset
        if (curIndex >= orderPressed.Length || waitingToReset || puzzleDone)
        {
            return;
        }

        // checks if this button is the same one as the one in the sequence
        // then update the counter of the sequence to the next one
        bool didPressRightButton = orderPressed[curIndex++] == index;

        if (!didPressRightButton) // if pressed wrong button in order
        {
            // in 'reset' mode, spawn enemies here and wait until player defeats them before restarting
            waitingToReset = true;
            spawner.SpawnEnemies(this);
        }

        // once the counter reaches the end, all buttons have been successfully pressed
        if (curIndex == orderPressed.Length)
        {
            // turn off the puzzle and reveal the key
            PuzzleComplete();
            key.SetActive(true);
        }
    }

    // turns off all the buttons and images on the screen
    public void PuzzleComplete()
    {
        puzzleDone = true;
        // all images on screen
        foreach (Image i in patternObj)
        {
            i.gameObject.SetActive(false);
        }
        // all button lights are turned off
        foreach(PuzzleOneButton b in buttons)
        {
            b.DisableButton();
        }
    }

    // called externally by the spawner script that was called in PressedButton function
    // resets all variables and generates a new order
    public void ResetPuzzle()
    {
        curIndex = 0;
        GeneratePattern();
        waitingToReset = false;
        puzzleDone = false;
    }

    // procedurally generates the order of the puzzle code to be displayed on screen
    private void GeneratePattern()
    {
        PuzzleOneButton tempButton;

        for (int i = 0; i < buttons.Length; i++)    // randomize button order
        {
            int rand = Random.Range(0, buttons.Length);

            tempButton = buttons[rand];
            buttons[rand] = buttons[i];
            buttons[i] = tempButton;
        }

        int num = patternObj.Length;

        for (int i = 0; i < patternObj.Length; i+=2) // randomize how many presses per colour and update view
        {
            patternObj[i].color = buttons[i/2].colour;
            patternObj[i+1].color = buttons[i/2].colour;

            int rand = Random.Range(1, 3);
            if (rand == 1) // one press only
            {
                // turn off the second image for that column
                patternObj[i + 1].gameObject.SetActive(false);
                num--;
            }
            else // 2 presses
            {
                // both images for that column will be on
                patternObj[i + 1].gameObject.SetActive(true);
            }

        }

        orderPressed = new int[num];

        for (int i = 0; i < buttons.Length; i++) // give buttons their id based on the order chosen above
        {
            buttons[i].GivePuzzleReference(this, i);
        }

        int temp = 0;
        for (int i = 0; i < patternObj.Length; i++) // get the numerical order of button ids to press
        {
            if (patternObj[i].gameObject.activeInHierarchy)
            {
                orderPressed[temp++] = buttons[i / 2].GetIndex();
            }

        }
    }
}
