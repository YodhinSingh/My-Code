using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeIndicator : MonoBehaviour
{
    public GameObject repairRayPrefab;
    private float attackCountdown = 0f;
    public float attackRate;
    private bool inRange = false;
    private Transform currentTarget = null;
    public GameObject emitter;

    private void Update()
    {
        //check if conditions are right for a drone to attack the target such as being discovered by player, etc
        if (inRange)
        {
            if (attackCountdown <= 0f && GetComponentInParent<RunningScript>().isDiscovered && currentTarget != null && currentTarget.gameObject.GetComponent<RestoreHealth>().getHealth() < 100)
            {
                if (GetComponentInParent<RunningScript>().player != null && GetComponentInParent<RunningScript>().player.gameObject.GetComponent<RunningScript>().isControlsActive())
                {
                    Repair(currentTarget);
                    attackCountdown = 1f / attackRate;
                }
            }
            attackCountdown -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PcPart")  // targets have the tag PcPart
        {
            currentTarget = other.transform;
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "PcPart")
        {
            inRange = false;
        }
    }

    void Repair(Transform other)
    {
        GameObject bulletGO = (GameObject)Instantiate(repairRayPrefab, emitter.transform.position, transform.rotation);
        RepairRay bullet = bulletGO.GetComponent<RepairRay>();

        if (bullet != null)
        {
            bullet.Seek(other);
            bullet.GetComponent<AudioSource>().PlayOneShot(bullet.GetComponent<AudioSource>().clip);
        }
    }
}
