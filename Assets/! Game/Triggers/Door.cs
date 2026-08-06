using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Door : MonoBehaviour {
    [Header("Basic Stats")]
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] bool isOpen;

    void Start() {
        SetOpen(isOpen);
    }

    public void SetOpen(bool value) {
        isOpen = value;
        boxCollider.enabled = !isOpen;
        if (sprite) sprite.enabled = !isOpen;

        if (isOpen) gameObject.tag = "Trigger";
        else gameObject.tag = "Untagged";
    }

}
