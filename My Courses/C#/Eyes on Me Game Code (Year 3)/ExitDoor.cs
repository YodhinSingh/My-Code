using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{

    // Destroy the circuit (= key) and remove that circuit's symbol from main lock door to indicate it is taken care of

    public GameObject door;
    int total = 0;

    public void CheckCompletion(int num)
    {
        if (num == 1)
        {
            GameObject.Find("Circuit1").SetActive(false);
            GameObject.Find("pasted__pCube2").SetActive(false);
        }
        if (num == 2)
        {
            GameObject.Find("Circuit2").SetActive(false);
            GameObject.Find("pasted__pCube3").SetActive(false);
        }
        if (num == 3)
        {
            GameObject.Find("Circuit3").SetActive(false);
            GameObject.Find("pasted__pCube4").SetActive(false);
        }
        total += num;
        if (total == 6) // once all the circuits (3 total) taken care of, destroy this door
        {
            door.SetActive(false);
        }
    }
}
