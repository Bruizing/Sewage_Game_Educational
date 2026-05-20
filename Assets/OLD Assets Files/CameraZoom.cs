using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float zoomedFOV = 30f; // Field of View when zoomed in.
    public float normalFOV = 60f; // Normal Field of View.
    public float zoomSpeed = 5f; // Speed of zoom transition.

    public Camera cam;

    void Start()
    {
        if (cam == null)
        {
            Debug.LogError("CameraZoom script is not attached to a camera.");
        }
    }

    void Update()
    {
        if (cam != null)
        {
            if (Input.GetMouseButton(0)) // Left mouse button is held down.
            {
                // Smoothly transition to the zoomed FOV.
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoomedFOV, zoomSpeed * Time.deltaTime);
            }
            else
            {
                // Smoothly transition back to the normal FOV.
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, normalFOV, zoomSpeed * Time.deltaTime);
            }
        }
    }
}