using UnityEngine;

public class Grounded : MonoBehaviour {
    public bool isGrounded;

    void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.CompareTag("Player") && !collision.CompareTag("Enemy")) {
            isGrounded = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (!collision.CompareTag("Player") && !collision.CompareTag("Enemy")) {
            isGrounded = false;
        }
    }
}
