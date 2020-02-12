using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enableInstruction : MonoBehaviour
{

    // Makes the objective menu appear/disappear for the player

    public Image WASD;   // controls shown at the beginning of game on wall
    public Image Mouse; // controls shown at the beginning of game on wall
    public GameObject hologramOBJ;  // Objective menu

    bool AllowChange = true;

    // Start is called before the first frame update
    void Start()
    {
        WASD.gameObject.SetActive(false);
        Mouse.gameObject.SetActive(false);
        hologramOBJ.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (AllowChange && Input.GetKeyDown(KeyCode.E)){
            showIntructions();
            AllowChange = false;
            this.gameObject.SetActive(false);
        }
    }

    private void showIntructions()
    {
            WASD.gameObject.SetActive(true);    
            Mouse.gameObject.SetActive(true);
            hologramOBJ.SetActive(true);  

    }
}
