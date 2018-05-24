using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform followTarget;
    public Transform Pivot;
    public Vector3 cameraOffset;

    [Range(0.01f, 1.0f)]
    public float cameraSpeed = 0.5f;
    public float rotationSpeed;
    public float maxViewAngle;
    public float minViewAngle;

    public bool useOffsetvalue = true;
    //public bool lookAtTarget = false;
    //public bool rotateAroundTarget = true;

    private void Start()
    {
        if(!useOffsetvalue)
        {
            cameraOffset = followTarget.position - transform.position;
        }

        Pivot.transform.position = followTarget.transform.position;
        Pivot.transform.parent = followTarget.transform;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        // Get mouse X position & rotate the target
        float horizontal = Input.GetAxis("Mouse X") * rotationSpeed;
        followTarget.Rotate(0f, horizontal, 0f);

        // Get mouse Y position & rotate the pivot
        float vertical = Input.GetAxis("Mouse Y") * rotationSpeed;
        Pivot.Rotate(-vertical, 0f, 0f);

        //Limit Camera Up/ Down rotation
        if(Pivot.rotation.eulerAngles.x > maxViewAngle && Pivot.rotation.eulerAngles.x < 180)
        {
            Pivot.rotation = Quaternion.Euler(maxViewAngle, 0f, 0f);
        }

        if (Pivot.rotation.eulerAngles.x > 180f && Pivot.rotation.eulerAngles.x < 360f + minViewAngle)
        {
            Pivot.rotation = Quaternion.Euler(360f + minViewAngle, 0f, 0f);
        }

        // Move the camera based on current rotation & original offset
        float desiredAngleY = followTarget.eulerAngles.y;
        float desiredAngleX = Pivot.eulerAngles.x;
        Quaternion rotation = Quaternion.Euler(desiredAngleX, desiredAngleY, 0f);
        transform.position = followTarget.position - (rotation * cameraOffset);

        if(transform.position.y < followTarget.position.y)
        {
            transform.position = new Vector3(transform.position.x, followTarget.position.y - 0.5f, transform.position.z);
        }


        transform.LookAt(followTarget);
    }
}
