using UnityEngine;


public class PlayerManager : MonoBehaviour {
    public static PlayerManager instance;

    [SerializeField] BoxCollider2D playerCollider;
    float colliderWidth;
    float colliderHeight;

    [Header("Triggers")]
    [SerializeField] Trigger body;
    [SerializeField] Trigger head;


    void Awake() {
        if (instance == null) instance = this;
        HorizontalFacingDirectionManager.instance.ChangeDirection += FlipPlayer;

        colliderWidth = playerCollider.bounds.size.x;
        colliderHeight = playerCollider.bounds.size.y;
    }
    void OnDisable() {
        HorizontalFacingDirectionManager.instance.ChangeDirection -= FlipPlayer;
    }


    // GETTERS
    public Transform GetTransform() { return transform; }
    public float GetPlayerWidth() { return colliderWidth; }
    public float GetPlayerHeight() { return colliderHeight; }

    public Vector2 GetAttackDirection() {
        if (AbilityManager.instance.GetAttack()) return AbilityManager.instance.GetAttack().GetAttackDirection();
        else return new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)).normalized;
    }

    public Health GetHealth() { return MovementManager.instance.GetHealth(); }

    public BoxCollider2D GetCollider() { return playerCollider; }

    // TRIGGERS
    public ref Trigger GetBody() { return ref body; }
    public ref Trigger GetHead() { return ref head; }
    public bool IsGrounded() { return body.isGrounded; }


    // EVENTS
    void FlipPlayer() { StartCoroutine(AbilityManager.instance.FlipPlayerCoroutine()); }


}
