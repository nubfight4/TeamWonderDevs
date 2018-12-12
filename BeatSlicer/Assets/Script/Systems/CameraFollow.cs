using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public float CameraMoveSpeed;
	
	Vector3 FollowPOS;
	public float minClampAngle;
    public float maxClampAngle;
    public float inputSensitivity;
	public float mouseX;
	public float mouseY;
	public float finalInputX;
	public float finalInputZ;
	private float rotY = 0.0f;
	private float rotX = 0.0f;

    public GameObject CameraFollowObj;
    public GameObject CameraObj;
    public GameObject PlayerObj;

    public Transform Pivot;

    // Use this for initialization
    void Start ()
    {
		Vector3 rot = transform.localRotation.eulerAngles;
		rotY = rot.y;
		rotX = rot.x;

        Pivot.transform.position = PlayerObj.transform.position;
        Pivot.transform.parent = null;
    }
	
	// Update is called once per frame
	void Update ()
    {
		mouseX = Input.GetAxis ("Mouse X");
		mouseY = Input.GetAxis ("Mouse Y");
		finalInputX = mouseX;
		finalInputZ = -mouseY;

        float horizontal = mouseX * inputSensitivity * Time.deltaTime;
        PlayerObj.transform.Rotate(0f, horizontal, 0f);

        rotY += finalInputX * inputSensitivity * Time.deltaTime;
		rotX += finalInputZ * inputSensitivity * Time.deltaTime;

        //xPivot.Rotate(0f, finalInputX * inputSensitivity * Time.deltaTime, 0f);

        rotX = Mathf.Clamp (rotX, -minClampAngle, maxClampAngle);

		Quaternion localRotation = Quaternion.Euler (0.0f, rotY, 0.0f);
		transform.rotation = localRotation;
        CameraObj.transform.rotation = Quaternion.Euler(rotX, rotY, 0.0f);
	}

	void LateUpdate ()
    {
		CameraUpdater ();
        Pivot.transform.position = PlayerObj.transform.position;
    }

    void CameraUpdater()
    {
		Transform target = CameraFollowObj.transform;

		//move towards the target
		float step = CameraMoveSpeed * Time.deltaTime;
        Vector3 selfPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 targetPos = new Vector3(target.position.x, target.position.y + 3.0f, target.position.z);
        transform.position = Vector3.MoveTowards (selfPos, targetPos, step);
	}   
}
