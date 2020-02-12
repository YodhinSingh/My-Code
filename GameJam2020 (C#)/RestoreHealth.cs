using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestoreHealth : MonoBehaviour
{
    private float health;
    private bool isComplete;
    public GameObject repairedObject;
    public GameObject damagedObject;

    public GameObject healthbar;
        // Start is called before the first frame update
    void Start()
    {
        health = 0;
        isComplete = false;
        GetComponentInParent<WinConditionScript>().addRepair(gameObject);   // add this obj to list of repairable objs
    }

    private void Update()
    {
        if (healthbar != null)  // set healthbar size
        {
            healthbar.GetComponent<Image>().fillAmount = health / 100;
        }
    }

    public void restoreHealth() // called whenever drone shoots the target
    {
        if (!isComplete)
        {
            health += 0.5f;

            if (health >= 100)  // is object repaired?
            {
                isComplete = true;
                GetComponentInParent<WinConditionScript>().IncreaseCounter();
                repairedObject.SetActive(true);
                damagedObject.SetActive(false);
            }
        }
    }

    public float getHealth()
    {
        return health;
    }

    public bool getComplete()
    {
        return isComplete;
    }
}
