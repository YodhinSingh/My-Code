using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHolder : MonoBehaviour
{
    // public variables of objects needed
    public GameObject[] buildings;
    public roadCheckpoint[] roadPoints;
    public Dictionary<Vector2, Transform> roadCheckpoints = new Dictionary<Vector2, Transform>();

    // reference for other scripts to access
    public static BuildingHolder instance;

    // called before start
    void Awake()
    {
        // only one reference exists of this for all scenes
        instance = this;

        // add all checkpoints into list
        foreach (roadCheckpoint t in roadPoints)
        {
            roadCheckpoints.Add(t.tableID, t.transform);
        }
    }
    
    // get a list of random buildings from selection based on length chosen
    public GameObject[] GetRandomBuildings(int length)
    {
        GameObject[] b = new GameObject[length];
        for (int i = 0; i < b.Length; i++)
        {
            int rand = Random.Range(0, buildings.Length);
            b[i] = buildings[rand];
        }

        return b;
    }

    /* get all ajacent intersections of a grid based on [row,col] chosen
     * e.g. [0,0] has [1,0] and [0,1] adjacent to it in a grid (only horizontal/vertical connections, no diagonal)
    */
    public Transform[] GetAjacentPoints(int row, int col)
    {
        Transform[] ajacentPoints;

        // get adjacent rows to the specified row using helper method (r.x is one row, r.y is another row)
        Vector2 r = GetGridNumber(row, 1);
        // get adjacent columns to the specified column (c.x is one column, c.y is another)
        Vector2 c = GetGridNumber(col, 1);

        // if specified point is not on any edge (4 adjacent points in grid)
        if (r.y != 0 && c.y != 0) 
        {
            ajacentPoints = new Transform[4];
            ajacentPoints[0] = roadCheckpoints[new Vector2(r.x, col)];
            ajacentPoints[1] = roadCheckpoints[new Vector2(r.y, col)];
            ajacentPoints[2] = roadCheckpoints[new Vector2(row, c.x)];
            ajacentPoints[3] = roadCheckpoints[new Vector2(row, c.y)];
        }
        // if on one of the 4 corners (2 adjacent points in grid)
        else if (r.y == 0 && c.y == 0) 
        {
            ajacentPoints = new Transform[2];
            ajacentPoints[0] = roadCheckpoints[new Vector2(r.x, col)];
            ajacentPoints[1] = roadCheckpoints[new Vector2(row, c.x)];
        }
        // if on top/bottom row (3 adjacent points in grid)
        else if (r.y == 0) 
        {
            ajacentPoints = new Transform[3];
            ajacentPoints[0] = roadCheckpoints[new Vector2(r.x, col)];
            ajacentPoints[1] = roadCheckpoints[new Vector2(row, c.x)];
            ajacentPoints[2] = roadCheckpoints[new Vector2(row, c.y)];
        }
        // (c.y == 0) if on first/last column (3 adjacent points in grid)
        else 
        {
            ajacentPoints = new Transform[3];
            ajacentPoints[0] = roadCheckpoints[new Vector2(r.x, col)];
            ajacentPoints[1] = roadCheckpoints[new Vector2(r.y, col)];
            ajacentPoints[2] = roadCheckpoints[new Vector2(row, c.x)];
        }

        return ajacentPoints;

    }

    // helper method to determine adjacent grid values
    private Vector2 GetGridNumber(int value, int modifier)
    {
        int v;
        int v2 = 0; // if this remains as 0, that means the point is on an edge
        switch (value)
        {
            case 1: // original point is on the left/top edge, return only the one to the right/below of it
                v = 2;
                break;
            case 5: // original point is on the right/bottom edge, return only the one to the left/top of it
                v = 4;
                break;
            default: // original point is not on any edge, return both points on each side
                v = value + modifier;
                v2 = value - modifier;
                break;
        }

        return new Vector2(v,v2);
    }
}
