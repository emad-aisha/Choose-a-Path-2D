using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {
    [HideInInspector] public static InputManager instance;
    [SerializeField] InputActionAsset inputActions;

    void Awake() { if (instance == null) instance = this; }

    public void EnableAction(string actionName) { inputActions.FindActionMap(actionName).Enable(); }
    public void DisableAction(string actionName) { inputActions.FindActionMap(actionName).Disable(); }

    public InputAction GetAction(string actionName, string action) { return inputActions.FindActionMap(actionName).FindAction(action); }

}
