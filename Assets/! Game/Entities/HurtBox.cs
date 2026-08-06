using UnityEngine;

public class HurtBox : MonoBehaviour {
    [SerializeField] int damage = 0;

    public delegate void HitEvent();
    public event HitEvent Hit;


    public void SetDamage(int newDamage) { damage = newDamage; }
    void OnTriggerEnter2D(Collider2D collision) {
        // ignore self
        if (!collision.CompareTag(gameObject.tag) && collision.TryGetComponent(out Health health)) {

            //if (gameObject.CompareTag("Player")) PlayerManager.instance.StartKnockback();
            Hit?.Invoke();
            health.Hurt(damage);
        }
    }

}
