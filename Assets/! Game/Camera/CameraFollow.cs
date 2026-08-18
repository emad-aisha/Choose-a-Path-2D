using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [Header("Basic Stats")]
    [SerializeField] float followPercent;
    [SerializeField] float z = -9;

    [Header("Bounds")]
    [SerializeField] BoxCollider2D bounds;
    [SerializeField] float xDistance;
    [SerializeField] float yDistance;

    [Header("X Stats")]
    [SerializeField] float xOffset;
    [SerializeField] float xSpeed;

    [Header("Y Stats")]
    [SerializeField] float yOffset;
    [SerializeField] float ySpeed;
    [SerializeField] float lookOffset;
    [SerializeField] float requiredTimeFalling;

    Vector2 playerPosition;
    Vector2 position;


    void Start() { playerPosition = PlayerManager.instance.GetTransform().position; }

    void FixedUpdate() {
        Clamp();
        LerpToNewPosition();

        transform.position = position;
        LookCheck();
        FallCheck();
    }


    // HELPERS
    void Clamp() {
        // update player position lmao
        playerPosition = PlayerManager.instance.GetTransform().position;

        playerPosition.x = Mathf.Clamp(playerPosition.x, bounds.bounds.min.x + xDistance, bounds.bounds.max.x - xDistance);
        playerPosition.y = Mathf.Clamp(playerPosition.y, bounds.bounds.min.y + yDistance + yOffset, bounds.bounds.max.y - yDistance + yOffset);
    }

    void LerpToNewPosition() {
        position = Vector3.Lerp(transform.position, playerPosition, followPercent);
        float x = Vector3.Lerp(transform.position, position, xSpeed * Time.deltaTime).x;
        float y = Vector3.Lerp(transform.position, position, ySpeed * Time.deltaTime).y;
        position = new Vector3(x, y);
    }

    void LookCheck() {
        if (VerticalFacingDirectionManager.instance.GetDirection() != 0) {
            Vector2 modifiedPosition = playerPosition;
            modifiedPosition.y += lookOffset * VerticalFacingDirectionManager.instance.GetDirection();
            transform.position = Vector3.Lerp(transform.position, modifiedPosition, followPercent * ySpeed * Time.deltaTime);
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }

    void FallCheck() {
        // if is falling
        if (MovementManager.instance.IsFalling(requiredTimeFalling)) {
            Vector2 modifiedPosition = playerPosition;
            modifiedPosition.y -= 3; // look down
            transform.position = Vector3.Lerp(transform.position, modifiedPosition, followPercent * (ySpeed / 2) * Time.deltaTime);
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }
}
