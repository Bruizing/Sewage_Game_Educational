using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//finished
public class CameraFollowAva : MonoBehaviour
{
    public Transform avalin;
    public BoxCollider2D cameraZone;
    public float smoothSpeed = 5f;
    public float zoneBuffer = 0.2f;
    public Vector3 offset;

    void LateUpdate()
    {
        if (avalin == null || cameraZone == null) return;

        Bounds bounds = cameraZone.bounds;

        float targetX = transform.position.x;
        float targetY = transform.position.y;

        if (avalin.position.x > bounds.max.x + zoneBuffer ||
            avalin.position.x < bounds.min.x - zoneBuffer)
        {
            targetX = avalin.position.x;
        }

        if (avalin.position.y > bounds.max.y + zoneBuffer ||
            avalin.position.y < bounds.min.y - zoneBuffer)
        {
            targetY = avalin.position.y;
        }

        Vector3 targetPosition = new Vector3(
            targetX + offset.x,
            targetY + offset.y,
            transform.position.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}