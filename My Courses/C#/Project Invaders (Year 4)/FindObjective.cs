using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FindObjective : MonoBehaviour
{
    // references to player
    private Transform playerTarget;
    private Transform bossTarget;
    public GameObject playerArrow;
    public GameObject bossArrow;
    private Vector3 pointToPlayer;
    private Vector3 pointToBoss;

    public Material[] colours;

    private bool isSensing = false;
    private bool isLvlThree;

    private void Update()
    {
        // if player turned on sense power
        if (isSensing)
        {
            if (bossTarget != null)
            {
                // point a UI arrow to the direction of the boss
                pointToBoss = bossTarget.position - transform.position;
                float angle = Mathf.Atan2(pointToBoss.x, pointToBoss.z) * Mathf.Rad2Deg;

                bossArrow.transform.rotation = Quaternion.Euler(0, angle, 0);
            }
            if (playerTarget != null)
            {
                // point a UI arrow to the direction of the other player
                pointToPlayer = playerTarget.position - transform.position;
                float angle = Mathf.Atan2(pointToPlayer.x, pointToPlayer.z) * Mathf.Rad2Deg;
                playerArrow.transform.rotation = Quaternion.Euler(0, angle, 0);
            }
        }
    }

    private void OnSixSense() // get input from unity input system
    {
        // on car chase levels it is always on
        if (!isLvlThree)
        {
            // turn off if was already on
            if (isSensing)
            {
                playerArrow.SetActive(false);
                bossArrow.SetActive(false);
                CancelInvoke();
                isSensing = false;
                return;
            }
            // otherwise turn on arrows
            isSensing = true;
            Invoke("TurnOffSense", 5);
            playerArrow.SetActive(true);
            bossArrow.SetActive(true);
        }
    }

    private void TurnOffSense()
    {
        isSensing = false;
        playerArrow.SetActive(false);
        bossArrow.SetActive(false);
        CancelInvoke();
    }

    // get references to players and boss
    public void SetUpReferences(Transform player, Transform boss, int indexOfPlayer, bool isLevelThree)
    {
        playerTarget = player;
        bossTarget = boss;
        int layer = LayerMask.NameToLayer("P" + indexOfPlayer + "Cam");
        playerArrow.GetComponentInChildren<MeshRenderer>().gameObject.layer = layer;
        bossArrow.GetComponentInChildren<MeshRenderer>().gameObject.layer = layer;

        if (indexOfPlayer == 1)
        {
            playerArrow.GetComponentInChildren<MeshRenderer>().material = colours[1]; // give p1, an arrow pointing to p2 with p2's colour
        }
        else if (indexOfPlayer == 2)
        {
            playerArrow.GetComponentInChildren<MeshRenderer>().material = colours[0]; // give p2, an arrow pointing to p1 with p1's colour
        }

        if (isLevelThree)
        {
            // level 3 (car) has them modified a bit, and are always on
            playerArrow.transform.localPosition = new Vector3(0, 4.08f, 3.92f);
            bossArrow.transform.localPosition = new Vector3(0, 4.08f, 3.92f);

            isSensing = true;
            playerArrow.SetActive(true);
            bossArrow.SetActive(true);
            isLvlThree = true;
        }
        else
        {
            // other levels must have players turn them on
            playerArrow.transform.localPosition = Vector3.zero;
            bossArrow.transform.localPosition = Vector3.zero;
            isLvlThree = false;
            isSensing = false;
            playerArrow.SetActive(false);
            bossArrow.SetActive(false);
        }
    }
}
