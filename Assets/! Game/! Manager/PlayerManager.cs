using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public static PlayerManager instance;

    void Awake() {
        if (instance == null) instance = this;
    }

    // GETTERS
    public Transform GetTransform() { return transform; }


    // SETTERS

}
