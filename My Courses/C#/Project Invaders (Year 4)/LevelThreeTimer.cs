using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelThreeTimer : MonoBehaviour
{
    // timer
    public int numberOfSeconds;
    private float numSeconds;

    private TextMeshProUGUI timer;
    private bool MissionOver = false;

    // Start is called before the first frame update
    void Start()
    {
        // apply number to UI
        numSeconds = numberOfSeconds + 1;
        timer = GetComponent<TextMeshProUGUI>();
        timer.text = ConvertSecondsToMinuteFormat();
    }

    // Update is called once per frame
    void Update()
    {
        // lower count every second
        numSeconds -= Time.deltaTime;

        // if timer reaches 0, mission is failed for this level
        if (numSeconds <= 0)
        {
            timer.text = "";
            MissionOver = true;
            LevelWinCondition.GameOver();
            return;
        }
        // ignore anything if mission is over
        else if (MissionOver)
        {
            return;
        }

        MissionOver = LevelWinCondition.Gameover;
        timer.text = ConvertSecondsToMinuteFormat();

    }

    private string ConvertSecondsToMinuteFormat()
    {
        int minutes = Mathf.FloorToInt(numSeconds / 60F);
        int seconds = Mathf.FloorToInt(numSeconds - minutes * 60);
        return string.Format("{0:0}:{1:00}", minutes, seconds);

    }

}
