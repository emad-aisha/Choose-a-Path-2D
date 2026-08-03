using UnityEngine;


public class HurtBox : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.CompareTag("Player") && collision.TryGetComponent(out Health health)) {
            Debug.Log("hrt");
        }
    }

}
