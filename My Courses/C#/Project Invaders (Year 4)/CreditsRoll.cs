using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsRoll : MonoBehaviour
{
    // basic script to handle scrolling down of a text box with the credit info

    RectTransform t;
    float speed = 70f;
    bool scrolling = false;

    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<RectTransform>(); // (-738 starting point value in editor)
        scrolling = true;
        InputManager.instance.gameObject.GetComponent<AudioLoopScript>().SetupMusic();
    }

    // Update is called once per frame
    void Update()
    {
        if (scrolling)
        {
            // gradually scroll through credits until text box is finished
            t.Translate(Vector3.up * Time.deltaTime * speed);
            if (t.localPosition.y > 1970)
            {
                scrolling = false;
                LoadMenu();
            }
        }
    }

    private void LoadMenu()
    {
        LevelWinCondition.LoadScene(0);
    }
}
