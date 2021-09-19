using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDistCheck : MonoBehaviour
{
    // static var to call for other classes
    public static EnemyDistCheck instance;

    // holds all enemies on level
    private List<GameObject> enemies = new List<GameObject>();

    // player references
    private Transform[] players = new Transform[2];

    // max disance from players
    private float maxDist = 300;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        GameObject[] p = InputManager.instance.GetReferenceToPlayers();
        players[0] = p[0].transform;
        players[1] = p[1].transform;

        InvokeRepeating("CheckEnemyRange", 1f, 10f);
    }

    // every x seconds, check how far each enemy is from each player and if they are too far, then disable them until they are close again
    private void CheckEnemyRange()
    {
        if (LevelWinCondition.Gameover)
        {
            return;
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null || players[0] == null || players[1] == null)
            {
                return;
            }

            float distP1 = Vector3.Distance(enemies[i].transform.position, players[0].position);
            float distP2 = Vector3.Distance(enemies[i].transform.position, players[1].position);

            if (distP1 <= maxDist || distP2 <= maxDist)
            {
                enemies[i].SetActive(true);
            }
            else if (distP1 > maxDist && distP2 > maxDist)
            {
                enemies[i].SetActive(false);
            }
        }
    }


    public void AddEnemy(GameObject t)
    {
        enemies.Add(t);
    }

    public void RemoveEnemy(GameObject t)
    {
        enemies.Remove(t);
    }
}
