using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle1Button : MonoBehaviour
{
    // player hits this button with bullet to change platfrom height in puzzle 1
    public int buttonNum = 1;

    public void changePlatforms()
    {
        GetComponentInParent<Puzzle1>().useMove(buttonNum);
    }
}
