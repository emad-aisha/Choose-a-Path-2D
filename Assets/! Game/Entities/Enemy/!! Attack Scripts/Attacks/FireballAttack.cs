using UnityEngine;
using System.Collections;


public class FireballAttack : BasicAttack {
    [Header("Fireball Info")]
    [SerializeField] GameObject prefab;
    [SerializeField, InspectorName("Amount"), Range(1, 5)] int numberOfFireballs = 1;
    [SerializeField, Range(0.1f, 0.8f)] float timeBetweenShots;
    [SerializeField] float lifespan;

    [Header("Fireball Stats")]
    [SerializeField, Range(4, 8)] int speed;
    [SerializeField, Range(0, 0.5f)] float acceleration;

    Vector3 attackPosition;

    void LateUpdate() {
        if (standStill) {
            transform.position = attackPosition;
        }
    }

    override public void Attack() {
        StartCoroutine(AtttackSequence());
    }


    // attack timer
    protected IEnumerator AtttackSequence() {
        canAttack = false;
        attackPosition = transform.position;
        standStill = true;
        yield return new WaitForSeconds(windup); // TODO: windup anim

        if (prefab.TryGetComponent(out Boomerang boomerang)) {
            StartCoroutine(WaitLonger());
            yield return StartCoroutine(SpawnBoomerangs());
        }
        else if (prefab.TryGetComponent(out Fireball fireball)) {
            yield return StartCoroutine(SpawnFireballs());
        }

        standStill = false;

        StartCoroutine(AttackCooldown());
    }


    // fireball timer
    IEnumerator SpawnFireballs() {
        for (int i = 0; i < numberOfFireballs; i++) {
            GameObject fireballObject = Instantiate(prefab, attackPosition, Quaternion.identity);
            Fireball fireball = fireballObject.GetComponent<Fireball>();
            fireball.SetData(speed, damage, attackPosition);
            fireball.StartLife(lifespan);

            yield return new WaitForSeconds(timeBetweenShots);
        }
    }


    // boomerang timers
    IEnumerator SpawnBoomerangs() {
        for (int i = 0; i < numberOfFireballs; i++) {
            GameObject boomerangObject = Instantiate(prefab, attackPosition, Quaternion.identity);
            Boomerang boomerang = boomerangObject.GetComponent<Boomerang>();
            boomerang.SetData(speed, acceleration, damage, attackPosition);
            boomerang.StartLife(lifespan);
            boomerang.StartComeback(transform.position);

            yield return new WaitForSeconds(timeBetweenShots);
        }
    }

    IEnumerator WaitLonger() {
        float time = 0;
        while (time < lifespan) {
            time += Time.deltaTime;
            standStill = true;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        standStill = false;
    }

}
