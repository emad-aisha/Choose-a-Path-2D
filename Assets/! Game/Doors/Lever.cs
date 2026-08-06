using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(BoxCollider2D))]
public class Lever : MonoBehaviour {
    [Header("Basic Stats")]
    [SerializeField] Health health;
    [SerializeField] Door doorToToggle;

    void Start() {
        health.Die += Flip;
    }

    void OnDisable() {
        health.Die -= Flip;
    }

    void Flip() {
        gameObject.tag = "Trigger";
        doorToToggle.SetNotOpen();
    }

}
