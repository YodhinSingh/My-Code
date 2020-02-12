using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puzzle3V3 : MonoBehaviour
{
    public GameObject[] turrets = new GameObject[2];
    public GameObject[] buttons = new GameObject[3];
    public GameObject hackObj;
    public GameObject ResetAll;
    public Text[] Code = new Text[3];

    int part1Num = 3;
    int part2Num = 2;
    int part3Num = 4;

    bool RedDone;
    bool BlueDone;
    bool PurpDone;
    bool puzDone;

    int[] order = { 0, 1, 2 };

    int countRed = 0;
    int countBlue = 0;
    float countPurple = 0;

    private bool isCollidedWithObj1 = false;
    private bool isCollidedWithObj2 = false;

    private float time = 0;
    readonly float timeConsideredSame = 0.2f;

    public Text redBar;
    public Text blueBar;
    public Text purpleBar;


    bool redBarModified = false;
    bool blueBarModified = false;
    bool purBarModified = false;
    bool ChangeRedBar = true;
    bool ChangeBlueBar = true;
    bool ChangePurBar = true;

    bool allowMore = true;
    bool allowMorePur1 = true;
    bool allowMorePur2 = true;
    int pur1 = 0;
    int pur2 = 0;

    // Start is called before the first frame update
    void Start()
    {
        SetUpCode();    // create randomized code at the start of every game
    }

    // Update is called once per frame
    void Update()
    {
        if ((isCollidedWithObj1 || isCollidedWithObj2)) // are both collisions happening at the same time? check how long it happens
        {
            time += Time.deltaTime;
        }
        else
        {
            time = 0;
        }
        if (hackObj.GetComponent<InteractableBox>().hackState)  //change button colour based on hackstate of hack obj
        {
            buttons[0].SetActive(false);
            buttons[1].SetActive(false);
            buttons[2].SetActive(true);
        }
        if (!hackObj.GetComponent<InteractableBox>().hackState)
        {
            buttons[2].SetActive(false);
            buttons[1].SetActive(true);
            buttons[0].SetActive(true);
        }
        ResetPuzzle(); // check if user has shot the result button
    }

    void ResetPuzzle()
    {
        if (ResetAll.GetComponent<ResetPuz3Button>().ResetPuz)
        {
            redBarModified = false;
            blueBarModified = false;
            purBarModified = false;
            ChangeRedBar = true;
            ChangeBlueBar = true;
            ChangePurBar = true;
            allowMore = true;
            allowMorePur1 = true;
            allowMorePur2 = true;
            pur1 = 0;
            pur2 = 0;
            RedDone = false;
            BlueDone = false;
            PurpDone = false;
            puzDone = false;
            countRed = 0;
            countBlue = 0;
            countPurple = 0;
            isCollidedWithObj1 = false;
            isCollidedWithObj2 = false;
            time = 0;
            redBar.text = "";
            blueBar.text = "";
            purpleBar.text = "";
            ResetAll.GetComponent<ResetPuz3Button>().ResetPuz = false;
        }
    }

    void updateText(bool col) // UI on screen to show which colour button is being pressed at the moment, and how many times has it happened
    {
        if (allowMore)
        {
            if (col && ChangeRedBar)    // if the red colour button has been pressed, and then another colour, cant change the red colour again
            {
                redBarModified = true;
                if (countRed == 4)  // if the count goes too high, reset value
                {
                    countRed = 0;
                    redBar.text = "";
                }
                countRed++;
                redBar.text = "";
                for (int i = 0; i < countRed; i++)
                    redBar.text += "I";
                for (int i = 0; i < (4 - countRed); i++)
                    redBar.text += "-";
                if (order[0] == 0 && countRed == part1Num && !BlueDone && !PurpDone && (!blueBarModified && !purBarModified))   // placement of UI text on scene
                {
                    redBar.transform.localPosition = new Vector3(-225f, 0, 0);  // first area
                    RedDone = true;
                }
                else if (order[1] == 0 && countRed == part2Num && ((!BlueDone && PurpDone) || (BlueDone && !PurpDone)) && ((!blueBarModified && purBarModified) || (blueBarModified && !purBarModified)))
                {
                    redBar.transform.localPosition = new Vector3(0f, 0, 0); // second area
                    RedDone = true;
                }
                else if (order[2] == 0 && countRed == part3Num && BlueDone && PurpDone && (blueBarModified && purBarModified))
                {
                    redBar.transform.localPosition = new Vector3(225f, 0, 0);   //last area
                    RedDone = true;
                }
                else
                {
                    if ((!blueBarModified && !purBarModified))
                        redBar.transform.localPosition = new Vector3(-225f, 0, 0);
                    else if ((!blueBarModified && purBarModified) || (blueBarModified && !purBarModified))
                        redBar.transform.localPosition = new Vector3(0f, 0, 0);
                    else if (blueBarModified && purBarModified)
                        redBar.transform.localPosition = new Vector3(225f, 0, 0);
                    RedDone = false;
                }

            }
            else if (!col && ChangeBlueBar) // same for blue bar
            {
                blueBarModified = true;
                if (countBlue == 4)
                {
                    countBlue = 0;
                    blueBar.text = "";
                }
                countBlue++;
                blueBar.text = "";
                for (int i = 0; i < countBlue; i++)
                    blueBar.text += "I";
                for (int i = 0; i < (4 - countBlue); i++)
                    blueBar.text += "-";
                if (order[0] == 1 && countBlue == part1Num && !RedDone && !PurpDone && (!redBarModified && !purBarModified))
                {
                    blueBar.transform.localPosition = new Vector3(-225f, 0, 0);
                    BlueDone = true;
                }
                else if (order[1] == 1 && countBlue == part2Num && ((!RedDone && PurpDone) || (RedDone && !PurpDone)) && ((!redBarModified && purBarModified) || (redBarModified && !purBarModified)))
                {
                    blueBar.transform.localPosition = new Vector3(0, 0, 0);
                    BlueDone = true;
                }
                else if (order[2] == 1 && countBlue == part3Num && RedDone && PurpDone && (redBarModified && purBarModified))
                {
                    blueBar.transform.localPosition = new Vector3(225f, 0, 0);
                    BlueDone = true;
                }
                else
                {
                    if ((!redBarModified && !purBarModified))
                        blueBar.transform.localPosition = new Vector3(-225f, 0, 0);
                    else if ((!redBarModified && purBarModified) || (redBarModified && !purBarModified))
                        blueBar.transform.localPosition = new Vector3(0, 0, 0);
                    else if (redBarModified && purBarModified)
                        blueBar.transform.localPosition = new Vector3(225f, 0, 0);
                    BlueDone = false;
                }
            }
        }
    }

    public void updateUIPurple()    // purple has its own function since both red and blue need to be pressed at same time for this (but similar code)
    {
        if (allowMorePur1)
        {
            if (hackObj.GetComponent<InteractableBox>().hackState && ChangePurBar)           // purple
            {
                purBarModified = true;
                if (countPurple == 4)
                {
                    countPurple = 0;
                    purpleBar.text = "";
                }
                countPurple++;
                purpleBar.text = "";
                for (int i = 0; i < countPurple; i++)
                    purpleBar.text += "I";
                for (int i = 0; i < (4 - countPurple); i++)
                    purpleBar.text += "-";

            }

            if (order[0] == 2 && countPurple == part1Num && !BlueDone && !RedDone && (!redBarModified && !blueBarModified))
            {
                purpleBar.transform.localPosition = new Vector3(-225f, 0, 0);
                PurpDone = true;
            }
            else if (order[1] == 2 && countPurple == part2Num && ((!BlueDone && RedDone) || (BlueDone && !RedDone)) && ((!redBarModified && blueBarModified) || (redBarModified && !blueBarModified)))
            {
                purpleBar.transform.localPosition = new Vector3(0, 0, 0);
                PurpDone = true;
            }
            else if (order[2] == 2 && countPurple == part3Num && BlueDone && RedDone && (redBarModified && blueBarModified))
            {
                purpleBar.transform.localPosition = new Vector3(225f, 0, 0);
                PurpDone = true;
            }
            else
            {
                if ((!redBarModified && !blueBarModified))
                    purpleBar.transform.localPosition = new Vector3(-225f, 0, 0);
                else if ((!redBarModified && blueBarModified) || (redBarModified && !blueBarModified))
                    purpleBar.transform.localPosition = new Vector3(0, 0, 0);
                else if (redBarModified && blueBarModified)
                    purpleBar.transform.localPosition = new Vector3(225f, 0, 0);
                PurpDone = false;
            }
        }
    }


    private void OnCollisionEnter(Collision col)
    {
        if (hackObj.GetComponent<InteractableBox>().hackState && ChangePurBar)  // purple
        {
            isCollidedWithObj1 = true;
            isCollidedWithObj2 = true;
            updateUIPurple();

            if (blueBarModified)
                ChangeBlueBar = false;
            if (redBarModified)
                ChangeRedBar = false;
            allowMorePur1 = false;

        }
        else if (!hackObj.GetComponent<InteractableBox>().hackState)
        {
            if (col.gameObject.CompareTag("turretBullet1") && !isCollidedWithObj2)         //red
            {
                isCollidedWithObj1 = true;
                if (blueBarModified)
                    ChangeBlueBar = false;
                if (purBarModified)
                    ChangePurBar = false;
                updateText(col.gameObject.GetComponent<TurretBullet>().colour1);
                allowMore = false;
            }
            if (col.gameObject.CompareTag("turretBullet2") && !isCollidedWithObj1)         //blue
            {
                isCollidedWithObj2 = true;
                if (redBarModified)
                    ChangeRedBar = false;
                if (purBarModified)
                    ChangePurBar = false;
                updateText(col.gameObject.GetComponent<TurretBullet>().colour1);
                allowMore = false;
            }
        }



    }

    private void OnCollisionExit(Collision col)
    {
            if (col.gameObject.CompareTag("turretBullet1"))
            {
                isCollidedWithObj1 = false;

            }
            if (col.gameObject.CompareTag("turretBullet2"))
            {
                isCollidedWithObj2 = false;
            }
            allowMore = true;
        allowMorePur1 = true;

        PuzComplete();
    }
    void SetUpCode()    // create random code that player will use in puzzle (how many times each button colour has to be pressed)
    {
        part1Num = Random.Range(1, 5);
        part2Num = Random.Range(1, 5);
        part3Num = Random.Range(1, 5);

        for (int i = 0; i < order.Length; i++)
        {
            int temp = order[i];
            int randomIndex = Random.Range(i, order.Length);
            order[i] = order[randomIndex];
            order[randomIndex] = temp;
        }

        for (int i = 0; i < order.Length; i++)
        {
            if (order[i] == 0)
            {
                Code[i].color = Color.red;
            }
            if (order[i] == 1)
            {
                Code[i].color = Color.blue;
            }
            if (order[i] == 2)
            {
                Code[i].color = new Color(138, 0, 164, 255);
            }
        }
        Code[0].text = part1Num.ToString();
        Code[1].text = part2Num.ToString();
        Code[2].text = part3Num.ToString();


    }

    void PuzComplete()
    {
        if (RedDone && BlueDone && PurpDone && !puzDone)    // check if all 3 buttons pressed in right order and amount, then destroy barrier so player can destroy circuit
        {
            GameObject.Find("barrier3").SetActive(false);
            turrets[0].GetComponent<TurretShooter>().keepShooting = false;
            turrets[1].GetComponent<TurretShooter>().keepShooting = false;
            ResetAll.SetActive(false);
            puzDone = true;
        }
    }
}
