using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopPuzCollide : MonoBehaviour
{

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("puzBoxes"))
        {
            Destroy(transform.parent.gameObject);
        }
    }

}
