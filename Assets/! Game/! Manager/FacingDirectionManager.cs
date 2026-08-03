using UnityEngine;
using UnityEngine.InputSystem;

public class FacingDirectionManager : Input {
    public static FacingDirectionManager instance;
    InputAction moveAction;
    [SerializeField] float offset;

    public delegate void ChangeDirectionEvent();
    public event ChangeDirectionEvent ChangeDirection;


    void Start() {
        if (instance == null) instance = this;

        moveAction = InputManager.instance.GetAction(actionName, "Move");
        moveAction.performed += MoveVisualizer;
    }
    void OnDisable() {
        moveAction.performed -= MoveVisualizer;
        ChangeDirection = null;
    }

    float xDirection = 1;

    void LateUpdate() {
        transform.position = PlayerManager.instance.GetTransform().position;
        transform.position += new Vector3(offset * xDirection, 0);
    }

    void MoveVisualizer(InputAction.CallbackContext context) {
        float newDirection = context.ReadValue<Vector2>().x;
        if (newDirection != xDirection) ChangeDirection.Invoke();
        xDirection = Mathf.RoundToInt(newDirection);
    }

    public float GetDirection() { return xDirection; }
}
