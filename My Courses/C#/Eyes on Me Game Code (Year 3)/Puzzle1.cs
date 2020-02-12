using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle1 : MonoBehaviour
{
    // Puzzle 1

    public GameObject[] levels  = new GameObject[4];
    bool puzSolved = false;

    float platform1;
    float platform2;
    float platform3;
    float platform4;

    const int MAX = 12;
    // Start is called before the first frame update
    void Start()
    {
        platform1 = 1;
        platform2 = 1;
        platform3 = 1;
        platform4 = 1;
    }

    // when bullet shoots this it modifies platform height based on the following values
    public void useMove(int value)
    {
        switch (value)  // each value is a platform (e.g. if hit platform 1: platform 1 and 2 increase by 1, platform 4 decreases by 1). These values not told to player, have to figure out.
        {
            case (1):
                platform1 = modify(platform1, true);    // true increases platform height by 1, false decreases it by 1
                platform2 = modify(platform2, true);
                platform4 = modify(platform4, false);
                break;
            case (2):
                platform2 = modify(platform2, false);
                platform3 = modify(platform3, true);
                break;
            case (3):
                platform1 = modify(platform1, true);
                platform4 = modify(platform4, true);
                break;
            case (4):
                platform1 = modify(platform1, false);
                platform2 = modify(platform2, true);
                platform4 = modify(platform4, true);
                break;
        }
        

        changeHeight();
    }

    private float modify(float ver, bool direction) // makes sure modification is in range
    {
        if (ver >= 2 && direction == true)
            return 3;
        else if (ver <= 1 && direction == false)
            return 0.1f;
        else if (direction == true)
            return (int) ver + 1;
        return (int) ver - 1;
    }

    private void changeHeight() // change platforms' scale
    {
        if (!puzSolved)
        {
            levels[0].transform.localScale = new Vector3(levels[0].transform.localScale.x, platform1, levels[0].transform.localScale.z);
            levels[1].transform.localScale = new Vector3(levels[1].transform.localScale.x, platform2, levels[1].transform.localScale.z);
            levels[2].transform.localScale = new Vector3(levels[2].transform.localScale.x, platform3, levels[2].transform.localScale.z);
            levels[3].transform.localScale = new Vector3(levels[3].transform.localScale.x, platform4, levels[3].transform.localScale.z);
        }

        if ((platform1 + platform2 + platform3 + platform4 >= MAX) && !puzSolved)   // if all platforms have a height of 3 then the player unlocks the circuit here
        {
            puzSolved = true;
            GameObject barrier = GameObject.Find("barrier1");
            barrier.SetActive(false);

        }
    }


}
