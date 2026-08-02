using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : Input {
    [Header("Walk Stats")]
    [SerializeField] float walkSpeed;
    InputAction moveAction;
    Vector3 moveDirection;

    [Header("Jump Stats")]
    [SerializeField] float jumpSpeed;
    [SerializeField] float holdJumpTime;
    [SerializeField] float gravity;


    bool ignore;
    void OnDisable() {
        moveAction.performed -= Move;
        moveAction.canceled -= StopMoving;
    }

    void Start() {
        SetMoveAction();

        moveDirection = Vector2.zero;
        GetComponent<Rigidbody2D>().gravityScale = gravity;
    }

    void Update() {
        transform.position += moveDirection * Time.deltaTime;
    }


    // EVENTS
    void Move(InputAction.CallbackContext context) {
        moveDirection = context.ReadValue<Vector2>();
        moveDirection.x *= walkSpeed;
        moveDirection.y *= jumpSpeed;
    }

    void StopMoving(InputAction.CallbackContext context) {
        moveDirection = Vector3.zero;
    }

    // SETTERS
    void SetMoveAction() {
        moveAction = InputManager.instance.GetAction(actionName, "Move");

        moveAction.performed += Move;
        moveAction.canceled += StopMoving;
    }

    // GETTERS

}
