using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    public GameObject target;   // player

    Vector3 initPos;

    // Start is called before the first frame update
    void Start()
    {
        initPos = target.transform.position;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))        // when it collides with player, send player back to where they started
        {
            col.gameObject.GetComponent<Transform>().position = initPos;
        }
    }
}
