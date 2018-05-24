using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour 
{
	public GameObject player;

	public float bulletSpeed = 10.0f;
	public float selfDestructTimer = 5.0f;

	private Rigidbody bulletRigidbody;
	private SphereCollider bulletSphereCollider; //  For Future Use, Probably :)


	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		bulletRigidbody = GetComponent<Rigidbody>();
		bulletSphereCollider = GetComponent<SphereCollider>();
	}
		

	void Start() 
	{
		bulletRigidbody.velocity = transform.forward * bulletSpeed;
	}


	void Update() 
	{
		transform.LookAt(player.transform.position);
		selfDestructFunction();
	}


	void selfDestructFunction()
	{
		selfDestructTimer -= Time.deltaTime;

		if(selfDestructTimer <= 0)
		{
			Destroy(this.gameObject); 
		}
	}
		

	void OnTriggerEnter(Collider other)
	{
		if(this.bulletSphereCollider.enabled == true)
		{
			if(other.tag == "Player")
			{
				Destroy(gameObject);
			}
		}
	}
}
