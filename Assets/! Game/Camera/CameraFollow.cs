using Unity.Mathematics;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [Header("Basic Stats")]
    [SerializeField] float followPercent;
    [SerializeField] float speed;
    [SerializeField] float z = -9;
    [Header("Y Follow")]
    [SerializeField] float yDistance;
    [SerializeField] float yOffset;

    Vector2 playerPosition;
    Vector2 position;

    void FixedUpdate() {
        ClampPlayer();

        position = Vector3.Lerp(transform.position, playerPosition, followPercent);
        transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }

    void ClampPlayer() {
        playerPosition = PlayerManager.instance.GetTransform().position;

        if (!PlayerManager.instance.IsGrounded()) {
            if (math.distance(playerPosition.y, transform.position.y) > yDistance * yDistance) {
                Debug.Log("really far");
                //playerPosition.y += yOffset;
            }
            else if (math.distance(playerPosition.y, transform.position.y) > yDistance) {
                Debug.Log("far");
                //playerPosition.y += yOffset / 2;
            }
            else {
                playerPosition.y = transform.position.y;
            }
        }
        else {
            playerPosition.y += yOffset;
        }
    }

}
