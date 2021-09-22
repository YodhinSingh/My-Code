using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PuzzleTwo : MonoBehaviour
{
    // references
    public GameObject Door;
    public EnemySpawner spawner;
    public TextMeshProUGUI timer;
    public GameObject checkCompleteCollider;
    public Transform switchKnob;
    public TextMeshProUGUI keyDoorInfo;

    // puzzle properties
    private int timeForPuzzle = 25;
    private int curTime;
    private bool puzzleIsRunning = false;
    private bool puzzleIsSolved = false;
    private bool keyIsTaken = false;

    // switch properties
    private float targetKnobRotation;
    private float targetKnobInEuler;

    private GameObject[] players;

    private void Start()
    {
        if (InputManager.instance == null)
        {
            return;
        }

        // get reference to each player
        players = InputManager.instance.GetReferenceToPlayers();
        players[0].GetComponent<PlayerCombat>().puz2 = this;
        players[1].GetComponent<PlayerCombat>().puz2 = this;

        // set switch knob to starting point
        targetKnobRotation = switchKnob.localRotation.x *-1;
        targetKnobInEuler = switchKnob.localEulerAngles.x * -1;

        // reset variables
        curTime = timeForPuzzle;
        ShowTimer();
    }

    // called when player starts this puzzle. Updates a timer.
    private void UpdateTimer()
    {
        // ignore if puzzle done
        if (keyIsTaken)
        {
            return;
        }

        // update visuals on UI
        ShowTimer();

        // countdown
        if (curTime > 0)
        {
            curTime--;
        }
    }

    // make variables back to starting value
    private void ResetTimer()
    {
        curTime = timeForPuzzle;
        ShowTimer();
    }

    // show timer in proper format
    private void ShowTimer()
    {
        // makes it so it appears as 0:08 vs 0:8
        if (curTime > 9)
        {
            // update UI text
            timer.text = "0:" + curTime;
        }
        else
        {
            timer.text = "0:0" + curTime;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        // ignore if puzzle done
        if (puzzleIsSolved)
        {
            return;
        }

        // used to start puzzle counter, ignore if already running
        if (!puzzleIsRunning && (other.gameObject.tag == "PlayerOne" || other.gameObject.tag == "PlayerTwo"))
        {
            // update player scripts for which switch they are near and can interact with
            other.GetComponent<PlayerCombat>().NearSwitchNum = 2;
            other.GetComponent<PlayerCombat>().isNearSwitch = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // update player scripts that they are too far to interact
        if ((other.gameObject.tag == "PlayerOne" || other.gameObject.tag == "PlayerTwo"))
        {
            other.GetComponent<PlayerCombat>().NearSwitchNum = 0;
            other.GetComponent<PlayerCombat>().isNearSwitch = false;
        }
    }

    // updates screen UI text based on puzzle status
    private void ChangeTvText(bool isPuzzleOn)
    {
        // while puzzle/timer is running, the door is temporarily unlocked
        if (isPuzzleOn)
        {
            keyDoorInfo.text = "KEY     DOOR    UNLOCKED";
            keyDoorInfo.color = Color.white;
        }
        else
        {
            keyDoorInfo.text = "KEY     DOOR    LOCKED";
            keyDoorInfo.color = Color.red;
        }
    }


    // called externally by player scripts, turns on the puzzle
    public void TurnOnPuzzle()
    {
        // only if its not done or already in progress
        if (!puzzleIsRunning && !puzzleIsSolved)
        {
            ChangeTvText(true);
            StartCoroutine(RotateKnob());
            puzzleIsRunning = true;
            Door.SetActive(false);
            InvokeRepeating("UpdateTimer", 0f, 1f);
            StartCoroutine(TimeTillPuzzleResets(timeForPuzzle));
        }
    }

    // called when puzzle timer is up
    private IEnumerator TimeTillPuzzleResets(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        // players too late, reset puzzle values
        ResetPuzzle();
    }

    // animation to move the knob
    private IEnumerator RotateKnob()
    {
        // makes knob animated backwards and forwards based on initial starting point
        int val;
        if (targetKnobRotation > 0)
        {
            val = 100;
        }
        else
        {
            val = -100;
        }

        // handles animating both ways
        while (Mathf.Abs(switchKnob.localRotation.x - targetKnobRotation) >= 0.1f)
        {
            switchKnob.Rotate(val * Time.deltaTime, 0, 0);
            yield return null;
        }

        // simplifies end numbers to exact values
        switchKnob.localRotation = Quaternion.Euler(targetKnobInEuler, 0,0);
        targetKnobRotation *= -1;
        targetKnobInEuler *= -1;
    }

    // called when puzzle is done and all objects must be disabled
    public void PuzzleComplete()
    {
        CancelInvoke();
        puzzleIsSolved = true;
        // turn off all models and UI
        Door.SetActive(false);
        checkCompleteCollider.SetActive(false);
        timer.gameObject.SetActive(false);
        ChangeTvText(true);
        keyDoorInfo.gameObject.SetActive(false);
    }

    // make variables back to starting value after failing puzzle
    public void ResetPuzzle()
    {
        // if player did not get the key then reset timer and enable trap door only
        if (!keyIsTaken && !puzzleIsSolved)
        {
            CancelInvoke();
            ResetTimer();
            StartCoroutine(RotateKnob());
            puzzleIsRunning = false;
            Door.SetActive(true);
            ChangeTvText(false);
            checkCompleteCollider.SetActive(false);
        }
        // if player got the key but could not get out the door in time, enter 'partial completion' state
        // enable trap door and spawn enemies to beat (once enemies are beaten, puzzle is done)
        if (keyIsTaken && !puzzleIsSolved)
        {
            puzzleIsRunning = true;
            Door.SetActive(true);
            ChangeTvText(false);
            spawner.SpawnEnemies(this);
            checkCompleteCollider.SetActive(false);
        }
    }

    // called when player takes key
    public void KeyTaken()
    {
        keyIsTaken = true;
        // if trap door active, player did not escape room in time, so summon more enemies
        if (Door.activeInHierarchy)
        {
            spawner.SpawnEnemies(this);
            return;
        }
        checkCompleteCollider.SetActive(true);
    }
}
