using System.Collections;
using Unity.Mathematics;
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
    [SerializeField] float verticalAttackOffset = 2;

    InputAction faceAction;
    Vector2 facingDirection;
    Vector2 attackDirection;


    void Start() {
        AbilityManager.instance.SetAttack(gameObject);
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
        int roundedY = Mathf.RoundToInt(facingDirection.y);
        int roundedX = Mathf.RoundToInt(facingDirection.x);
        hitBox.transform.position = PlayerManager.instance.GetTransform().position;

        // attack vertically
        if (facingDirection.y != 0) {

            // if facing down and not on ground OR facing up
            if ((roundedY < 0 && !PlayerManager.instance.IsGrounded()) || roundedY > 0) {
                hitBox.transform.position += new Vector3(0, roundedY * verticalAttackOffset);
                attackDirection.x = 0;
                attackDirection.y = roundedY;
            }
            else {
                // attack horizontally
                hitBox.transform.position += new Vector3(HorizontalFacingDirectionManager.instance.GetDirection() * attackOffset, 0);
                attackDirection.x = HorizontalFacingDirectionManager.instance.GetDirection();
                attackDirection.y = 0;
            }
        }
        else {
            // attack horizontally
            hitBox.transform.position += new Vector3(HorizontalFacingDirectionManager.instance.GetDirection() * attackOffset, 0);
            attackDirection.x = HorizontalFacingDirectionManager.instance.GetDirection();
            attackDirection.y = 0;
        }

    }

    // EVENT ---
    void StartAttack(InputAction.CallbackContext context) {
        if (!canAttack) return;
        StartCoroutine(EnableHitbox());
        StartCoroutine(AttackCooldown());
    }

    // GETTERS
    public Vector2 GetAttackDirection() { return attackDirection; }

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
