using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleOneButton : MonoBehaviour
{
    // visual properties
    public Color32 colour; // Color32(143, 0, 254, 255); // purple colour code
    public Material pressMat;
    public Material defaultMat;
    public Material turnOffColour;

    // references to puzzle
    private PuzzleOne puzzle;
    private int index;

    // press properties
    private bool justPressed = false;
    private bool isPressed = false;
    private bool isDisabled = false;

    // the unique ID of each button in the puzzle
    public int GetIndex()
    {
        return index;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        // if the puzzle is over, make sure it remains turned off
        if (puzzle == null || isDisabled)
        {
            DisableButton();
            return;
        }

        // make sure players only can interact, and not too fast in succession
        if (!justPressed && (other.gameObject.tag == "PlayerOne" || other.gameObject.tag == "PlayerTwo"))
        {
            // make colour brighter to show its been pressed and update puzzle
            ChangeButtonColour(2);
            isPressed = justPressed = true;
            puzzle.PressedButton(index);
            StartCoroutine(Cooldown());
        }

    }

    // cooldown to ensure the puzzle doesnt recieve too many on collision updates
    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(0.5f);
        // change colour and variables back to normal
        justPressed = false;
        ChangeButtonColour(1);
        isPressed = false;
    }

    // gives a reference of itself to the puzzle
    public void GivePuzzleReference(PuzzleOne p, int i)
    {
        puzzle = p;
        index = i;
    }

    // turns off the button and its colour
    public void DisableButton()
    {
        isDisabled = true;

        ChangeButtonColour(0);
    }

    // changes the buttons color to normal/pressed/off
    private void ChangeButtonColour(int version)
    {
        // get materials
        MeshRenderer m = GetComponent<MeshRenderer>();
        Material[] mats = GetComponent<MeshRenderer>().materials;

        if (version == 0 || isDisabled) // turn off / disable
        {
            mats[1] = turnOffColour;
        }
        else if (version == 1) // regular
        {
            mats[1] = defaultMat;
        }
        else // pressed
        {
            mats[1] = pressMat;
        }

        // apply materials
        m.sharedMaterials = mats;
    }

}
