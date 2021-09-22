using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleKeyBoss : MonoBehaviour
{
    // models
    public GameObject[] keyModels;
    public GameObject door;

    // spawner
    public EnemySpawner spawner;

    // acknowledges when a key has been won, and disables the corresponding key model on the main locked door
    public void KeyUnlocked(int index)
    {
        keyModels[index - 1].SetActive(false);
        // checks to see if all keys have been won
        CheckIfComplete();
    }

    // determines if all puzzles have been completed for the  level
    private void CheckIfComplete()
    {
        // checks if it is completed by seeing if all the key models are gone
        for (int i = 0; i < keyModels.Length; i++)
        {
            if (keyModels[i].activeInHierarchy)
            {
                // if not then leave for now
                return;
            }
        }

        // if complete, then disable the door and stop the enemy spawner from creating more enemies
        door.SetActive(false);
        spawner.StopGeneralSpawning();
        // let manager know that the puzzles are done
        InputManager.instance.Lvl4ObjectiveComplete();
    }
}
