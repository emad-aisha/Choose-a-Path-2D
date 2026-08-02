using UnityEngine;

public abstract class Input : MonoBehaviour {
    [SerializeField] protected string actionName = "Player";

    void OnEnable() { InputManager.instance.EnableAction(actionName); }
}
