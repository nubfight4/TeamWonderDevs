using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public float CameraMoveSpeed = 120.0f;
	
	Vector3 FollowPOS;
	public float clampAngle = 80.0f;
    public float inputSensitivity = 150.0f;
	public float camDistanceXToPlayer;
	public float camDistanceYToPlayer;
	public float camDistanceZToPlayer;
	public float mouseX;
	public float mouseY;
	public float finalInputX;
	public float finalInputZ;
	public float smoothX;
	public float smoothY;
	private float rotY = 0.0f;
	private float rotX = 0.0f;

    public GameObject CameraFollowObj;
    public GameObject CameraObj;
    public GameObject PlayerObj;

    public Transform xPivot;
    public Transform yPivot; // Set this for vertical rotation

    // Use this for initialization
    void Start ()
    {
		Vector3 rot = transform.localRotation.eulerAngles;
		rotY = rot.y;
		rotX = rot.x;

        xPivot.transform.position = PlayerObj.transform.position;
        xPivot.transform.parent = null;
        yPivot.transform.position = CameraObj.transform.position;
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

        xPivot.Rotate(0f, finalInputX * inputSensitivity * Time.deltaTime, 0f);

        rotX = Mathf.Clamp (rotX, -clampAngle, clampAngle);

		Quaternion localRotation = Quaternion.Euler (rotX, rotY, 0.0f);
		transform.rotation = localRotation;
	}

	void LateUpdate ()
    {
		CameraUpdater ();

        xPivot.transform.position = PlayerObj.transform.position;
        yPivot.transform.position = CameraObj.transform.position;

    }

    void CameraUpdater()
    {
		Transform target = CameraFollowObj.transform;

		//move towards the target
		float step = CameraMoveSpeed * Time.deltaTime;
		transform.position = Vector3.MoveTowards (transform.position, target.position, step);
	}   
}
