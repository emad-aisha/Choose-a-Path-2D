using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerManager : MonoBehaviour {
    public static PlayerManager instance;

    //("Player Data")
    float colliderWidth;
    float colliderHeight;

    [Header("Triggers")]
    [SerializeField] Trigger body;
    [SerializeField] Trigger head;


    void Awake() {
        if (instance == null) instance = this;
        HorizontalFacingDirectionManager.instance.ChangeDirection += FlipPlayer;

        colliderWidth = GetComponent<BoxCollider2D>().bounds.size.x;
        colliderHeight = GetComponent<BoxCollider2D>().bounds.size.y;
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
        else return new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized;
    }

    public Health GetHealth() { return MovementManager.instance.GetHealth(); }

    // TRIGGERS
    public ref Trigger GetBody() { return ref body; }
    public ref Trigger GetHead() { return ref head; }
    public bool IsGrounded() { return body.isGrounded; }


    // EVENTS
    void FlipPlayer() { StartCoroutine(AbilityManager.instance.FlipPlayerCoroutine()); }


}
