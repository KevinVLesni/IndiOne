using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSize : MonoBehaviour
{
    public float zoomSpeed = 1.0f;
    public float minZoom = 2.0f;
    public float maxZoom = 10.0f;

    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scrollInput);
    }

    void ZoomCamera(float zoomInput)
    {
        Camera mainCamera = Camera.main;

        // Adjust the orthographic size based on scroll input
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize - zoomInput * zoomSpeed, minZoom, maxZoom);
    }
}
