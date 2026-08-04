using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerManager : MonoBehaviour {
    public static PlayerManager instance;
    [Header("Player Data")]
    [SerializeField] float colliderWidth;
    [SerializeField] float colliderHeight;

    [Header("Knockback")]
    [SerializeField] Health playerHealth;
    [SerializeField] HurtBox hurtBox;
    [SerializeField] float knockback;
    [SerializeField] float knockbackTime;

    [Header("Triggers")]
    [SerializeField] Trigger body;
    [SerializeField] Trigger head;

    [Header("Misc")]
    [SerializeField] Rigidbody2D playerRigidbody;
    [SerializeField] Movement movementController;
    [SerializeField] Attack attack;


    void Awake() {
        if (instance == null) instance = this;
        hurtBox.Hit += StartKnockback;
        playerHealth.Fling += StartKnockback;
        HorizontalFacingDirectionManager.instance.ChangeDirection += FlipPlayer;

        colliderWidth = GetComponent<BoxCollider2D>().bounds.size.x;
        colliderHeight = GetComponent<BoxCollider2D>().bounds.size.y;
    }

    void OnDisable() {
        hurtBox.Hit -= StartKnockback;
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


    // attack
    public Vector2 GetAttackDirection() { return attack.GetAttackDirection(); }

    // EVENTS
    void StartKnockback() { StartCoroutine(SetKnockback(-GetAttackDirection())); }
    void FlipPlayer() { StartCoroutine(FlipPlayerCoroutine()); }

    // TIMERS
    public IEnumerator SetKnockback(Vector3 direction) {
        if (!gameObject.CompareTag("Player")) yield break;

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

    IEnumerator FlipPlayerCoroutine() {
        int safety = 0;
        while (hurtBox.gameObject.activeSelf && safety < 200) {
            safety++;
            yield return new WaitForEndOfFrame();
        }
        transform.Rotate(0, 180, 0);
    }

}
