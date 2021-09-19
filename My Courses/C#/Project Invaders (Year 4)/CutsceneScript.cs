using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneScript : MonoBehaviour
{
    // These are the different dialogues that happen at each level
    private TextMeshProUGUI[] statesText;

    public TextMeshProUGUI[] introStates;
    public TextMeshProUGUI[] lvl1_2States;
    public TextMeshProUGUI[] lvl2_3States;
    public TextMeshProUGUI[] lvl3_4States;
    public TextMeshProUGUI[] lvl4_5States;
    public TextMeshProUGUI[] endStates;
    public TextMeshProUGUI[] choiceStates;

    public GameObject[] images;
    private int imageIndex;
    /* each number has a predefined image associated with it:
     * 0 is boss, 1 is p1 happy, 2 is p1 sad, 3 is p2 happy, 4 is p2 sad, 5 is both sad, 6 is both happy, 7 is office bg, 8 is office error bg
     * 9 is p1 modifiable, 10 is p2 modifiable 
     */
    // these arrays store the order of the images that are shown for the cutscene based on the current scores of the players
    private int[] introOrder = {9,10,0,0,0,0,10,9,0};

    private int[] choiceOrderWinner = {0, 0, -1, 0, -1};
    private int[] choiceOrderLoseTie = { 0, 0, 5};
    private int[] choiceOrderWinTie = { 0, 0, 6 };

    private int[] choiceUsed;
    private int[] lvlUsed;

    private int[] lvl1_2Order = {0, 9, 10};
    private int[] lvl2_3Order = {0, 0, 1, 10 };
    private int[] lvl3_4Order = { 0, 9, 10 };
    private int[] lvl4_5Order = { 0, 0, 6, 10, 9, 3 };

    private int[] lvl5EndWinner = { 0, 0, -1, 0, -1, 0 };
    private int[] lvl5EndLoseTie = { 0, 0, 9, 3, 1 };
    private int[] lvl5EndWinTie = { 0, 0, 1, 3, 0, 1, 3, 9, 3};
    private int[] lvl5Used;

    // variables that hold the current winner/loser/etc at the current situation
    private int state;
    private int currentDialogue;
    private string winner;
    private string loser;
    private string winnerHandle;
    private string loserHandle;

    private int stopIndex;
    private bool donePerformanceReport;
    private bool doneAllText;

    /* modify player eye graphics from full circle to winking 
     * 0 is p1 Left eye, 1 is p1 Right, 2 is p2 Left, 3 is p2 Right
     */
    public Image[] playerEyes;

    // create face animation based on required emotion (playerEyes array will be used for int player)
    private void ChangeEyes(int player, string face) 
    {
        if (face.Equals("question"))
        {
            // make one eye a semi circle, the other a full circle to create this emotion
            playerEyes[player].fillAmount = 0.5f;
            playerEyes[player+1].fillAmount = 1;
        }
        else if (face.Equals("surpised"))
        {
            playerEyes[player].fillAmount = 1;
            playerEyes[player + 1].fillAmount = 1;
        }
        else if (face.Equals("attitude"))
        {
            playerEyes[player].fillAmount = 0.5f;
            playerEyes[player + 1].fillAmount = 0.5f;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // initialize variables
        imageIndex = currentDialogue = 0;
        donePerformanceReport = false;
        doneAllText = false;

        // find out which level the game is coming from (and as a result which level heading to)
        state = InputManager.instance.GetSceneNum();

        // set player controls to cutscene mode
        GameObject[] players = InputManager.instance.GetPlayersCutscene();
        players[0].GetComponent<PlayerUI>().SetCutsceneMode(this);
        players[1].GetComponent<PlayerUI>().SetCutsceneMode(this);

        // start cutscene music
        InputManager.instance.gameObject.GetComponent<AudioLoopScript>().SetupMusic();

        // set the dialogue and images based on level
        switch (state)
        {
            case 0: // just started game
                statesText = introStates;
                introStates[currentDialogue].gameObject.SetActive(true);
                stopIndex = introStates.Length - 1;
                images[8].SetActive(true);
                break;
            case 1: // level 1 completed, heading to level 2
                statesText = lvl1_2States;
                choiceStates[currentDialogue].gameObject.SetActive(true);
                images[8].SetActive(true);
                lvlUsed = lvl1_2Order;
                break;
            case 2:
                statesText = lvl2_3States;
                choiceStates[currentDialogue].gameObject.SetActive(true);
                images[8].SetActive(true);
                lvlUsed = lvl2_3Order;
                break;
            case 3:
                statesText = lvl3_4States;
                choiceStates[currentDialogue].gameObject.SetActive(true);
                images[8].SetActive(true);
                lvlUsed = lvl3_4Order;
                break;
            case 4:
                statesText = lvl4_5States;
                choiceStates[currentDialogue].gameObject.SetActive(true);
                images[8].SetActive(true);
                lvlUsed = lvl4_5Order;
                break;
            case 5:
                statesText = endStates;
                endStates[currentDialogue].gameObject.SetActive(true);
                images[7].SetActive(true);
                break;
        }

        // helper method to traverse intro (just started game) dialogue and images
        if (state == 0)
        {
            ChangeImages();
        }
        //  determine end results if this is at the end of game
        else if (state == 5)
        {
            // set winner and loser information based on who has the higher score
            int finalStatus = DetermineWhosWinning();

            if (finalStatus == 1) // p1 winning
            {
                winner = "Blaze";
                loser = "Shiver";
                winnerHandle = "BLAZE";
                loserHandle = "SHIVER";
                stopIndex = 5;
                lvl5EndWinner[2] = 1;
                lvl5EndWinner[4] = 4;
                lvl5Used = lvl5EndWinner;
            }
            else if (finalStatus == 2) // p2 winning
            {
                loser = "Blaze";
                winner = "Shiver";
                loserHandle = "BLAZE";
                winnerHandle = "SHIVER";
                stopIndex = 5;
                lvl5EndWinner[2] = 3;
                lvl5EndWinner[4] = 2;
                lvl5Used = lvl5EndWinner;
            }
            else if (finalStatus == 0) // lose tie
            {
                currentDialogue = 5;
                stopIndex = 9;
                lvl5Used = lvl5EndLoseTie;
                ChangeEyes(0, "attitude");
            }
            else // win tie
            {
                currentDialogue = 9;
                stopIndex = statesText.Length - 1;
                lvl5Used = lvl5EndWinTie;
                ChangeEyes(0, "surpised");
            }

            // modify dialogue based on winner and loser
            statesText[1].text = "Congratulations " + winner + ", " + statesText[1].text;
            statesText[2].text = winnerHandle + ": " + statesText[2].text;
            statesText[3].text = "BOSS: Sigh… " + loser + ", " + statesText[3].text;
            statesText[4].text = loserHandle + ": " + statesText[4].text;
            // modify images shown based on info above
            ChangeImagesDynamic();
        }
        // otherwise they are coming from any level that is not at the start/end 
        else
        {
            // determine who is in the lead, and assign the winner/loser accordingly
            int CurrentStatus = DetermineWhosWinning();

            if (CurrentStatus == 1) // p1 winning
            {
                winner = "Blaze";
                loser = "Shiver";
                winnerHandle = "BLAZE";
                loserHandle = "SHIVER";
                stopIndex = 4;
                choiceUsed = choiceOrderWinner;
                choiceUsed[2] = 1;
                choiceUsed[4] = 4;
            }
            else if (CurrentStatus == 2) // p2 winning
            {
                loser = "Blaze";
                winner = "Shiver";
                loserHandle = "BLAZE";
                winnerHandle = "SHIVER";
                stopIndex = 4;
                choiceUsed = choiceOrderWinner;
                choiceUsed[2] = 3;
                choiceUsed[4] = 2;
            }
            else if (CurrentStatus == 0) // lose tie
            {
                currentDialogue = 4;
                stopIndex = 6;
                choiceUsed = choiceOrderLoseTie;
            }
            else // win tie
            {
                currentDialogue = 6;
                stopIndex = choiceStates.Length - 1;
                choiceUsed = choiceOrderWinTie;
            }
            choiceStates[1].text = "Good job " + winner + ", " + choiceStates[1].text;
            choiceStates[2].text = winnerHandle + ": " + choiceStates[2].text;
            choiceStates[3].text = "BOSS: " + loser + ", " + choiceStates[3].text;
            choiceStates[4].text = loserHandle + ": " + choiceStates[4].text;
            // modify images shown based on information above
            ChangeImagesDynamic();
        }

    }

    // helper method that is scripted to on which emotions to show at which point
    private void IntroFaceChanges()
    {
        // image index refers to the number of the image in the sequence shown
        if (imageIndex == 0)
        {
            // helper method that changes the face of a specific player based on the emotion given (0= p1, 2 = p2)
            ChangeEyes(0, "question");
        }
        else if (imageIndex == 6)
        {
            ChangeEyes(2, "attitude");
        }
        else
        {
            ChangeEyes(0, "surpised");
            ChangeEyes(2, "surpised");
        }
    }

    //
    private void RegLevelFaceChanges()
    {
        if (state == 1) // lvl 1 to 2
        {
            if (imageIndex == 1)
            {
                ChangeEyes(0, "question");
            }
            else if (imageIndex == 2)
            {
                ChangeEyes(2, "attitude");
            }
            else
            {
                ChangeEyes(0, "surpised");
                ChangeEyes(2, "surpised");
            }
        }
        else if (state == 2) // lvl 2 to 3
        {
            ChangeEyes(0, "surpised");
            ChangeEyes(2, "surpised");
        }
        else if (state == 3) // lvl 3 to 4
        {
            if (imageIndex == 1)
            {
                ChangeEyes(0, "attitude");
            }
            else
            {
                ChangeEyes(0, "surpised");
                ChangeEyes(2, "surpised");
            }
        }
        else if (state == 4) // lvl 4 to 5
        {
            if (imageIndex == 3)
            {
                ChangeEyes(2, "surpised");
            }
            if (imageIndex == 4)
            {
                ChangeEyes(0, "attitude");
            }
            else
            {
                ChangeEyes(0, "surpised");
                ChangeEyes(2, "surpised");
            }
        }

    }

    // traverse the images: enable the next one and disable the previous, and skip certain images in the middle based on scores
    // this function is for 'static' cutscenes that dont change based on player scores
    private void ChangeImages()
    {
        // special order of images for introduction
        if (state == 0)
        {
            IntroFaceChanges();
            // int[] introOrder = {9,10,0,0,0,10,9,0};
            if (imageIndex > 0)
            {
                images[introOrder[imageIndex - 1]].SetActive(false);
            }

            if (imageIndex < introOrder.Length)
            {
                images[introOrder[imageIndex++]].SetActive(true);
            }
        }
        // order of images for mission briefs for the next level
        else
        {
            RegLevelFaceChanges();
            if (imageIndex > 0)
            {
                images[lvlUsed[imageIndex - 1]].SetActive(false);
            }

            if (imageIndex < lvlUsed.Length)
            {
                images[lvlUsed[imageIndex++]].SetActive(true);
            }
        }
    }

    // traverse the images: enable the next one and disable the previous, and skip certain images in the middle based on scores
    // this function is for 'dynamic' cutscenes that change based on player scores
    private void ChangeImagesDynamic()
    {
        // if not final level, the order of images is preset (as shown in comments below) based on scores
        if (state != 5)
        {
            //choiceOrderWinner = { 0, 0, -1, 0, -1 };
            //choiceOrderLoseTie = { 0, 0, 5 };    
            //choiceOrderWinTie = { 0, 0, 6 };

            if (imageIndex > 0)
            {
                images[choiceUsed[imageIndex - 1]].SetActive(false);
            }

            if (imageIndex < choiceUsed.Length)
            {
                images[choiceUsed[imageIndex++]].SetActive(true);
            }
        }
        else
        {
            // final cutscene has special order of images
            if (imageIndex > 0)
            {
                images[lvl5Used[imageIndex - 1]].SetActive(false);
            }

            if (imageIndex < lvl5Used.Length)
            {
                images[lvl5Used[imageIndex++]].SetActive(true);
            }
        }
    }

    public void NextDialogue()
    {
        if (state != 0 && state != 5) // in between levels
        {
            choiceStates[0].gameObject.SetActive(false);
            // fist go through the 'performance report' where boss talks based on player scores (dynamic)

            if (!donePerformanceReport && Transition(choiceStates)) // if going to next dialogue succeeds then leave this method until called again
            {
                ChangeImagesDynamic();
                return;
            }

            if (!doneAllText)
            {
                ChangeImagesDynamic();
                imageIndex = 0;
            }

            // once players are told about their performance, go to next section of cutscene which briefs them on their next mission
            doneAllText = donePerformanceReport = true;
            stopIndex = statesText.Length - 1;

            if (!Transition(statesText)) // if going to next dialogue fails i.e. reached to end of dialogue
            {
                StartNextLevel();
                return;
            }
            ChangeImages();
        }
        else if (state == 0) // intro scene
        {
            doneAllText = true;
            if (!Transition(statesText)) // if going to next dialogue fails i.e. reached to end of dialogue
            {
                StartNextLevel();
                return;
            }
            ChangeImages();
        }
        else // final result after lvl 5
        {
            statesText[0].gameObject.SetActive(false);

            doneAllText = true;
            if (!Transition(statesText)) // if going to next dialogue fails i.e. reached to end of dialogue
            {
                LoadCredits();
                return;
            }
            ChangeImagesDynamic();
        }
    }

  
    // actual method to handle enabling/disabling images in array
    private bool Transition(TextMeshProUGUI[] slides)
    {
        // advance the index of array
        currentDialogue++;

        // if for the current situation, further dialogue is not relevent, then return 'failed'
        if (currentDialogue > stopIndex && doneAllText)
        {
            currentDialogue = -1;
            return false;
        }
        // if all dialogue is done, then remove last dialogue and then return 'failed'
        if (currentDialogue > stopIndex)
        {
            slides[currentDialogue - 1].gameObject.SetActive(false);
            currentDialogue = -1;
            return false;
        }
        // if its not the first dialogue, then disable the last one
        if (currentDialogue > 0)
        {
            slides[currentDialogue - 1].gameObject.SetActive(false);
        }
        // enable the next dialogue and return 'successful'
        slides[currentDialogue].gameObject.SetActive(true);
        return true;
    }

    private void StartNextLevel()
    {
        LevelWinCondition.LoadScene(state + 1);
    }


    private void LoadCredits()
    {
        InputManager.instance.ResetGame();
        LevelWinCondition.LoadScene(7);
    }

    // get scores of players and compare to minimum required score to determine current status
    private int DetermineWhosWinning()
    {
        int[] scores = InputManager.instance.getTotalScores();
        int minScore = LevelWinCondition.GetMinScoreBasedOnScene(state);
        bool p1AboveMin = scores[0] >= minScore;
        bool p2AboveMin = scores[1] >= minScore;

        if (scores[0] > scores[1] && p1AboveMin && !p2AboveMin) // p1 winning
        {
            return 1;
        }
        else if (scores[0] < scores[1] && !p1AboveMin && p2AboveMin) // p2 winning
        {
            return 2;
        }
        else if (!p1AboveMin && !p2AboveMin) // both losing
        {
            return 0;
        }
        else // both doing ok
        {
            return 3;
        }
    }

}
