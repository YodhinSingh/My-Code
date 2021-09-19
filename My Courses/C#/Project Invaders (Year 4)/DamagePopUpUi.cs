using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamagePopUpUi
{
    // hold all the UI damage numbers
    public static DamageUiScript[] UI = new DamageUiScript[21];
    static int curElementIndex = 0;
    static int curElementIndexForAdding = 0;

    // have reference to each player's camera
    public static Camera[] cam = new Camera[2];

    // put a UI number on the screen of the player that called it (by attacking an enemy)
    public static void ShowRandomUi(int index, int damageValue, Vector3 pos, bool isElement)
    {
        // get an unused UI pop up
        DamageUiScript d = UI[curElementIndex];
        if (d == null)
        {
            return;
        }
        // add the damage number
        d.ShowDamage(damageValue, pos, cam[index], isElement);
        // modify array index
        UpdateIndex();
    }

    private static void UpdateIndex()
    {
        // loop index back to start (each pop up will only last a few seconds and then can be reused)
        curElementIndex = (curElementIndex + 1) % UI.Length;
    }

    public static void AddUI(DamageUiScript d)
    {
        // damage ui script class can add itself to this list
        UI[curElementIndexForAdding++] = d;
    }

    public static void ClearList()
    {
        curElementIndex = 0;
        curElementIndexForAdding = 0;
        UI = new DamageUiScript[21];
    }
}
