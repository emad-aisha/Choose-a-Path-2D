using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public static PlayerManager instance;
    [Header("Triggers")]
    [SerializeField] Trigger body;
    [SerializeField] Trigger head;

    void Awake() {
        if (instance == null) instance = this;
    }

    // GETTERS
    public Transform GetTransform() { return transform; }
    public ref Trigger GetBody() { return ref body; }
    public ref Trigger GetHead() { return ref head; }

    public bool IsGrounded() { return body.isGrounded; }

    // SETTERS

}
