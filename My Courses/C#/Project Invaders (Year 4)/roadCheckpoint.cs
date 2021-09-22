using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roadCheckpoint : MonoBehaviour
{
    // properties
    public Vector2 tableID; // give row,col of pos
    private Vector3 pos;

    // reference to other points
    public Transform[] ajacentPoints;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;

        GetAjacentPoints();
    }

    private void GetAjacentPoints()
    {
        // find out all adjacent intersection points from building holder class
        ajacentPoints = BuildingHolder.instance.GetAjacentPoints((int)tableID.x, (int)tableID.y);

    }

    // called when an enemy wants to know the next point they should head towards
    public Transform GetNextPoint(Transform previousPoint)
    {
        int rand;
        
        // randomly choose the next point for an enemy that is adjacent to this one, but it cannot be the one that they just came from before this
        do
        {
            rand = Random.Range(0, ajacentPoints.Length);

        } while (ajacentPoints[rand] == previousPoint);
        
        /*
        rand = Random.Range(0, ajacentPoints.Length);           // other way to get next point, will be faster than above but less random
        if (ajacentPoints[rand] == previousPoint)
        {
            return ajacentPoints[(rand + 1) % ajacentPoints.Length];
        }
        */
        return ajacentPoints[rand];

    }
}
