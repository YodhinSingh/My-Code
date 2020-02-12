using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class scoreManager : MonoBehaviour
{
    //keeps track of score of player from collectibles

    public int score;
    public Text scoreDisplay;

    private void Update()
    {
        scoreDisplay.text = "Score: " + score.ToString();
        PlayerPrefs.SetInt("Score", score);
        if (PlayerPrefs.GetInt("HighScore") < score)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("obstacle"))
        {
            increaseScore(1);
        }
    }
    public void increaseScore(int num)
    {
        score = score + num;
    }
}
