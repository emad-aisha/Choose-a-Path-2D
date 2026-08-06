using UnityEngine;

public class JumpBoost : MonoBehaviour {
    [Header("Basic Stats")]
    [SerializeField] float newJumpSpeed;

    // GETTER
    public float GetJumpValue() { return newJumpSpeed; }

}
