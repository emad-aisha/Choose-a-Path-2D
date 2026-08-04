using UnityEngine;
using UnityEngine.InputSystem;

public class Grapple : Input {
    InputAction interactAction;

    void Start() {
        interactAction = InputManager.instance.GetAction(actionName, "Interact");
        interactAction.performed += Interact;
    }

    void OnDisable() {
        interactAction.performed -= Interact;
    }

    // EVENTS
    void Interact(InputAction.CallbackContext context) {
        // get grapple direction
        // grapple in that direction
        
    }


}
