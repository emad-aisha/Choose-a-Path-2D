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
    [SerializeField] float attackOffset = 1;

    InputAction faceAction;
    Vector2 facingDirection;


    void Start() {
        attackAction = InputManager.instance.GetAction(actionName, "Attack");
        faceAction = InputManager.instance.GetAction(actionName, "Move");
        attackAction.performed += StartAttack;

        hitBox.GetComponent<HurtBox>().SetDamage(damage);
    }
    void OnDisable() {
        attackAction.performed -= StartAttack;
    }
    void Update() { AttackDirection(); }


    void AttackDirection() {
        if (hitBox.activeSelf) return; // dont change direction when shown
        facingDirection = faceAction.ReadValue<Vector2>();

        // attack vertically
        if (facingDirection.y != 0) {
            hitBox.transform.position = PlayerManager.instance.GetTransform().position;

            // if facing down and not on ground OR facing up
            if ((facingDirection.y < 0 && !PlayerManager.instance.IsGrounded()) || facingDirection.y > 0) {
                hitBox.transform.position += new Vector3(0, VerticalFacingDirectionManager.instance.GetPureDirection() * attackOffset);
            }
            else {
                // attack horizontally
                hitBox.transform.position = PlayerManager.instance.GetTransform().position;
                hitBox.transform.position += new Vector3(HorizontalFacingDirectionManager.instance.GetDirection() * attackOffset, 0);
            }
        }
        else {
            // attack horizontally
            hitBox.transform.position = PlayerManager.instance.GetTransform().position;
            hitBox.transform.position += new Vector3(HorizontalFacingDirectionManager.instance.GetDirection() * attackOffset, 0);
        }

    }

    // EVENT ---
    void StartAttack(InputAction.CallbackContext context) {
        if (!canAttack) return;
        StartCoroutine(EnableHitbox());
        StartCoroutine(AttackCooldown());
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
