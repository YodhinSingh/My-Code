using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleThree : MonoBehaviour
{
    // references
    public PuzzleThreeSwitch[] switches;
    public TextMeshProUGUI[] patternObj;
    public EnemySpawner[] spawner;
    public GameObject[] walls;
    public GameObject key;
    private GameObject[] players;

    // properties
    private int curIndex;
    private bool waitingToReset = false;
    private bool puzzleIsSolved = false;


    // Start is called before the first frame update
    void Start()
    {
        
        if (InputManager.instance == null)
        {
            return;
        }

        // get references to each player 
        players = InputManager.instance.GetReferenceToPlayers();
        players[0].GetComponent<PlayerCombat>().puz3 = this;
        players[1].GetComponent<PlayerCombat>().puz3 = this;
        
        // generate a code
        curIndex = 0;
        GeneratePattern();
    }

    // checks which switch was activated and determines if it was the right one for the player
    public void ActivateSwitch(int id, int playerIndex)
    {
        // ignore when puzzle is done or in pause mode
        if (waitingToReset || puzzleIsSolved)
        {
            return;
        }

        int temp = id % 2; // player 1 should press even numbers, player 2 should press odd (starting index 0)

        // check if the player activated the right switch
        bool didPressRightButton = playerIndex == temp; // player one = 0, player two = 1

        if (!didPressRightButton) // if pressed wrong switch 
        {
            // spawn enemies and pause the puzzle. Also enable a door to block player from leaving
            waitingToReset = true;
            spawner[id - 30].SpawnEnemies(this); // '-30' is to offset ID (e.g. 31 => puzzle 3 switch 2)
            walls[id - 30].SetActive(true);      // since spawners deal with all puzzles, IDs for puzzle 3 are given '+30' to make them unique 
            return;
        }

        // if pressed right switch, then update puzzle
        switches[id - 30].PuzzleStatus(true);
        // play animation for switch working and disable it from the screen
        switches[id - 30].StartSwitch();
        patternObj[id - 30].text = "";
        // update counter
        curIndex++;

        // once counter reaches code length, the puzzle is solved
        if (curIndex == patternObj.Length)
        {
            key.SetActive(true);
        }
    }

    // called externally by spawner that was called by ActivateSwitch method. Opens blocked door and continues puzzle
    public void CanContinuePuzzle()
    {
        waitingToReset = false;
        for (int i = 0; i < walls.Length; i++)
        {
            walls[i].SetActive(false);
        }
    }

    // called when puzzle is complete. Turns puzzle and components off.
    public void KeyTaken()
    {
        puzzleIsSolved = true;

        // screen code
        foreach (TextMeshProUGUI i in patternObj)
        {
            i.gameObject.SetActive(false);
        }

        // all blocked doors
        for (int i = 0; i < walls.Length; i++)
        {
            walls[i].SetActive(false);
        }

        // all switches are safety checked to be completed
        for (int i = 0; i < switches.Length; i++)
        {
            switches[i].PuzzleStatus(true);
        }
    }


    // procedurally generates the code for the order of switches
    private void GeneratePattern()
    {

        for (int i = 0; i < switches.Length; i++) // give switches id
        {
            switches[i].RecieveId(i);
        }

        for (int i = 0; i < patternObj.Length; i++) 
        {
            patternObj[i].text = (switches[i].GetId() - 29).ToString(); // write order of switches on ui screen (offsets ID)
            patternObj[i].color = switches[i].colour;
        }
    }
}
