using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlsMenu : MonoBehaviour
{
    public Image spaceBar;          // Images of controls on screen, pressing the corresponding buttons changes their colour to blue.
    public Image wKey;
    public Image aKey;
    public Image sKey;
    public Image dKey;
    public TMPro.TMP_Text spaceBarText;
    public TMPro.TMP_Text wKeyText;
    public TMPro.TMP_Text aKeyText;
    public TMPro.TMP_Text sKeyText;
    public TMPro.TMP_Text dKeyText;
    public Image leftMouse;
    public Image rightMouse;
    public Color whiteColor;
    public Color blueColor;

    // Start is called before the first frame update
    void Start()
    {
        whiteColor = new Color(1f, 1f, 1f, 1f);
        blueColor = new Color(175f / 255f, 175f / 255f, 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey ("escape"))
        {
            OpenMainMenu();
        }
        else if (Input.GetKey ("space"))
        {
            spaceBar.color = blueColor;
            spaceBarText.color = blueColor;
        }
        else if (Input.GetKey ("w"))
        {
            wKey.color = blueColor;
            wKeyText.color = blueColor;
        }
        else if (Input.GetKey ("a"))
        {
            aKey.color = blueColor;
            aKeyText.color = blueColor;
        }
        else if (Input.GetKey ("s"))
        {
            sKey.color = blueColor;
            sKeyText.color = blueColor;
        }
        else if (Input.GetKey ("d"))
        {
            dKey.color = blueColor;
            dKeyText.color = blueColor;
        }
        else if (Input.GetMouseButton(0))
        {
            leftMouse.color = blueColor;
        }
        else if (Input.GetMouseButton(1))
        {
            rightMouse.color = blueColor;
        }
        else
        {
            spaceBar.color = whiteColor;
            wKey.color = whiteColor;
            aKey.color = whiteColor;
            sKey.color = whiteColor;
            dKey.color = whiteColor;
            leftMouse.color = whiteColor;
            rightMouse.color = whiteColor;
            spaceBarText.color = whiteColor;
            wKeyText.color = whiteColor;
            aKeyText.color = whiteColor;
            sKeyText.color = whiteColor;
            dKeyText.color = whiteColor;
}
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
