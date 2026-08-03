using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : Input {
    Grounded body;

    [Header("Walk Stats")]
    [SerializeField] float walkSpeed;
    InputAction moveAction;
    InputAction jumpAction;

    [Header("Jump Stats")]
    [SerializeField] float maxJumpHeight;
    [SerializeField] float gravity;
    Rigidbody2D rigidBody;

    bool isMoving;

    void OnDisable() {
        moveAction.performed -= Move;
        moveAction.canceled -= StopMoving;

        jumpAction.performed -= Jump;
        jumpAction.canceled -= StopJumping;
    }

    void Start() {
        SetMoveAction();
        body = GetComponentInChildren<Grounded>();
        body.HitGround += HitGround;

        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.gravityScale = gravity;
    }

    void Update() {
        if (isMoving) {
            int moveDirection = Mathf.RoundToInt(moveAction.ReadValue<Vector2>().x);
            rigidBody.linearVelocityX = moveDirection * walkSpeed;
        }
    }

    // EVENTS
    void Move(InputAction.CallbackContext context) {
        int moveDirection = Mathf.RoundToInt(context.ReadValue<Vector2>().x);

        if (moveDirection != 0) {
            isMoving = true;

            rigidBody.linearVelocityX = moveDirection * walkSpeed;
        }
    }

    void StopMoving(InputAction.CallbackContext context) {
        int moveDirection = Mathf.RoundToInt(context.ReadValue<Vector2>().x);

        if (moveDirection == 0) {
            isMoving = false;
            rigidBody.linearVelocityX = 0;
        }
    }

    void Jump(InputAction.CallbackContext context) {
        if (body.isGrounded) {
            rigidBody.linearVelocityY = maxJumpHeight;
        }
    }


    void StopJumping(InputAction.CallbackContext context) {
        rigidBody.linearVelocityY *= 0.5f;
    }

    void HitGround() {
        rigidBody.linearVelocityY = 0;
    }

    // SETTERS
    void SetMoveAction() {
        moveAction = InputManager.instance.GetAction(actionName, "Move");
        jumpAction = InputManager.instance.GetAction(actionName, "Jump");

        moveAction.performed += Move;
        moveAction.canceled += StopMoving;

        jumpAction.performed += Jump;
        jumpAction.canceled += StopJumping;
    }

    // GETTERS

}
