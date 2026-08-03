using UnityEngine;


public class HurtBox : MonoBehaviour {
    int damage = 0;

    public void SetDamage(int newDamage) { damage = newDamage; }

    void OnTriggerEnter2D(Collider2D collision) {
        // ignore self
        if (!collision.CompareTag(gameObject.tag) && collision.TryGetComponent(out Health health)) {
            health.Hurt(damage);
        }
    }

}
