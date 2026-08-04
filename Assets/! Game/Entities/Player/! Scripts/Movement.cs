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
    bool canMove = true;


    void Start() {
        SetMoveAction();
        PlayerManager.instance.GetBody().TriggerEnter += HitGround;
        PlayerManager.instance.GetBody().TriggerExit += LeftGround;

        PlayerManager.instance.GetBody().TriggerEnter += HitHead;

        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.gravityScale = gravity;
    }


    void FixedUpdate() {
        if (!canMove) return;

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

        PlayerManager.instance.GetBody().TriggerEnter -= HitGround;
        PlayerManager.instance.GetBody().TriggerExit -= LeftGround;

        PlayerManager.instance.GetBody().TriggerExit -= HitHead;
    }

    // MOVE
    void Move(InputAction.CallbackContext context) {
        if (!canMove) return;
        int moveDirection = Mathf.RoundToInt(context.ReadValue<Vector2>().x);
        if (moveDirection == 0 || !canMove) return;

        isMoving = true;
        rigidBody.linearVelocityX = moveDirection * walkSpeed;
    }
    void StopMoving(InputAction.CallbackContext context) {
        if (!canMove) return;
        int moveDirection = Mathf.RoundToInt(context.ReadValue<Vector2>().x);
        if (moveDirection != 0) return;

        isMoving = false;
        rigidBody.linearVelocityX = 0;
    }

    // JUMP
    void Jump(InputAction.CallbackContext context) {
        if (!canMove) return;
        isJumping = true;
        if (PlayerManager.instance.IsGrounded() || canJump) {
            rigidBody.linearVelocityY = jumpSpeed;
        }
    }
    void StopJumping(InputAction.CallbackContext context) {
        if (!canMove) return;
        isJumping = false;
        rigidBody.linearVelocityY *= stopJumpMod;
    }
    void HitHead() {
        if (!canMove) return;
        rigidBody.linearVelocity = Vector2.zero;
    }
    void HitGround() {
        if (!canMove) return;
        rigidBody.gravityScale = gravity;
        rigidBody.linearVelocity = Vector2.zero;
        canJump = false;
    }
    void LeftGround() {
        if (!canMove) return;
        if (!isJumping) StartCoroutine(CoyoteTime());
    }


    // HELPER -- 
    IEnumerator CoyoteTime() {
        if (!canMove) yield break;
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
    public void SetCanMove(bool value) {
        canMove = value;
    }

    public void SetGravity(float newGravity) { rigidBody.gravityScale = newGravity; }
    public void ResetGravity() { rigidBody.gravityScale = gravity; }

    public IEnumerator StopGravity(float time) {
        rigidBody.gravityScale = 0;
        yield return new WaitForSeconds(time);
        rigidBody.gravityScale = gravity;
    }


    // GETTERS ---

}
