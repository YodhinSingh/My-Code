using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    // settings of camera
    public float minDistance = 1f;
    public float maxDistance = 4f;
    public float smoothTime = 10f;
    public LayerMask hitLayers;
    Vector3 dollyDir;
    float distance;

    // Start is called before the first frame update
    void Awake()
    {
        //initialize variables
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        /* modify camera position based on player status
         * this only handles position of camera for collisions at a specified location (i.e. closer or further at point X),
         * not the actual movement of following player (refer to CameraScript)
        */
        Vector3 desiredCameraPos = transform.parent.TransformPoint(dollyDir * maxDistance);
        RaycastHit hit;

        // if object in the way of camera, move camera closer to player (based on layers allowed and no triggers)
        if (Physics.Linecast(transform.parent.position, desiredCameraPos, out hit, hitLayers) && !hit.collider.isTrigger)
        {
            distance = Mathf.Clamp((hit.distance * 0.9f), minDistance, maxDistance);
        }
        else
        {
            distance = maxDistance;
        }
        // update camera position to finalized location
        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * smoothTime);
    }

    // modifiy camera preset distances based on lavel requirements (player vs car camera)
    public void ChangeLevelInfo(bool isLevelThree)
    {
        if (isLevelThree)
        {
            minDistance = 10;
            maxDistance = 20;
        }
        else
        {
            minDistance = 4;
            maxDistance = 12;
        }
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }
}
