using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class HurtBox : MonoBehaviour {
    [SerializeField] int damage = 0;

    public delegate void HitEvent();
    public event HitEvent Hit;

    void Start() {
        gameObject.tag = "Trigger";
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public void SetDamage(int newDamage) { damage = newDamage; }
    void OnTriggerEnter2D(Collider2D collision) {
        // ignore self
        if (!collision.CompareTag(gameObject.tag) && collision.TryGetComponent(out Health health)) {

            Hit?.Invoke();
            health.Hurt(damage);
        }
    }

}
