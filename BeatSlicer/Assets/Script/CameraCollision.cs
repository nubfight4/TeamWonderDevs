using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour {

	public float minDistance = 1.0f;
	public float maxDistance = 4.0f;
	public float smooth = 10.0f;
	Vector3 dollyDir;
	public Vector3 dollyDirAdjusted;
	public float distance;
    public Material m_TransMat;

    float raySphereThickness = 2f;
    GameObject player;
    Vector3 playerPos;
    private LayerMask wallMask = 16;

	// Use this for initialization
	void Awake ()
    {
		dollyDir = transform.localPosition.normalized;
		distance = transform.localPosition.magnitude;

        distance = maxDistance;
        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);
        transform.localPosition = dollyDir * distance;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update ()
    {
		//Vector3 desiredCameraPos = transform.parent.TransformPoint (dollyDir * maxDistance);

        playerPos = player.transform.position;

        Vector3 origin = playerPos;
        Vector3 direction = transform.TransformDirection(Vector3.back);

        //new code
        RaycastHit[] hits;

        hits = Physics.SphereCastAll(origin, raySphereThickness, direction, wallMask.value);
        foreach (RaycastHit hit in hits) 
        {
            if (hit.collider.tag == ("Wall"))
            {
                MeshRenderer R = hit.collider.GetComponent<MeshRenderer>();
                if (R == null)
                    continue; // no renderer attached? go to next hit

                AutoTransparent AT = R.GetComponent<AutoTransparent>();
                if (AT == null) // if no script is attached, attach one
                {
                    AT = R.gameObject.AddComponent<AutoTransparent>();
                }
                AT.BeTransparent(m_TransMat); // get called every frame to reset the falloff}
            }
        }
    }
}
