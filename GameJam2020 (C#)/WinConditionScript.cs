using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinConditionScript : MonoBehaviour
{
    // this keeps track of all repairable objs and their condition

    public List <GameObject> repairs = new List<GameObject>();
    public GameObject player;
    bool NotFixed;
    int counter;
    public TMPro.TMP_Text Obj;
    // Start is called before the first frame update
    void Start()
    {
        NotFixed = true;
        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (counter == repairs.Count)
        {
            player.GetComponent<RunningScript>().DisableControls();
            Obj.gameObject.SetActive(false);
            PauseMenu.endConditionWin = true;
            Time.timeScale = 0f;
        }

        Obj.text = counter + "/" + repairs.Count + "\nRepaired";
        print(counter);
    }

    public void addRepair(GameObject r)
    {
        repairs.Add(r);
    }

    public void IncreaseCounter()
    {
        counter++;
    }

}
