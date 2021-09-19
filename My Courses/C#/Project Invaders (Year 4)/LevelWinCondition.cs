using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelWinCondition
{
    // handler class on winning requirements

    // references
    public static GameObject winScreen;
    public static GameObject loseScreen;
    public static int enemyKillCount = 0;
    public static GameObject p1;
    public static GameObject p2;

    public static bool Gameover = false;
    private static bool isPaused = false;

    private static int[] minScoresInitial = {2250, 4000, 4500, 5600, 7500 };

    private static int[] minScores = { 2250, 6250, 10750, 16350, 23850 };

    public static void BossIsDead()
    {
        // Win screen
        TextMeshProUGUI title = winScreen.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        title.text = "LEVEL " + GetSceneNum() + " COMPLETE";
        winScreen.SetActive(true);
        StopAllGamePlay(true);
    }

    public static void GameOver()
    {
        // Lose Screen
        loseScreen.SetActive(true);
        StopAllGamePlay(false);
    }

    // switch player controls to UI
    private static void SwitchPlayerControls()
    {
        InputManager.instance.SetPlayerUI();
    }

    // stop all enemy/player scripts
    public static void StopAllGamePlay(bool isWin)
    {
        SwitchPlayerControls();
        Gameover = true;
        DamagePopUpUi.ClearList();

        p1.GetComponentInParent<PlayerScript>().AllowControls = false;
        p2.GetComponentInParent<PlayerScript>().AllowControls = false;

        if (!isWin)
        {
            return;
        }

        // if p1 is a drone, then only p2 gets enemy bonus
        if (p1.tag == "Drone" && p2.tag != "Drone")
        {
            SetScores(1, 1000, 0, 1000);
            SetScores(2, 1000, enemyKillCount * 100, 1000);
        }
        // if p2 is a drone, then only p1 gets enemy bonus
        else if (p2.tag == "Drone" && p1.tag != "Drone")
        {
            SetScores(1, 1000, enemyKillCount * 100, 1000);
            SetScores(2, 1000, 0, 1000);
        }
        // if both alive, then shared equally, regardless of who killed how many
        else
        {
            SetScores(1, 1000, (enemyKillCount/2f) * 100, 1000);
            SetScores(2, 1000, (enemyKillCount/2f) * 100, 1000);
        }
    }

    // show scores on UI
    private static void SetScores(int playerIndex, int baseScore, float enemyXP, int bossXP)
    {
        TextMeshProUGUI score;
        baseScore *= GetSceneNum();

        int total = (int)(baseScore + enemyXP + bossXP);

        float multiplier = LevelMultiplier(total);

        // update player 1 UI for score
        if (playerIndex == 1)
        {
            score = winScreen.transform.Find("P1Score").GetComponent<TextMeshProUGUI>();
            score.fontSize = 60;
            score.text = "BLAZE:\n";
            p1.GetComponentInParent<PlayerCombat>().SetUpDamageValues(multiplier);
            p1.GetComponentInParent<PlayerScript>().ResetPlayer();
        }
        // update player 2 UI
        else
        {
            score = winScreen.transform.Find("P2Score").GetComponent<TextMeshProUGUI>();
            score.fontSize = 60;
            score.text = "SHIVER:\n";
            p2.GetComponentInParent<PlayerCombat>().SetUpDamageValues(multiplier);
            p2.GetComponentInParent<PlayerScript>().ResetPlayer();
        }
        // update general UI
        InputManager.instance.setTotalScore(playerIndex - 1, total);
        score.text += "BASE XP: " + baseScore + "\nENEMIES XP: " + enemyXP +"\nBOSS XP: " + bossXP + "\nLEVEL: " + total + " PTS";
        score.text += "\n\nTOTAL: " + InputManager.instance.getTotalScores()[playerIndex - 1] + " PTS";
        score.text += "\nMIN REQ: " + minScores[GetSceneNum()-1] + " PTS";

    }

    public static bool IsGamePaused()
    {
        return Gameover || isPaused;
    }

    public static void SetGamePaused(bool isTrue)
    {
        if (isTrue)
        {
            isPaused = true;
        }
        else
        {
            isPaused = false;
        }
    }

    // get  min score needed to do well in cutscene
    public static int GetMinScoreBasedOnScene(int value)
    {
        return minScores[value - 1];
    }

    private static float LevelMultiplier(float score)
    {
        float level = SceneManager.GetActiveScene().buildIndex;

        return (score / 10000f) * 1.2f;
    }

    public static void Reset()
    {
        enemyKillCount = 0;
        Gameover = false;
    }

    public static int GetSceneNum()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    // load the next level
    public static void LoadScene(int sceneNum)
    {
        switch (sceneNum)
        {
            case -1:
                Application.Quit();    // Quit
                break;
            case 0:
                InputManager.instance.ResetGame();
                SceneManager.LoadScene("Menu");    // main menu
                break;
            case 1:
                SceneManager.LoadScene("Level One");    // level 1
                break;
            case 2:
                SceneManager.LoadScene("Level Two");    // level 2
                break;
            case 3:
                SceneManager.LoadScene("Level Three");    // level 3
                break;
            case 4:
                SceneManager.LoadScene("Level Four");    // level 4
                break;
            case 5:
                SceneManager.LoadScene("Level Five");    // level 5
                break;
            case 6:
                SceneManager.LoadScene("Cutscene");     // cutscene called between levels to get progress
                break;
            case 7:
                SceneManager.LoadScene("Credits");      // final credits
                break;
        }

    }

}
