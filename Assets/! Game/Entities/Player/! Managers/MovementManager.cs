using UnityEngine;
using System.Collections;

public class MovementManager : MonoBehaviour {
    public static MovementManager instance;

    [Header("Knockback")]
    [SerializeField] Health playerHealth;
    [SerializeField] float knockback;
    [SerializeField] float knockbackTime;

    [Header("Misc")]
    [SerializeField] Rigidbody2D playerRigidbody;
    [SerializeField] Movement movementController;


    void Awake() {
        if (instance == null) instance = this;
        playerHealth.Fling += StartKnockback;
    }

    void OnDisable() { playerHealth.Fling -= StartKnockback; }

    public Health GetHealth() { return playerHealth; }

    // GETTERS
    public ref Rigidbody2D GetRigidbody() { return ref playerRigidbody; }

    // GRAVITY
    public void StopPlayerGravity(float time) { StartCoroutine(movementController.StopGravity(time)); }
    public void StopPlayerGravity() { movementController.SetGravity(0); }
    public void StartPlayerGravity() { movementController.ResetGravity(); }


    // SET MOVING VALUES 
    public void SetPlayerCanMove(bool value) {
        if (!value) playerRigidbody.linearVelocity = Vector2.zero;
        movementController.SetCanMove(value);
    }

    public void SetPlayerSprint(bool isSprinting, float sprintMod) {
        movementController.SetSprinting(isSprinting);
        movementController.SetSprintMod(sprintMod);
    }
    public void SetJumpValue(float newJumpValue) {
        movementController.SetJumpSpeed(newJumpValue);
    }

    // KNOCKBACK
    public void StartKnockback() { StartCoroutine(SetKnockback(-PlayerManager.instance.GetAttackDirection())); }
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
