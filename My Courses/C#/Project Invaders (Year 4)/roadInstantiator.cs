using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roadInstantiator : MonoBehaviour
{
    // properties
    public GameObject roadTilePrefab;
    public int numOfRows = 9;               // make sure its odd
    private const int DIST_BTWN_TILES = 217; //  hard coded based on model

    private void Awake()
    {
        // starting point
        Vector3 pos = new Vector3(-DIST_BTWN_TILES * (numOfRows/2), 0, DIST_BTWN_TILES * (numOfRows / 2));

        for (int i = 0; i < numOfRows; i++) // rows
        {
            for (int j = 0; j <numOfRows; j++) // columns
            {
                // create a square grid of this model spaced equally
                Instantiate(roadTilePrefab, pos, Quaternion.identity);
                // move to next column
                pos.Set(pos.x + DIST_BTWN_TILES, 0, pos.z);
            }
            // move to next row
            pos.Set(-DIST_BTWN_TILES * (numOfRows / 2), 0, pos.z - DIST_BTWN_TILES);
        }
    }
}
