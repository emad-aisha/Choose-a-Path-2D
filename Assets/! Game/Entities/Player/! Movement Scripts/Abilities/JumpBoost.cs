using UnityEngine;

public class JumpBoost : MonoBehaviour {
    [Header("Basic Stats")]
    [SerializeField] float newJumpSpeed;
    [SerializeField] bool isActive;

    public delegate void GotEvent();
    public event GotEvent StartJumpBoost;

    void OnDisable() { StartJumpBoost = null; }

    void Start() {
        // TODO: TEMPORAITL
        AbilityManager.instance.SetJumpBoost(this);
        ApplyJumpBoost();
    }

    // TODO: needs to be called in item to apply
    public void ApplyJumpBoost() { if (isActive) StartJumpBoost?.Invoke(); }

    // SETTERS
    public void SetActive(bool value) { isActive = value; }

    // GETTER
    public float GetJumpValue() { return newJumpSpeed; }

}
