using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Fireball : MonoBehaviour {
    int damage = 1;
    float speed;

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


    public void SetData(float speedValue, int damageValue, Vector3 enemyPos) {
        speed = speedValue;

        shotPosition = enemyPos;
        playerPosition = PlayerManager.instance.GetTransform().position;
        damage = damageValue;

        direction = (playerPosition - shotPosition).normalized;
    }

    public void StartLife(float lifespan) {
        StartCoroutine(LifeSpan(lifespan));
    }

    // damager
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && other.TryGetComponent(out Health playerHealth)) {
            playerHealth.Hurt(damage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Collision")) {
            Destroy(gameObject);
        }
    }

    // timer
    public IEnumerator LifeSpan(float lifespan) {
        yield return new WaitForSeconds(lifespan);
        Destroy(gameObject);
    }

}
