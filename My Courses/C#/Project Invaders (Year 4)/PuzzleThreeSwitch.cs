using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleThreeSwitch : MonoBehaviour
{
    // model properties
    public Transform switchKnob;
    public Color32 colour;

    // puzzle properties
    private bool puzzleIsSolved;
    private int switchNum;

    // animation properties
    private float targetKnobRotation;
    private float targetKnobInEuler;
    private float[] initialValuesForRotation = new float[2];

    private void Start()
    {
        // set initial location of swtich knob
        targetKnobRotation = switchKnob.localRotation.x * -1;
        targetKnobInEuler = switchKnob.localEulerAngles.x * -1;
        initialValuesForRotation[0] = switchKnob.localRotation.x;
        initialValuesForRotation[1] = switchKnob.localEulerAngles.x;

    }

    private void OnTriggerStay(Collider other)
    {
        // ignore if puzzle done
        if (puzzleIsSolved)
        {
            return;
        }

        // let player scripts know if they are near this and can interact with it
        if ( (other.gameObject.tag == "PlayerOne" || other.gameObject.tag == "PlayerTwo"))
        {
            other.GetComponent<PlayerCombat>().NearSwitchNum = switchNum;
            other.GetComponent<PlayerCombat>().isNearSwitch = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // let player scripts know they are not near it anymore
        if ((other.gameObject.tag == "PlayerOne" || other.gameObject.tag == "PlayerTwo"))
        {
            other.GetComponent<PlayerCombat>().NearSwitchNum = switchNum;
            other.GetComponent<PlayerCombat>().isNearSwitch = false;
        }
    }

    // sets if the puzzle is done or not
    public void PuzzleStatus(bool IsPuzzleSolved)
    {
        puzzleIsSolved = IsPuzzleSolved;
    }

    // the main puzzle script handles giving IDs
    public void RecieveId(int id)
    {
        // offset the ID by 30 (30 due to puzzle 3) since spawner classes deal 
        // with all 3 puzzles at once so this is a unique ID for this switch
        switchNum = 30 + id;
    }

    public int GetId()
    {
        return switchNum;
    }

    // play 'animation' where switch knob moves from one side to the other
    public void StartSwitch()
    {
        StartCoroutine(RotateKnob());
    }

    // makes switch back to starting point
    public void ResetSwitch()
    {
        StopAllCoroutines();

        targetKnobRotation = initialValuesForRotation[0] * -1;
        targetKnobInEuler = initialValuesForRotation[1] * - 1;
        switchKnob.localRotation = Quaternion.Euler(initialValuesForRotation[1], 0, 0);
    }

    // animation to move the knob
    private IEnumerator RotateKnob()
    {
        while (Mathf.Abs(switchKnob.localRotation.x - targetKnobRotation) >= 0.1f)
        {
            // rotate it smoothly like an animation
            switchKnob.Rotate(100 * Time.deltaTime, 0, 0);
            yield return null;
        }
    }

}
