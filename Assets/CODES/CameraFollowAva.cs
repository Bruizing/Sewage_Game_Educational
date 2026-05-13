using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//finished
public class CameraFollowAva : MonoBehaviour
{
    public Transform Avalin;
    public float smoothSpeed = 4f;
    public Vector3 offset;
    public BoxCollider2D cameraZone;
    public float zoneBuffer = 0.2f;//for jitter remove

    void Update()
    {
        float minX = cameraZone.bounds.min.x;
        float maxX = cameraZone.bounds.max.x;
        // start with current camera pos   vv
        float targetX = transform.position.x;
        // Only update targetY if Avalin is outside the camera zone bounds
        if (Avalin.position.x > maxX + zoneBuffer || Avalin.position.x < minX - zoneBuffer)
        {
            targetX = Avalin.position.x;
        }
        targetX = Mathf.Clamp(targetX, minX, maxX);

        // Keep X position fixed with offset, only update Y position
        Vector3 targetPosition = new Vector3(targetX + offset.x, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);
    }
}