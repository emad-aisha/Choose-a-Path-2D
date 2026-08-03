using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : Input {
    InputAction attackAction;
    [Header("Basic Stats")]
    [SerializeField] int damage;
    [SerializeField] float cooldown;
    bool canAttack = true;

    [Header("Hitbox Stuff")]
    [SerializeField] GameObject hitBox;
    [SerializeField] float timeOnScreen;

    void Start() {
        attackAction = InputManager.instance.GetAction(actionName, "Attack");
        attackAction.performed += StartAttack;

        FacingDirectionManager.instance.ChangeDirection += MoveHitbox;
    }
    void OnDisable() {
        attackAction.performed -= StartAttack;

        FacingDirectionManager.instance.ChangeDirection -= MoveHitbox;
    }

    // EVENT ---
    void StartAttack(InputAction.CallbackContext context) {
        if (!canAttack) return;
        StartCoroutine(EnableHitbox());
        StartCoroutine(AttackCooldown());
    }

    void MoveHitbox() {
        hitBox.transform.position = new Vector3(PlayerManager.instance.GetTransform().position.x - FacingDirectionManager.instance.GetDirection(), PlayerManager.instance.GetTransform().position.y);
    }


    // TIMERS --- 
    IEnumerator EnableHitbox() {
        hitBox.SetActive(true);
        hitBox.GetComponent<BoxCollider2D>().enabled = true;
        yield return new WaitForSeconds(timeOnScreen);
        hitBox.GetComponent<BoxCollider2D>().enabled = false;
        hitBox.SetActive(false);
    }

    IEnumerator AttackCooldown() {
        canAttack = false;
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }

}
