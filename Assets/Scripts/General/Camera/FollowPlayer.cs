using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;

    [Tooltip("Distance to player it needs to reach for the camera to start moving")]
    [SerializeField] private float minDistance;

    [SerializeField] private float cameraMoveAcceleration;
    [SerializeField] private float cameraMaxSpeed;
    private void OnEnable()
    {
        
    }

    private void Update()
    {
        float distanceToPlayer = ( (Vector2) player.position - (Vector2) transform.position).magnitude;
        Vector2 cameraVelocity = Vector2.zero;

        if(distanceToPlayer > minDistance)
        {
            cameraVelocity = (player.position - transform.position).normalized * cameraMoveAcceleration * (distanceToPlayer - minDistance) * Time.deltaTime;
            cameraVelocity = new(Mathf.Clamp(cameraVelocity.x, -cameraMaxSpeed, cameraMaxSpeed), Mathf.Clamp(cameraVelocity.y, -cameraMaxSpeed, cameraMaxSpeed));
        }

        transform.position += (Vector3) cameraVelocity;
    }
}
