using UnityEngine;
using UnityEngine.InputSystem;

public class VerticalFacingDirectionManager : Input {
    public static VerticalFacingDirectionManager instance;
    InputAction moveAction;
    [SerializeField] float offset;
    [SerializeField] float lookTimer;
    float internalTimer = 0;
    bool holdingButton = false;

    bool lookingUp;
    public delegate void ChangeDirectionEvent();
    public event ChangeDirectionEvent LookDirection;
    public event ChangeDirectionEvent StopLookDirection;

    float yDirection = 1;

    void Awake() {
        if (instance == null) instance = this;

        moveAction = InputManager.instance.GetAction(actionName, "Move");
    }

    void OnDisable() {
        LookDirection = null;
        StopLookDirection = null;
    }

    void Update() {
        if (holdingButton) { internalTimer += Time.deltaTime; }
        else { internalTimer = 0; }

        if (internalTimer > lookTimer) {
            lookingUp = true;
            internalTimer = 0;
        }
    }


    // EVENTS ---
    void LateUpdate() {
        StartLooking();
        StopLooking();

        transform.position = PlayerManager.instance.GetTransform().position;
        transform.position += new Vector3(0, offset * yDirection);
    }

    // FACING --- 
    void StartLooking() {
        float newDirection = moveAction.ReadValue<Vector2>().y;

        if (newDirection == 0) {
            StopLookDirection?.Invoke();
            holdingButton = false;
            lookingUp = false;
            return;
        }

        yDirection = Mathf.RoundToInt(newDirection);
        LookDirection?.Invoke();
    }
    void StopLooking() {
        if (moveAction.ReadValue<Vector2>().x == 0) {
            holdingButton = true;
        }
        else {
            holdingButton = false;
            lookingUp = false;
        }
    }


    // GETTERS --- 
    public float GetDirection() {
        if (lookingUp) return yDirection;
        else return 0;
    }
    public float GetPureDirection() {
        return yDirection;
    }

}
