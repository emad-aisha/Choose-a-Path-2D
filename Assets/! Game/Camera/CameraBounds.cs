using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CameraBounds : MonoBehaviour {

    void Start() {
        GetComponent<BoxCollider2D>().isTrigger = true;
        gameObject.tag = "Trigger";
    }
}
