﻿using UnityEngine;
using System.Collections;

public class CameraEvent : MonoBehaviour {
    public float left, right, lower, upper;
    public float zoom;
    void OnTriggerEnter2D(Collider2D col)
    {
        Camera.main.GetComponent<CameraController>().Camera_Offset_Change(left, right, lower, upper);
        if (zoom != 0)
            Camera.main.GetComponent<CameraController>().Camera_Size_Change(zoom);
            
    }



   

    
}
