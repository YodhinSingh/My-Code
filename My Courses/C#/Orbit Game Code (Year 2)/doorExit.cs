using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorExit : MonoBehaviour
{
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();   // find the player in scene
    }

    // Update is called once per frame
    void Update()
    {
        if (player.keyNum == 3) // check if the player has all 3 keys. If they do, then 'unlock' the door by rotating it upwards and placing it at the hinge.
        {
            transform.eulerAngles = new Vector3(0, 0, 90f);
            transform.position = new Vector3(58f, -61.8f, 0f);
        }
    }
}
