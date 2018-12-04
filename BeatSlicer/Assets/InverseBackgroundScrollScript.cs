using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseBackgroundScrollScript : MonoBehaviour {

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
                        -deltaX * ScrollSpeedX * (i + 1) , minDragX, maxDragX);

            float newY = Mathf.Clamp(Layers[i].transform.position.y +
                        -deltaY * ScrollSpeedY * (i + 1) , minDragY, maxDragY);

            newPosition = new Vector3(newX, newY, Layers[i].transform.position.z);

            Layers[i].transform.position = newPosition;
            /*
            if (i == Layers.Length - 1)
            {
                newPosition = new Vector3(newX, newY, Layers[i].transform.position.z);

                Layers[i].transform.position = newPosition;
            }
            else if (Layers[Layers.Length-1].transform.position.x < maxDragX && Layers[Layers.Length-1].transform.position.x > minDragX
                  && Layers[Layers.Length - 1].transform.position.y < maxDragY && Layers[Layers.Length - 1].transform.position.y > minDragY)
            {
                newPosition = new Vector3(newX, newY, Layers[i].transform.position.z);

                Layers[i].transform.position = newPosition;
            }
            */
        }


        #region Limiters  
        /*
         FrontLayer.transform.position = new Vector3(Mathf.Lerp(0.7f, -0.7f, FrontLayerScrollSpeed * Input.mousePosition.x),
                                                    Mathf.Lerp(-0.7f,  0.7f, FrontLayerScrollSpeed * Input.mousePosition.y),
                                                    transform.position.z);
        
        FrontLayer.transform.position = new Vector3(Mathf.Clamp(FrontLayerScrollSpeed * Input.mousePosition.x, -0.7f,0.7f),
                                                    Mathf.Clamp(FrontLayerScrollSpeed * Input.mousePosition.y, -0.7f, 0.7f), 
                                                    transform.position.z);
        if (FrontLayer.transform.position.x >= 0.7f)
                {
                    FrontLayer.transform.position = new Vector3(0.7f,transform.position.y,transform.position.z);
                }

                if (FrontLayer.transform.position.x <= -0.7f)
                {
                    FrontLayer.transform.position = new Vector3(-0.7f, transform.position.y, transform.position.z);
                }

                if (FrontLayer.transform.position.y >= 0.7f)
                {
                    FrontLayer.transform.position = new Vector3(transform.position.x, 0.7f, transform.position.z);
                }

                if (FrontLayer.transform.position.y <= -0.7f)
                {
                    FrontLayer.transform.position = new Vector3(transform.position.x,- 0.7f, transform.position.z);
                }
                */


        #endregion
    }
}
