using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    // camera settings
    public float CameraMoveSpeed = 120f;

    private Transform CameraFollowObj;
    private Vector3 offset;

    // setup up remaining variables for each camera to follow different players
    public void SetUp(Vector3 offset, Transform target, int index, bool islvlThree)
    {
        CameraFollowObj = target;
        transform.position = CameraFollowObj.position;

        // modify camera offset
        this.offset = offset;
        Vector3 temp = new Vector3(offset.x - 1f, offset.y, offset.z);
        GetComponentInChildren<Camera>().transform.localPosition = temp;

        // only render content relevent to this player
        int layerMask = index == 0 ? ~LayerMask.GetMask("P2Cam") : ~LayerMask.GetMask("P1Cam");
        GetComponentInChildren<Camera>().cullingMask = layerMask;

        GetComponentInChildren<CameraCollision>().ChangeLevelInfo(islvlThree);

    }

    // better for camera physics so no stutter
    void LateUpdate()
    {
        CameraUpdater();
    }

    void CameraUpdater()
    {
        if (CameraFollowObj == null)
        {
            return;
        }
        
        // follow player based on time and computer processing speed
        Vector3 target = CameraFollowObj.position;

        float step = CameraMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);
    }





}
