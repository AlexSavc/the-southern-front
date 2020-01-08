using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanZoom : MonoBehaviour
{
    Vector3 touchStart;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private float minZoom = 1;
    [SerializeField]
    private float maxZoom = 8;

    public bool limit = false;

    void Update()
    {
        if (Utility.IsPonterOverUIObject()) return;

        if (Input.GetMouseButtonDown(0))
        {
            if(limit)
            {
                if(Input.GetKey(KeyCode.Space))
                {
                    touchStart = cam.ScreenToWorldPoint(Input.mousePosition);
                }
            }
            else
            {
                touchStart = cam.ScreenToWorldPoint(Input.mousePosition);
            }
        }

        if(Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;
            
            //Do the 2-touch panning

            Zoom(difference * 0.01f);
        }



        else if(Input.GetMouseButton(0))
        {
            if (limit)
            { 
                // so you can move the canvas with mouse while painting, needs mapEditor to acount for it
                //(disable painting while Space is pressed)
                if (Input.GetKey(KeyCode.Space))
                {
                    Vector3 direction = touchStart - cam.ScreenToWorldPoint(Input.mousePosition);
                    cam.transform.position += direction;
                }
            }
            else
            {
                Vector3 direction = touchStart - cam.ScreenToWorldPoint(Input.mousePosition);
                cam.transform.position += direction;
            }
        }
    }

    void Zoom(float increment)
    {
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - increment, minZoom, maxZoom);

    }

    public void SetLimit(bool toLimit)
    {
        limit = toLimit;
    }
}
