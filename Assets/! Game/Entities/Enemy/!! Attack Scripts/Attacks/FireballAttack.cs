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


    // attack logic
    override protected IEnumerator AttackLogic() {
        standStill = true;

        if (prefab.TryGetComponent(out Boomerang boomerang)) {
            StartCoroutine(WaitLonger());
            yield return StartCoroutine(SpawnBoomerangs());
        }
        else if (prefab.TryGetComponent(out Fireball fireball)) {
            yield return StartCoroutine(SpawnFireballs());
        }

        standStill = false;
    }


    // fireball timer
    IEnumerator SpawnFireballs() {
        for (int i = 0; i < numberOfFireballs; i++) {
            GameObject fireballObject = Instantiate(prefab, stopPosition, Quaternion.identity);
            Fireball fireball = fireballObject.GetComponent<Fireball>();
            fireball.SetData(speed, damage, stopPosition);
            fireball.StartLife(lifespan);

            yield return new WaitForSeconds(timeBetweenShots);
        }
    }


    // boomerang timers
    IEnumerator SpawnBoomerangs() {
        for (int i = 0; i < numberOfFireballs; i++) {
            GameObject boomerangObject = Instantiate(prefab, stopPosition, Quaternion.identity);
            Boomerang boomerang = boomerangObject.GetComponent<Boomerang>();
            boomerang.SetData(speed, acceleration, damage, stopPosition);
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
