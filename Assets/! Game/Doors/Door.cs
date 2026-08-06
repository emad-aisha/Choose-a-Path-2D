using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Door : MonoBehaviour {
    [Header("Basic Stats")]
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] bool isOpen;

    public delegate void OpenEvent();
    event OpenEvent Open;

    void Start() {
        Open += StartOpen;
        SetOpen(isOpen);
    }

    void OnDisable() {
        Open -= StartOpen;
        Open = null;
    }

    public void SetOpen(bool value) {
        isOpen = value;
        Open?.Invoke();
    }

    public void SetNotOpen() {
        isOpen = !isOpen;
        Open?.Invoke();
    }


    void StartOpen() {
        boxCollider.enabled = !isOpen;
        if (sprite) sprite.enabled = !isOpen;

        if (isOpen) gameObject.tag = "Trigger";
        else gameObject.tag = "Untagged";

    }

}
