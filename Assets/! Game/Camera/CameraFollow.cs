using Unity.Mathematics;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [Header("Basic Stats")]
    [SerializeField] float followPercent;
    [SerializeField] float z = -9;
    [SerializeField] float yOffset;

    [Header("Speed")]
    [SerializeField] float ySpeed;
    [SerializeField] float xSpeed;

    [Header("Bounds")]
    [SerializeField] float xDistance;
    [SerializeField] float yDistance;
    [SerializeField] Transform upperBound;
    [SerializeField] Transform lowerBound;
    [SerializeField] Transform rightBound;
    [SerializeField] Transform leftBound;

    Vector2 playerPosition;
    Vector2 position;

    void Start() { playerPosition = PlayerManager.instance.GetTransform().position; }

    void FixedUpdate() {
        ClampHorizontal();
        ClampVertical();

        position = Vector3.Lerp(transform.position, playerPosition, followPercent);
        float x = Vector3.Lerp(transform.position, position, xSpeed * Time.deltaTime).x;
        float y = Vector3.Lerp(transform.position, position, ySpeed * Time.deltaTime).y;
        position = new Vector3(x, y);

        transform.position = position;
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }

    void ClampHorizontal() {
        if (math.distance(PlayerManager.instance.GetTransform().position.x, rightBound.position.x) > xDistance
        && math.distance(PlayerManager.instance.GetTransform().position.x, leftBound.position.x) > xDistance) {
            playerPosition.x = PlayerManager.instance.GetTransform().position.x;
        }
        else if (math.distance(PlayerManager.instance.GetTransform().position.x, rightBound.position.x) < xDistance) { }
        else if (math.distance(PlayerManager.instance.GetTransform().position.x, leftBound.position.x) < xDistance) { }
    }

    void ClampVertical() {
        if (math.distance(PlayerManager.instance.GetTransform().position.y, upperBound.position.y) > yDistance
        && math.distance(PlayerManager.instance.GetTransform().position.y, lowerBound.position.y) > yDistance) {
            playerPosition.y = PlayerManager.instance.GetTransform().position.y;
            playerPosition.y += yOffset;
        }
        else if (math.distance(PlayerManager.instance.GetTransform().position.y, upperBound.position.y) < yDistance) { }
        else if (math.distance(PlayerManager.instance.GetTransform().position.y, lowerBound.position.y) < yDistance) { }
    }

}
