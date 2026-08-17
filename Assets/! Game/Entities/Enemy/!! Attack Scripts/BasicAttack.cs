using System.Collections;
using UnityEngine;
using Unity.Mathematics;

public abstract class BasicAttack : MonoBehaviour {
    [SerializeField] LayerMask ignoreLayer;

    [Header("Basic Stats")]
    [SerializeField] protected float attackRange;
    [SerializeField] protected int damage;
    [SerializeField] bool lineOfSiteNeeded;

    [Header("Timers")]
    [SerializeField] protected float windup;
    [SerializeField] float cooldown;
    protected bool canAttack = true;
    protected bool standStill = false;

    void Update() {
        // TODO: DO a line of sight check
        Debug.DrawRay(transform.position, (PlayerManager.instance.GetTransform().position - transform.position).normalized * attackRange, Color.red);

        if (CheckLineOfSite())
            if ((math.distance(transform.position, PlayerManager.instance.GetTransform().position) <= attackRange) && canAttack) Attack();
    }

    public abstract void Attack();


    protected IEnumerator AttackCooldown() {
        canAttack = false;
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }

    bool CheckLineOfSite() {
        if (!lineOfSiteNeeded) return true;
        else {
            RaycastHit2D raycast = Physics2D.Raycast(transform.position, (PlayerManager.instance.GetTransform().position - transform.position).normalized, attackRange, ~ignoreLayer);
            return raycast.collider && raycast.collider.CompareTag("Player");
        }
    }

}
