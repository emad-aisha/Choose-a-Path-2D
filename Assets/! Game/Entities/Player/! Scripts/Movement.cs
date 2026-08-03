using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : Input {
    [Header("Move Stats")]
    [SerializeField] float walkSpeed;
    InputAction moveAction;
    bool isMoving;

    [Header("Jump Stats")]
    [SerializeField] float jumpSpeed;
    [SerializeField] float coyoteTime;
    float stopJumpMod = 0.5f;
    InputAction jumpAction;

    [Header("Gravity")]
    [SerializeField] float maxGravity;
    [SerializeField] float gravity;
    [SerializeField] float gravityAcceleration;
    bool isJumping;
    bool canJump = true;

    Rigidbody2D rigidBody;
    Grounded body;

    void Start() {
        SetMoveAction();
        body = GetComponentInChildren<Grounded>();
        body.HitGround += HitGround;
        body.LeftGround += LeftGround;

        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.gravityScale = gravity;
    }

    void Update() {
        if (isMoving) {
            int moveDirection = Mathf.RoundToInt(moveAction.ReadValue<Vector2>().x);
            rigidBody.linearVelocityX = moveDirection * walkSpeed;
        }

        if (isJumping && rigidBody.gravityScale < maxGravity) {
            rigidBody.gravityScale += gravityAcceleration * Time.deltaTime;
        }
    }

    // EVENTS ---
    // DEALLOCATION
    void OnDisable() {
        moveAction.performed -= Move;
        moveAction.canceled -= StopMoving;

        jumpAction.performed -= Jump;
        jumpAction.canceled -= StopJumping;

        body.HitGround -= HitGround;
        body.LeftGround -= LeftGround;
    }

    // MOVE
    void Move(InputAction.CallbackContext context) {
        int moveDirection = Mathf.RoundToInt(context.ReadValue<Vector2>().x);
        if (moveDirection == 0) return;

        isMoving = true;
        rigidBody.linearVelocityX = moveDirection * walkSpeed;
    }
    void StopMoving(InputAction.CallbackContext context) {
        int moveDirection = Mathf.RoundToInt(context.ReadValue<Vector2>().x);
        if (moveDirection != 0) return;

        isMoving = false;
        rigidBody.linearVelocityX = 0;
    }

    // JUMP
    void Jump(InputAction.CallbackContext context) {
        isJumping = true;
        if (body.isGrounded || canJump) {
            rigidBody.linearVelocityY = jumpSpeed;
        }
    }
    void StopJumping(InputAction.CallbackContext context) {
        isJumping = false;
        rigidBody.linearVelocityY *= stopJumpMod;
    }
    void HitGround() {
        rigidBody.gravityScale = gravity;
        canJump = false;
    }
    void LeftGround() { if (!isJumping) StartCoroutine(CoyoteTime()); }

    // HELPER -- 
    IEnumerator CoyoteTime() {
        canJump = true;
        yield return new WaitForSeconds(coyoteTime);
        canJump = false;
    }

    // SETTERS ---
    void SetMoveAction() {
        moveAction = InputManager.instance.GetAction(actionName, "Move");
        jumpAction = InputManager.instance.GetAction(actionName, "Jump");

        moveAction.performed += Move;
        moveAction.canceled += StopMoving;

        jumpAction.performed += Jump;
        jumpAction.canceled += StopJumping;
    }

    // GETTERS ---

}
