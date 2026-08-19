using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(BoxCollider2D))]
public class Lever : MonoBehaviour {
    [Header("Basic Stats")]
    [SerializeField] Health health;
    [SerializeField] Door doorToToggle;

    void Start() {
        SetDefaults();

        health.Die += Flip;
    }

    void OnDisable() {
        health.Die -= Flip;
    }

    void Flip() {
        gameObject.tag = "Trigger";
        doorToToggle.SetNotOpen();
    }


    void SetDefaults() {
        health = GetComponent<Health>();

        GetComponent<BoxCollider2D>().isTrigger = true;
        tag = "Enemy";
        health.SetMaxHealth(1);
    }


}
