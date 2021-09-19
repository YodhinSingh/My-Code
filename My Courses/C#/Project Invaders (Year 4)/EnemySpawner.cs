using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    // references to spawnable enemies, etc
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    public Transform teleportPoint;
    public ParticleSystem particles;
    int numEnemies;
    PuzzleThree puzzle3;
    PuzzleTwo puzzle2;
    PuzzleOne puzzle1;
    private bool hasSpawnwedAlready = false;
    public bool isGeneralSpawner = false;
    public TextMeshProUGUI canvasText;

    private int maxEnemies = 15;
    public EnergyBar energy;
    public Image visualOfMaxEnemies;

    private bool canSpawn = false;

    private void Start()
    {
        numEnemies = 0;
        // general spawner will spawn on intervals rather than only once
        if (isGeneralSpawner)
        {
            canSpawn = true;
            SpawnEnemiesAtPoint();
            InvokeRepeating("SpawnEnemyOnRegularInterval", 15, 15);
            CountEnemies();
            if (numEnemies == 0)
            {
                // spawn a random enemy at each spawn point
                for (int i = 0; i < spawnPoints.Length; i++)
                {
                    int rand = Random.Range(0, enemyPrefabs.Length);
                    GameObject e = Instantiate(enemyPrefabs[rand], spawnPoints[i].position, spawnPoints[i].rotation);
                    e.GetComponent<EnemyScript>().RecieveSpawnInfo(this);
                    numEnemies++;
                }
            }
        }
    }

    public void StopGeneralSpawning()
    {
        CancelInvoke();
        canSpawn = false;
        CountEnemies();
    }

    // called at a specific interval to to handle spawning and particle effects
    private void SpawnEnemyOnRegularInterval()
    {
        if (LevelWinCondition.Gameover || !canSpawn)
        {
            CancelInvoke();
            return;
        }

        particles.Play();

        int rand = Random.Range(0, enemyPrefabs.Length);

        GameObject e = Instantiate(enemyPrefabs[rand], teleportPoint.position, teleportPoint.rotation);
        e.GetComponent<EnemyScript>().RecieveSpawnInfo(this);
        e.GetComponentInChildren<EnemySightRange>().IncreaseRangeTemporarily(5, 3);
        numEnemies++;

        CountEnemies();
    }

    // keep track of how many enemies
    private void CountEnemies()
    {
        // display on screen
        canvasText.text = "x " + numEnemies.ToString();

        // have UI to show it with colours
        visualOfMaxEnemies.fillAmount = numEnemies / (maxEnemies + 1f);

        if (visualOfMaxEnemies.fillAmount > 0.8f)
        {
            visualOfMaxEnemies.color = Color.red;
            canvasText.color = Color.red;
        }
        else
        {
            visualOfMaxEnemies.color = Color.yellow;
            canvasText.color = Color.yellow;
        }


        if (numEnemies > maxEnemies)
        {
            energy.RemoveEnergy(100); // if too many enemies stay on 3rd floor, then lose
        }
    }

    // regular spawning for one time only
    private void SpawnEnemiesAtPoint()
    {
        if (LevelWinCondition.Gameover)
        {
            CancelInvoke();
            return;
        }

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            int rand = Random.Range(0, enemyPrefabs.Length);
            GameObject e = Instantiate(enemyPrefabs[rand], spawnPoints[i].position, spawnPoints[i].rotation);
            e.GetComponent<EnemyScript>().RecieveSpawnInfo(this);
            numEnemies++;
        }
    }

    // public method to call for one time spawns for each type of puzzle
    public void SpawnEnemies(PuzzleTwo p)
    {
        if (hasSpawnwedAlready)
        {
            return;
        }

        puzzle2 = p;
        hasSpawnwedAlready = true;

        SpawnEnemiesAtPoint();
    }

    public void SpawnEnemies(PuzzleOne p)
    {
        puzzle1 = p;

        SpawnEnemiesAtPoint();
    }

    public void SpawnEnemies(PuzzleThree p)
    {
        puzzle3 = p;

        SpawnEnemiesAtPoint();
    }



    public void RemoveEnemy()
    {
        numEnemies--;

        if (isGeneralSpawner)
        {
            // general spawners will spawn more later
            CountEnemies();
        }
        else
        {
            // one time spawners will 'finish' and tell the puzzle that its done
            if (numEnemies > 0)
            {
                return;
            }

            if (puzzle1 != null)
            {
                puzzle1.ResetPuzzle();
            }
            else if (puzzle2 != null)
            {
                puzzle2.PuzzleComplete();
            }
            else if (puzzle3 != null)
            {
                puzzle3.CanContinuePuzzle();
            }
        }


    }
}
