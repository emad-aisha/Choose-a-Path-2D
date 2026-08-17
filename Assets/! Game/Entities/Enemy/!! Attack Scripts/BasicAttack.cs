using System.Collections;
using UnityEngine;
using Unity.Mathematics;

public abstract class BasicAttack : MonoBehaviour {
    [Header("Basic Stats")]
    [SerializeField] protected float attackRange;
    [SerializeField] protected int damage;

    [Header("Timers")]
    [SerializeField] protected float windup;
    [SerializeField] float cooldown;
    protected bool canAttack = true;
    protected bool standStill = false;

    void Update() {
        // TODO: DO a line of sight check
        if ((math.distance(transform.position, PlayerManager.instance.GetTransform().position) <= attackRange) && canAttack) Attack();
    }

    public abstract void Attack();


    protected IEnumerator AttackCooldown() {
        canAttack = false;
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }

}
