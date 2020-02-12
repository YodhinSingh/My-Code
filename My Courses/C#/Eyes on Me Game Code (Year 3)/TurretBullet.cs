using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    public bool colour1 = true; // colour1 = true means it represents Red. false means Blue. Used in puzzle 3 script

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5f);
    }

}
