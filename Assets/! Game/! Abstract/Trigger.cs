using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D)), RequireComponent(typeof(Rigidbody2D))]
public class Trigger : MonoBehaviour {
    [SerializeField] List<string> ignoreTags;
    [SerializeField] List<string> useTags;
    public bool isGrounded;

    public delegate void HitGroundEvent();
    public event HitGroundEvent TriggerEnter;
    public event HitGroundEvent TriggerExit;

    void Start() {
        GetComponent<BoxCollider2D>().isTrigger = true;
        tag = "Trigger";

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }

    void OnDisable() {
        TriggerEnter = null;
        TriggerExit = null;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (CheckTags(collision)) {
            isGrounded = true;
            TriggerEnter?.Invoke();
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (CheckTags(collision)) {
            isGrounded = false;
            TriggerExit?.Invoke();
        }
    }

    bool CheckTags(Collider2D collision) {
        return UseTags(collision) && IgnoresTags(collision);
    }


    bool IgnoresTags(Collider2D collision) {
        if (ignoreTags == null) return true;

        for (int i = 0; i < ignoreTags.Count; i++) {
            if (collision.CompareTag(ignoreTags[i])) return false;
        }
        return true;
    }

    bool UseTags(Collider2D collision) {
        if (useTags == null) return true;

        for (int i = 0; i < useTags.Count; i++) {
            if (!collision.CompareTag(useTags[i])) return false;
        }
        return true;
    }
}
