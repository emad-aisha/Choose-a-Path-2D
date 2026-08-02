using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : Input {
    Grounded body;

    [Header("Walk Stats")]
    [SerializeField] float walkSpeed;
    InputAction moveAction;
    Vector3 moveDirection;

    [Header("Jump Stats")]
    [SerializeField] float jumpSpeed;
    [SerializeField] float holdJumpTime;
    [SerializeField] float gravity;

    Rigidbody2D rigidBody;


    bool ignore;
    void OnDisable() {
        moveAction.performed -= Move;
        moveAction.canceled -= StopMoving;
    }

    void Start() {
        SetMoveAction();
        body = GetComponentInChildren<Grounded>();

        moveDirection = Vector2.zero;
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.gravityScale = gravity;
    }

    void Update() {
        //transform.position += moveDirection * Time.deltaTime;
    }


    // EVENTS
    void Move(InputAction.CallbackContext context) {
        moveDirection = context.ReadValue<Vector2>();
        moveDirection.x *= walkSpeed;

        if (moveDirection.y > 0) moveDirection.y *= jumpSpeed;
        else moveDirection.y = 0;

        rigidBody.linearVelocityX = moveDirection.x;
        if (body.isGrounded) rigidBody.linearVelocityY = moveDirection.y;
    }

    void StopMoving(InputAction.CallbackContext context) {
        rigidBody.linearVelocity = new Vector2(0, rigidBody.linearVelocity.y * 0.5f);
    }

    // SETTERS
    void SetMoveAction() {
        moveAction = InputManager.instance.GetAction(actionName, "Move");

        moveAction.performed += Move;
        moveAction.canceled += StopMoving;
    }

    // GETTERS

}
