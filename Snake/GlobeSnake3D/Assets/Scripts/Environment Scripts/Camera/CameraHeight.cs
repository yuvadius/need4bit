﻿using UnityEngine;
using System.Collections;

public class CameraHeight : MonoBehaviour
{

    public Transform pivotPoint;
    public Flying flyHeight;
    float height;
    float heightAtGlobeLevel;
    float heightAtMaxLevel;

    public void myUpdate()
    {
        
        float currentHeight = flyHeight.get_current_height();
        float maxHeight = flyHeight.get_max_height();

        float cameraHeight = currentHeight * (heightAtMaxLevel - heightAtGlobeLevel) / maxHeight + heightAtGlobeLevel;

        set_camera_height(cameraHeight);
    }

    void set_camera_height(float cameraHeight)
    {
        height = cameraHeight;
        Vector3 heightVector = (transform.position - pivotPoint.position).normalized * height;
        Vector3 newPosition = pivotPoint.position + heightVector;
        transform.position = newPosition;
    }

    public void set_height_at_globe_level(float _height)
    {
        heightAtGlobeLevel = _height;
    }
    public void set_height_at_max_level(float _height)
    {
        heightAtMaxLevel = _height;
    }
}