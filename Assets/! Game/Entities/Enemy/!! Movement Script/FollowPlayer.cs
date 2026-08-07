using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class FollowPlayer : MonoBehaviour {
    [Header("Basic Stats")]
    [SerializeField] float speed;
    [SerializeField] float slowDownSpeed;

    [Header("Misc")]
    [SerializeField] float triggerDistance;
    [SerializeField] float stopDistance;
    Vector3 playerPosition;

    Rigidbody2D enemyRigidbody;

    void Start() {
        gameObject.tag = "Enemy";
        GetComponent<BoxCollider2D>().isTrigger = true;

        playerPosition = PlayerManager.instance.GetTransform().position;
        enemyRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update() {
        playerPosition = PlayerManager.instance.GetTransform().position;

        if (math.distance(playerPosition, transform.position) < triggerDistance
         && math.distance(playerPosition, transform.position) > stopDistance) {
            enemyRigidbody.linearVelocity = (playerPosition - transform.position).normalized * speed;
        }
        else {
            int xDirection = enemyRigidbody.linearVelocityX > 0 ? 1 : -1;
            int yDirection = enemyRigidbody.linearVelocityY > 0 ? 1 : -1;

            enemyRigidbody.linearVelocity -= new Vector2(slowDownSpeed * xDirection, slowDownSpeed * yDirection) * Time.deltaTime;
        }
    }

    // collision hurt
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent(out Health playerHealth)) {
            playerHealth.Hurt(1);
        }
    }

}
