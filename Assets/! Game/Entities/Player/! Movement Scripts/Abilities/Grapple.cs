using UnityEngine;
using UnityEngine.InputSystem;

public class Grapple : Input {
    InputAction interactAction;

    public enum Direction { None, Up, Side };
    [Header("Basic Stats")]
    public LayerMask ignoreLayers;
    public Direction direction;
    public float distance;
    public float speed;
    public float windup;
    public bool isActive;
    // bool canGrapple = true;

    public delegate void GrappleEvent();
    public event GrappleEvent StartGrapple;


    void Start() {
        AbilityManager.instance.SetGrapple(this);

        interactAction = InputManager.instance.GetAction(actionName, "Interact");
        interactAction.started += Interact;
    }

    void OnDisable() {
        interactAction.started -= Interact;
        StartGrapple = null;
    }

    // EVENTS
    void Interact(InputAction.CallbackContext context) {
        if (!isActive) return;
        StartGrapple?.Invoke();
    }


}
