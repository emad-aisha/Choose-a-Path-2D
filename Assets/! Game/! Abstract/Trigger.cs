using UnityEngine;

public class Trigger : MonoBehaviour {
    public bool isGrounded;
    public delegate void HitGroundEvent();
    public event HitGroundEvent TriggerEnter;
    public event HitGroundEvent TriggerExit;

    void OnDisable() {
        TriggerEnter = null;
        TriggerExit = null;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.CompareTag("Player") && !collision.CompareTag("Enemy")) {
            isGrounded = true;
            TriggerEnter?.Invoke();
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (!collision.CompareTag("Player") && !collision.CompareTag("Enemy")) {
            isGrounded = false;
            TriggerExit?.Invoke();
        }
    }
}
