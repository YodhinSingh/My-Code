using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSuitColour : MonoBehaviour
{
    // materials to use
    public Material[] playerSpecific;
    public Material[] swordSpecific;

    // modify materials on player model
    public void PutMaterialsOnBody(SkinnedMeshRenderer b, int playerIndex)
    {
        // get materials
        Material[] mBody = b.materials;

        // put player specific colours on the model
        mBody[3] = playerSpecific[playerIndex];

        // apply change
        b.sharedMaterials = mBody;
    }

    // modify materials on sword model
    public void PutMaterialOnSword(MeshRenderer m, int playerIndex)
    {
        Material[] mBody = m.materials;

        // modify sword and tilt based on player colour scheme
        mBody[1] = playerSpecific[playerIndex];
        mBody[0] = swordSpecific[playerIndex];

        // apply change
        m.sharedMaterials = mBody;
    }


}
