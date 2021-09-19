using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpScreenLayouts : MonoBehaviour
{
    // UI script to change between layouts on help menu (show the various controls based on inputs)

    public GameObject[] layouts;

    private int index;
    // Start is called before the first frame update
    void Start()
    {
        index = 0;
    }


    public void NextLayout()
    {
        layouts[index].SetActive(false);

        index = Mathf.Min(index + 1, layouts.Length - 1);

        layouts[index].SetActive(true);
    }

    public void PrevLayout()
    {
        layouts[index].SetActive(false);

        index = Mathf.Max(index - 1, 0);

        layouts[index].SetActive(true);
    }

}
