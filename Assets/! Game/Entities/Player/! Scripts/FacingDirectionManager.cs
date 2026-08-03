using UnityEngine;
using UnityEngine.InputSystem;

public class FacingDirectionManager : Input {
    public static FacingDirectionManager instance;
    InputAction moveAction;
    [SerializeField] float offset;

    void Start() {
        if (instance == null) instance = this;

        moveAction = InputManager.instance.GetAction(actionName, "Move");
        moveAction.performed += MoveVisualizer;
    }
    void OnDisable() {
        moveAction.performed -= MoveVisualizer;
    }

    float xDirection = 1;

    void LateUpdate() {
        transform.position = PlayerManager.instance.GetTransform().position;
        transform.position += new Vector3(offset * xDirection, 0);
    }

    void MoveVisualizer(InputAction.CallbackContext context) {
        xDirection = context.ReadValue<Vector2>().x;
        xDirection = Mathf.RoundToInt(xDirection);
    }

    public float GetDirection() { return xDirection; }
}
