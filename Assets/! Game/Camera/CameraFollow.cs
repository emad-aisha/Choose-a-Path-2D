using Unity.Mathematics;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [Header("Basic Stats")]
    [SerializeField] float followPercent;
    [SerializeField] float z = -9;

    [Header("X Stats")]
    [SerializeField] float xOffset;
    [SerializeField] float xSpeed;

    [Header("Y Stats")]
    [SerializeField] float yOffset;
    [SerializeField] float ySpeed;
    [SerializeField] float lookOffset;

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
        if (VerticalFacingDirectionManager.instance.GetDirection() != 0) {
            Vector2 modifiedPosition = playerPosition;
            modifiedPosition.y += lookOffset * VerticalFacingDirectionManager.instance.GetDirection();
            transform.position = Vector3.Lerp(transform.position, modifiedPosition, followPercent * xSpeed * Time.deltaTime);
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }

    void ClampHorizontal() {
        if (math.distance(PlayerManager.instance.GetTransform().position.x, rightBound.position.x) > xDistance
        && math.distance(PlayerManager.instance.GetTransform().position.x, leftBound.position.x) > xDistance) {
            playerPosition.x = PlayerManager.instance.GetTransform().position.x + (xOffset * HorizontalFacingDirectionManager.instance.GetDirection());
        }
        else { }
    }

    void ClampVertical() {
        if (math.distance(PlayerManager.instance.GetTransform().position.y, upperBound.position.y) > yDistance
        && math.distance(PlayerManager.instance.GetTransform().position.y, lowerBound.position.y) > yDistance) {
            playerPosition.y = PlayerManager.instance.GetTransform().position.y;
            playerPosition.y += yOffset;
        }
        else { }
    }

}
