using System;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public static PlayerManager instance;
    [Header("Knockback Stats")]
    [SerializeField] float knockback;
    [SerializeField] float knockbackTime;

    [Header("Triggers")]
    [SerializeField] Trigger body;
    [SerializeField] Trigger head;

    [Header("Misc")]
    [SerializeField] Rigidbody2D playerRigidbody;
    [SerializeField] Movement movementController;
    [SerializeField] Health playerHealth;
    [SerializeField] Attack attack;
    [SerializeField] HurtBox hurtBox;


    void Awake() {
        if (instance == null) instance = this;
        hurtBox.Hit += StartKnockback;
        playerHealth.Fling += StartKnockback;
    }

    void OnDisable() {
        hurtBox.Hit -= StartKnockback;
        playerHealth.Fling -= StartKnockback;
    }

    // GETTERS
    public Transform GetTransform() { return transform; }
    public ref Trigger GetBody() { return ref body; }
    public ref Trigger GetHead() { return ref head; }
    public ref Rigidbody2D GetRigidbody() { return ref playerRigidbody; }

    public bool IsGrounded() { return body.isGrounded; }
    public Vector2 GetAttackDirection() { return attack.GetAttackDirection(); }

    // EVENTS
    void StartKnockback() { StartCoroutine(SetKnockback(-GetAttackDirection())); }


    // SETTERS
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

}
