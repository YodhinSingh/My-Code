using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEnemyTravelPath : MonoBehaviour
{
    // store information on travel targets
    private Transform[] carTarget = new Transform[2];

    // only 1 reference needed for leader car, all other cars simply follow this one
    public static CarEnemyTravelPath instance;

    public Transform leader;
    private bool coolDownOver = true;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // give initial target values
        GiveCarTarget();
    }

    // get target destination  
    public Transform GetTargetPoint()
    {
        return carTarget[0];
    }

    // get position of boss enemey/leader
    public Transform GetLeaderPos()
    {
        return leader;
    }

    // assign the next target intersection for car to head towards
    public Transform GetNextPoint()
    {
        // assign carTarget[0] as next destination, and [1] as last reached destination
        Transform t = carTarget[0];
        carTarget[0] = carTarget[0].GetComponent<roadCheckpoint>().GetNextPoint(carTarget[1]);
        carTarget[1] = t;
        return carTarget[0];
    }

    // assign an inital target intersection for car to head towards
    public void GiveCarTarget()
    {
        carTarget[0] = BuildingHolder.instance.roadPoints[0].transform;     // current target
        carTarget[1] = null;     // previous target
    }

    // interal cooldown before choosing next point (safety measure to avoid inifinite choosing)
    public bool CanGetNextPoint()
    {
        if (coolDownOver)
        {
            return true;
        }
        return false;
    }

    private IEnumerator cooldown()
    {
        yield return new WaitForSeconds(1f);
        coolDownOver = true;
    }

}
