using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypeScript : MonoBehaviour
{
    // materials for enemy
    public Material both;
    public Material fire;
    public Material ice;

    public SkinnedMeshRenderer m;
    
    public void SetUp(int elementType, bool isBoss)
    {
        if (m == null)
        {
            return;
        }

        // change material based on element
        Material temp;
        switch (elementType)
        {
            case 0: // fire weakness creature
                temp = ice;
                break;
            case 1: // ice weakness creature
                temp = fire;
                break;
            default: // normal
                temp = both;
                break;
        }

        Material[] mColours = m.materials;

        // bosses have specific index to their model
        if (isBoss)
        {
            mColours[6] = temp;
        }
        // regular enemies have other indecies
        else
        {
            for (int i = 0; i < mColours.Length; i++)
            {
                if (mColours[i].name.Equals("ReplaceBugColour (Instance)"))
                {
                    mColours[i] = temp;
                }
            }
        }

        m.sharedMaterials = mColours;


    }
   
}
