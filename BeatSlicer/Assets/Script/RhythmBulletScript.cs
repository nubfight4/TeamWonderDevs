using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmBulletScript : MonoBehaviour {

    public Transform PlayerModel;
    public GameObject RhythmBar;
	
	// Update is called once per frame
	void Update () {
        float distance = Vector3.Distance(PlayerModel.transform.position, transform.position);

        if (distance < 2.5f)
        {
            RhythmBar.SetActive(true);
        }
        else
        {
            RhythmBar.SetActive(false);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Sword")
        {
            Debug.Log("sad");
            gameObject.SetActive(false);
        }
    }
}
