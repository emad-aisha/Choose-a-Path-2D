using UnityEngine;
using UnityEngine.InputSystem;

public class Sprint : Input {
    [Header("Basic Stats")]
    [SerializeField] float sprintMod;
    [SerializeField] float timeToSpeed;

    public delegate void StartSprintEvent();
    public event StartSprintEvent StartSprint;
    public event StartSprintEvent EndSprint;
    InputAction sprintAction;


    void Start() {
        sprintAction = InputManager.instance.GetAction(actionName, "Sprint");
        sprintAction.started += StartSprinting;
        sprintAction.canceled += StopSprinting;

        AbilityManager.instance.SetSprint(this);
    }
    void OnDisable() {
        sprintAction.started -= StartSprinting;
        sprintAction.canceled -= StopSprinting;
        StartSprint = null;
        EndSprint = null;
    }

    // EVENTS
    void StartSprinting(InputAction.CallbackContext context) { StartSprint?.Invoke(); }
    void StopSprinting(InputAction.CallbackContext context) { EndSprint?.Invoke(); }

    // GETTERS
    public float GetSprintMod() { return sprintMod; }
    public float GetTimeToSpeed() { return timeToSpeed; }

}
