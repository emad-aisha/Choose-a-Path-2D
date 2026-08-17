using UnityEngine;
using System.Collections;


public class FireballAttack : BasicAttack {
    [Header("Fireball Info")]
    [SerializeField] GameObject fireballPrefab;
    [SerializeField, InspectorName("Amount")] int numberOfFireballs;
    [SerializeField] float timeBetweenShots;
    [SerializeField] float lifespan;

    [Header("Fireball Stats")]
    [SerializeField] float speed;

    Vector3 attackPosition;

    void LateUpdate() {
        if (standStill) {
            transform.position = attackPosition;
        }
    }

    override public void Attack() {
        StartCoroutine(AtttackSequence());
    }


    // timers
    protected IEnumerator AtttackSequence() {
        canAttack = false;
        attackPosition = transform.position;
        standStill = true;
        yield return new WaitForSeconds(windup); // TODO: windup anim

        yield return StartCoroutine(SpawnFireballs());
        standStill = false;

        StartCoroutine(AttackCooldown());
    }

    IEnumerator SpawnFireballs() {
        for (int i = 0; i < numberOfFireballs; i++) {
            GameObject fireballObject = Instantiate(fireballPrefab, attackPosition, Quaternion.identity);
            Fireball fireball = fireballObject.GetComponent<Fireball>();
            fireball.SetData(speed, damage, attackPosition);
            fireball.StartLife(lifespan);

            yield return new WaitForSeconds(timeBetweenShots);
        }
    }

}
