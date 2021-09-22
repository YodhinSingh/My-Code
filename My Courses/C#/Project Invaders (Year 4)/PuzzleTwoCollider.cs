using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTwoCollider : MonoBehaviour
{
    // reference to puzzle
    public PuzzleTwo puzzle;

    private void OnTriggerEnter(Collider other)
    {
        // when triggered, puzzle is complete
        if ((other.gameObject.tag == "PlayerOne" || other.gameObject.tag == "PlayerTwo"))
        {
            puzzle.PuzzleComplete();
        }
    }
}
