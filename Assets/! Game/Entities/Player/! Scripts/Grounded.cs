using UnityEngine;

public class Grounded : MonoBehaviour {
    public bool isGrounded;
    public delegate void HitGroundEvent();
    public event HitGroundEvent HitGround;


    void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.CompareTag("Player") && !collision.CompareTag("Enemy")) {
            isGrounded = true;
            HitGround?.Invoke();
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (!collision.CompareTag("Player") && !collision.CompareTag("Enemy")) {
            isGrounded = false;
        }
    }
}
