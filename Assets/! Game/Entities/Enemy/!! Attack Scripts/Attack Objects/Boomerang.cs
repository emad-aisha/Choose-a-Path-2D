using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Boomerang : MonoBehaviour {
    // basic stats
    int damage = 1;
    float speed;
    float acceleration;
    float lifespan = 0;

    // positions
    Vector3 shotPosition;
    Vector3 playerPosition;
    Vector3 direction;



    void Start() {
        GetComponent<BoxCollider2D>().isTrigger = true;
        tag = "Trigger";
    }

    void LateUpdate() {
        transform.position += direction * (speed * Time.deltaTime);
    }

    public void SetData(float speedValue, float accelerationValue, int damageValue, Vector3 enemyPos) {
        speed = speedValue;
        acceleration = accelerationValue;
        damage = damageValue;

        shotPosition = enemyPos;
        playerPosition = PlayerManager.instance.GetTransform().position;

        direction = (playerPosition - shotPosition).normalized;
    }

    public void StartLife(float lifespanValue) {
        lifespan = lifespanValue;
        StartCoroutine(LifeSpan());
        StartCoroutine(AccelerateSpeed());
    }
    public void StartComeback(Vector3 enemyPosition) {
        StartCoroutine(Comeback(enemyPosition));
    }


    // damager
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && other.TryGetComponent(out Health playerHealth)) {
            playerHealth.Hurt(damage);
            Destroy(gameObject);
        }
    }

    // timers
    IEnumerator LifeSpan() {
        yield return new WaitForSeconds(lifespan);
        Destroy(gameObject);
    }

    IEnumerator Comeback(Vector3 currentEnemyPosition) {
        yield return new WaitForSeconds(lifespan / 2);
        direction = (currentEnemyPosition - playerPosition).normalized;
    }

    IEnumerator AccelerateSpeed() {
        float time = 0;

        while (time < (lifespan / 3)) {
            speed += acceleration;
            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        time = 0;
        while (time < (lifespan / 3)) {
            speed -= acceleration;
            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

    }

}
