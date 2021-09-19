using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySightRange : MonoBehaviour
{
    // references to other players
    private bool foundPlayerOne;
    private bool foundPlayerTwo;
    private EnemyScript enemyBody;

    private Vector3 originalSightRange;

    private void Awake()
    {
        originalSightRange = GetComponent<BoxCollider>().size;
    }

    private void Start()
    {
        foundPlayerOne = false;
        foundPlayerTwo = false;
        enemyBody = GetComponentInParent<EnemyScript>();

    }

    // increase range of sight for a set amount of time
    public void IncreaseRangeTemporarily(int size, int time)
    {
        if (originalSightRange == Vector3.zero)
        {
            originalSightRange = GetComponent<BoxCollider>().size;
        }
        GetComponent<BoxCollider>().size = originalSightRange * size;

        if (IsInvoking())
        {
            CancelInvoke();
        }
        Invoke("ResetRange", time);
    }

    // make sight range back to normal
    private void ResetRange()
    {
        GetComponent<BoxCollider>().size = originalSightRange;
        foundPlayerOne = false;
        foundPlayerTwo = false;
        enemyBody.IsPlayerInRange(false, null);
    }

    private void OnTriggerEnter(Collider other)
    {
        IsPlayerFound(other, true);
    }

    private void OnTriggerStay(Collider other)
    {
        IsPlayerFound(other, true);
    }

    private void OnTriggerExit(Collider other)
    {
        IsPlayerFound(other, false);
    }

    private void IsPlayerFound(Collider other, bool isTrue)
    {
        if (enemyBody == null)
        {
            return;
        }
        // see which player is close to this enemy
        if (((isTrue && !foundPlayerTwo) || !isTrue) && (other.tag == "PlayerOne") || other.tag == "PlayerCarOne")
        {
            foundPlayerOne = isTrue;
            enemyBody.IsPlayerInRange(isTrue, other.transform);
        }
        if (((isTrue && !foundPlayerOne) || !isTrue) && (other.tag == "PlayerTwo") || other.tag == "PlayerCarTwo")
        {
            foundPlayerTwo = isTrue;
            enemyBody.IsPlayerInRange(isTrue, other.transform);
        }
    }
}
