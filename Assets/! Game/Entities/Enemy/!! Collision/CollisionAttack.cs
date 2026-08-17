using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class CollisionAttack : MonoBehaviour {
    [Header("Basic Stats")]
    [SerializeField] int damage = 1;

    void Start() {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && collision.TryGetComponent(out Health playerHealth)) {
            playerHealth.Hurt(damage);
        }
    }


}
