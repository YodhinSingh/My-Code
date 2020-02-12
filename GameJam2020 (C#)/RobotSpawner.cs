using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotSpawner : MonoBehaviour
{
    public GameObject Robot;
    public GameObject RobotSpawnPoint;
    GameObject Temp_Robot;
    bool AllowCreate;
    public int index;

    // Start is called before the first frame update
    void Start()
    {
        AllowCreate = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && AllowCreate)
        {
            AllowCreate = false;
            CreateBot(other.gameObject);
            Destroy(gameObject);
        }
    }

    void CreateBot(GameObject other)
    {
        if (index < other.GetComponent<RunningScript>().robots.Count)
            other.GetComponent<RunningScript>().robots[index].SetActive(true);
    }
}
