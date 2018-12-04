using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrollScript : MonoBehaviour {

    public GameObject[] Layers;

    float mousePosXStart;
    public float minDragX;
    public float maxDragX;

    float mousePosYStart;
    public float minDragY;
    public float maxDragY;

    public float ScrollSpeedX;
    public float ScrollSpeedY;

    private void Start()
    {
        mousePosXStart = Input.mousePosition.x;
        mousePosYStart = Input.mousePosition.y;
    }


    // Update is called once per frame
    void FixedUpdate () {
        Vector3 newPosition;
        float deltaX = Input.mousePosition.x - mousePosXStart;
        float deltaY = Input.mousePosition.y - mousePosYStart;

        for (int i=0;i<Layers.Length;i++)
        {
            float newX = Mathf.Clamp(Layers[i].transform.position.x +
                        deltaX * ScrollSpeedX * (i + 1) , minDragX, maxDragX);

            float newY = Mathf.Clamp(Layers[i].transform.position.y +
                        deltaY * ScrollSpeedY * (i + 1) , minDragY, maxDragY);

           newPosition = new Vector3(newX, newY, Layers[i].transform.position.z);

           Layers[i].transform.position = newPosition;
        }
    }
}
