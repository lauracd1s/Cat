using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.1f;
    public float offsetY = 1.5f;

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 desiredPosition = new Vector3(
            player.position.x,
            player.position.y + offsetY,
            transform.position.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed
        );
    }
}
