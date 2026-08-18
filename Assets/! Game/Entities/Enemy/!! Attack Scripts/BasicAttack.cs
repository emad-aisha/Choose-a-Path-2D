using System.Collections;
using UnityEngine;
using Unity.Mathematics;

public abstract class BasicAttack : MonoBehaviour {
    [Tooltip("Dont forget to set this to Enemy and Ignore Raycast for it to work properly")]
    [SerializeField] LayerMask ignoreLayer;

    [Header("Basic Stats")]
    [SerializeField] float attackRange;
    [SerializeField] protected int damage;
    [SerializeField] bool lineOfSiteNeeded;

    [Header("Timers")]
    [SerializeField] float windup;
    [SerializeField] float cooldown;
    protected bool canAttack = true;


    protected bool standStill = false;
    protected Vector3 stopPosition;


    void Update() {
        // Line of site check
        Debug.DrawRay(transform.position, (PlayerManager.instance.GetTransform().position - transform.position).normalized * attackRange, Color.red);
        if (CheckLineOfSite())
            if ((math.distance(transform.position, PlayerManager.instance.GetTransform().position) <= attackRange) && canAttack) Attack();
    }

    void LateUpdate() {
        if (standStill) transform.position = stopPosition;
    }


    public void Attack() { StartCoroutine(AtttackSequence()); }
    abstract protected IEnumerator AttackLogic();


    IEnumerator AtttackSequence() {
        yield return StartCoroutine(Windup());
        yield return StartCoroutine(AttackLogic());
        StartCoroutine(AttackCooldown());
    }


    // timers
    protected IEnumerator Windup() {
        canAttack = false;
        stopPosition = transform.position;
        standStill = true;
        yield return new WaitForSeconds(windup); // TODO: windup anim
        standStill = false;
    }

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
