using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    int score = 0;
    public Text scoreText;
    public Text highScoreText;
    int highScore = 0;

    // Use this for initialization
    void Start()
    {
        score = PlayerPrefs.GetInt("Score");
        scoreText.text = "Your Score: " + score.ToString();
        highScore = PlayerPrefs.GetInt("HighScore");
        highScoreText.text = "HighScore: " + highScore.ToString();
    }

}
