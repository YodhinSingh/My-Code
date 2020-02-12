using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followParent : MonoBehaviour
{
    public Transform parent;
    // Start is called before the first frame update
    void Start()
    {
        //transform.SetParent(parent.transform);
    }
    private void Update()
    {
        transform.position = new Vector3(parent.position.x, parent.position.y + 0.3f, parent.position.z);
    }

}
