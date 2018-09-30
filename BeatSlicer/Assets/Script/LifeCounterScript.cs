using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCounterScript : MonoBehaviour {

    float lifeCount;

    public GameObject lifeObjectRef;
    GameObject[] lifeObject = new GameObject[5];
    RectTransform lifeObjectRT;
    public PlayerModelScript player;
    float playerHealth;

    void Start () {
        #region Create Multiple Lives
        lifeCount = player.maxHealth / 2;
        lifeObjectRT = lifeObjectRef.GetComponent<RectTransform>();

        for (int i = 0; i < lifeCount; i++)
        {
            GameObject newLifeObject = Instantiate(lifeObjectRef, this.transform);
            RectTransform newLifeObjectRT = newLifeObject.GetComponent<RectTransform>();
            newLifeObjectRT.anchoredPosition = new Vector2(-800 + (100 * i), -331);

            lifeObject[i] = newLifeObject;
        }
        #endregion
    }
	
	// Update is called once per frame
	void Update () {
        #region Unoptimized Player Health Check
        GameObject currentLifeGreen;
        GameObject currentLifeRed;

        if (player.health == 9)
        {
            currentLifeGreen = lifeObject[4].transform.Find("GreenGlow").gameObject;
            currentLifeRed = lifeObject[4].transform.Find("RedGlow").gameObject;

            currentLifeGreen.SetActive(false);
            currentLifeRed.SetActive(true);
        }

        else if (player.health == 7)
        {
            currentLifeGreen = lifeObject[3].transform.Find("GreenGlow").gameObject;
            currentLifeRed = lifeObject[3].transform.Find("RedGlow").gameObject;

            currentLifeGreen.SetActive(false);
            currentLifeRed.SetActive(true);
        }

        else if (player.health == 5)
        {
            currentLifeGreen = lifeObject[2].transform.Find("GreenGlow").gameObject;
            currentLifeRed = lifeObject[2].transform.Find("RedGlow").gameObject;

            currentLifeGreen.SetActive(false);
            currentLifeRed.SetActive(true);
        }

        else if (player.health == 3)
        {
            currentLifeGreen = lifeObject[1].transform.Find("GreenGlow").gameObject;
            currentLifeRed = lifeObject[1].transform.Find("RedGlow").gameObject;

            currentLifeGreen.SetActive(false);
            currentLifeRed.SetActive(true);
        }

        else if (player.health == 1)
        {
            currentLifeGreen = lifeObject[0].transform.Find("GreenGlow").gameObject;
            currentLifeRed = lifeObject[0].transform.Find("RedGlow").gameObject;

            currentLifeGreen.SetActive(false);
            currentLifeRed.SetActive(true);
        }

        if (player.health <= 8)
        {
            lifeObject[4].SetActive(false);
        }

        if (player.health <= 6)
        {
            lifeObject[3].SetActive(false);
        }
    
        if (player.health <= 4)
        {
            lifeObject[2].SetActive(false);
        }

        if (player.health <= 2)
        {
            lifeObject[1].SetActive(false);
        }      
        #endregion
    }
}
