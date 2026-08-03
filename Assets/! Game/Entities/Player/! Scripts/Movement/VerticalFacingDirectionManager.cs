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
    //public delegate void ChangeDirectionEvent();
    //public event ChangeDirectionEvent LookDirection;
    //public event ChangeDirectionEvent StopLookDirection;

    float yDirection = 1;

    void Start() {
        if (instance == null) instance = this;

        moveAction = InputManager.instance.GetAction(actionName, "Move");
        moveAction.performed += MoveVisualizer;
        moveAction.canceled += StopLooking;
    }

    void OnDisable() {
        moveAction.performed -= MoveVisualizer;
        moveAction.canceled -= StopLooking;
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
        transform.position = PlayerManager.instance.GetTransform().position;
        transform.position += new Vector3(0, offset * yDirection);
    }

    void MoveVisualizer(InputAction.CallbackContext context) {
        float newDirection = context.ReadValue<Vector2>().y;
        if (newDirection != 0) holdingButton = true;
        yDirection = Mathf.RoundToInt(newDirection);
    }

    void StopLooking(InputAction.CallbackContext context) {
        holdingButton = false;
        lookingUp = false;
    }

    public float GetDirection() {
        if (lookingUp) return yDirection;
        else return 0;
    }
}
