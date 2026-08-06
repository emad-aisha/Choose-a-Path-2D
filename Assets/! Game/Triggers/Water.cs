using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Water : MonoBehaviour {

    public delegate void RespawnEvent();
    public event RespawnEvent Respawn;

    void Start() {
        RespawnManager.instance.AddWaterEvent(this);
    }

    void OnDisable() {
        Respawn = null;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && collision.gameObject.TryGetComponent(out Health playerHealth)) {
            playerHealth.Hurt(1);
            Respawn?.Invoke();
        }
        else if (collision.gameObject.TryGetComponent(out Health enemyHealth)) {
            enemyHealth.Hurt(10000);
        }
    }

}
