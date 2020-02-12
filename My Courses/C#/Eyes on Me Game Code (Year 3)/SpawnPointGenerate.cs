using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPointGenerate : MonoBehaviour
{
    public GameObject player;
    public GameObject[] spawnPoints;
    public GameObject[] spawnAmmo;
    public GameObject[] permSpawnPoints;
    public GameObject[] spawnPointsPuzzle;

    public int numItems;

    private float time;
    private int timeTillRespawn = 60;
    private GameObject AmmoCoinPerm;

    public Text countdownTimer;
    public GameObject holo;


    // Start is called before the first frame update
    void Start()
    {
        time = timeTillRespawn;
        countdownTimer.text = "";
        holo.SetActive(false);

        for (int i = 0; i < numItems; i++)      // generate ammo pickups at locations that are currently empty
        {
            int randomIndex = Random.Range(0, spawnPoints.Length);
            if (spawnPoints[randomIndex].CompareTag("EmptySpawnAmmo"))
            {
                if (spawnPoints[randomIndex].GetComponent<followParent>() != null)
                    PickSpawnAmmo(spawnPoints[randomIndex].transform.position, Quaternion.identity, true);
                else
                    PickSpawnAmmo(spawnPoints[randomIndex].transform.position, Quaternion.identity, false);
         
                spawnPoints[randomIndex].tag = "UsedSpawnAmmo";     // chnge its tag to say its used
            }
            else if (spawnPoints[randomIndex].CompareTag("UsedSpawnAmmo"))
            {
                i--;        // must have exact number of ammo pickups, no less if it picks a spot that was filled
            }
        }

        for (int i = 0; i < spawnPointsPuzzle.Length; i++)  // puzzle ammo points are always filled with ammo pick up
        {
            PuzzleSpawnAmmo(spawnPointsPuzzle[i].transform.position, Quaternion.identity, spawnPointsPuzzle[i].GetComponent<SpawnProbability>().puzAmmoType);
            spawnPointsPuzzle[i].tag = "UsedSpawnAmmo";
        }

        for (int i = 0; i < permSpawnPoints.Length; i++)        // permanent ammo points are the only ones that are constantly refilled whenever empty after some time
        {
            PickAmmoBasedProb(permSpawnPoints[i].transform.position, Quaternion.identity);
            permSpawnPoints[i].tag = "UsedSpawnAmmo";
        }

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < permSpawnPoints.Length; i++)        // refill permanent ammo points if they are empty and its been time seconds
        {
            if (permSpawnPoints[i].CompareTag("EmptySpawnAmmo"))
            {
                countdownTimer.text = ((int)time).ToString();
                time = time - Time.deltaTime;
                holo.SetActive(true);
                
                if (time <= 1)
                {
                    PickAmmoBasedProb(permSpawnPoints[i].transform.position, Quaternion.identity);
                    permSpawnPoints[i].tag = "UsedSpawnAmmo";
                    time = timeTillRespawn;
                    countdownTimer.text = "";
                    holo.SetActive(false);
                }
            }

        }
    }

    public void ResetTag(int num)
    {
        permSpawnPoints[num].tag = "EmptySpawnAmmo";
    }

    void PickSpawnAmmo(Vector3 pos, Quaternion rot, bool allow)
    {
        GameObject temp;
        int randomIndex = Random.Range(0, spawnAmmo.Length);
        temp = Instantiate(spawnAmmo[randomIndex], pos, rot) as GameObject;     // instantiates ammo at point
        if (allow)
        {
            temp.transform.SetParent(spawnPoints[12].transform);        // if its on moving platform, then parent it on it so it moves with it
        }
    }

    void PickAmmoBasedProb(Vector3 pos, Quaternion rot)     // determine which ammo to spawn based on current player ammo counts (probability based)
    {
        int[] bulletCount = player.GetComponentInChildren<bulletShoot>().GetAllAmmoCounts();
        float[] prob = new float[3];

        /*                          example scenario:
         * 5     3     4    = 12 -> total ammo
         * 5/12, 3/12, 4/12      -> ammo type / total
         * 7/24, 9/24, 8/24      -> inverse: prob of picking lowest ammo type
        */

        int totalAmmo = bulletCount[0] + bulletCount[1] + bulletCount[2];

        if (totalAmmo != 0)
        {
            prob[0] = ((totalAmmo - bulletCount[0] *1f) / (totalAmmo*2)) * 100f;
            prob[1] = ((totalAmmo - bulletCount[1] *1f) / (totalAmmo * 2)) * 100f;
            prob[2] = ((totalAmmo - bulletCount[2] *1f) / (totalAmmo * 2)) * 100f;
        }
        else if (totalAmmo == 0)
        {
            prob[0] = Random.Range(0, 101) *1f;
            prob[1] = Random.Range(0, 101) *1f;
            prob[2] = Random.Range(0, 101) *1f;
        }

        int rand = Random.Range(0, 100);
        int cumulative = 0;

        for (int i = 0; i < spawnAmmo.Length; i++)
        {
            cumulative += (int) prob[i];
            if (rand <= cumulative)
            {
                AmmoCoinPerm = Instantiate(spawnAmmo[i], pos, rot) as GameObject;
                AmmoCoinPerm.GetComponent<AmmoCoin>().Restart = true;
                break;
            }
        }
    }

    void PuzzleSpawnAmmo(Vector3 pos, Quaternion rot, int ver)  // instantiate ammo at location
    {
        Instantiate(spawnAmmo[ver], pos, rot);
    }
}
