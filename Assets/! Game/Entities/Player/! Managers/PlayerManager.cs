using System.Collections;
using UnityEngine;

// TODO: seperate into a movement manager
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerManager : MonoBehaviour {
    public static PlayerManager instance;
    //("Player Data")
    float colliderWidth;
    float colliderHeight;

    [Header("Knockback")]
    [SerializeField] Health playerHealth;
    [SerializeField] float knockback;
    [SerializeField] float knockbackTime;

    [Header("Triggers")]
    [SerializeField] Trigger body;
    [SerializeField] Trigger head;

    [Header("Misc")]
    [SerializeField] Rigidbody2D playerRigidbody;
    [SerializeField] Movement movementController;


    void Awake() {
        if (instance == null) instance = this;
        playerHealth.Fling += StartKnockback;
        HorizontalFacingDirectionManager.instance.ChangeDirection += FlipPlayer;

        colliderWidth = GetComponent<BoxCollider2D>().bounds.size.x;
        colliderHeight = GetComponent<BoxCollider2D>().bounds.size.y;
    }

    void OnDisable() {
        playerHealth.Fling -= StartKnockback;
        HorizontalFacingDirectionManager.instance.ChangeDirection -= FlipPlayer;
    }

    // GETTERS
    public Transform GetTransform() { return transform; }
    public float GetPlayerWidth() { return colliderWidth; }
    public float GetPlayerHeight() { return colliderHeight; }

    // triggers
    public ref Trigger GetBody() { return ref body; }
    public ref Trigger GetHead() { return ref head; }

    // movement
    public ref Rigidbody2D GetRigidbody() { return ref playerRigidbody; }
    public void SetPlayerCanMove(bool value) {
        if (!value) playerRigidbody.linearVelocity = Vector2.zero;
        movementController.SetCanMove(value);
    }
    public bool IsGrounded() { return body.isGrounded; }
    // gravity
    public void StopPlayerGravity(float time) { StartCoroutine(movementController.StopGravity(time)); }
    public void StopPlayerGravity() { movementController.SetGravity(0); }
    public void StartPlayerGravity() { movementController.ResetGravity(); }

    public float GetWalkSpeed() { return movementController.GetWalkSpeed(); }
    public void SetPlayerSprint(bool isSprinting, float sprintMod) {
        movementController.SetSprinting(isSprinting);
        movementController.SetSprintMod(sprintMod);
    }

    public void SetJumpValue(float newJumpValue) {
        movementController.SetJumpSpeed(newJumpValue);
    }


    // attack
    public Vector2 GetAttackDirection() {
        if (AbilityManager.instance.GetAttack()) return AbilityManager.instance.GetAttack().GetAttackDirection();
        else return new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized;
    }

    // EVENTS
    public void StartKnockback() { StartCoroutine(SetKnockback(-GetAttackDirection())); }
    void FlipPlayer() { StartCoroutine(AbilityManager.instance.FlipPlayerCoroutine()); }

    // TIMERS
    public IEnumerator SetKnockback(Vector3 direction) {
        movementController.SetCanMove(false);

        playerRigidbody.linearVelocity = Vector2.zero;
        playerRigidbody.linearVelocity = direction * knockback;
        if (direction.y > 0) playerRigidbody.linearVelocity = direction * (knockback * 2); // double force

        float time = 0;
        while (time < knockbackTime) {
            time += Time.deltaTime;
            playerRigidbody.linearDamping += Time.deltaTime * knockback;

            yield return new WaitForSeconds(Time.deltaTime);
        }

        playerRigidbody.linearDamping = 0;
        playerRigidbody.linearVelocity = Vector2.zero;
        movementController.SetCanMove(true);
    }

}
