using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Door : MonoBehaviour {
    [Header("Basic Stats")]
    [SerializeField] bool isOpen;

    BoxCollider2D boxCollider;
    SpriteRenderer sprite;

    public delegate void OpenEvent();
    event OpenEvent Open;

    void Start() {
        SetDefaults();

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

    void SetDefaults() {
        boxCollider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }


}
