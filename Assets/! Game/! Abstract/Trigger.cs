using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {
    [SerializeField] List<string> ignoreTags;
    public bool isGrounded;

    public delegate void HitGroundEvent();
    public event HitGroundEvent TriggerEnter;
    public event HitGroundEvent TriggerExit;

    void OnDisable() {
        TriggerEnter = null;
        TriggerExit = null;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (IgnoresTags(collision)) {
            isGrounded = true;
            TriggerEnter?.Invoke();
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (IgnoresTags(collision)) {
            isGrounded = false;
            TriggerExit?.Invoke();
        }
    }


    bool IgnoresTags(Collider2D collision) {
        if (ignoreTags == null) return true;

        for (int i = 0; i < ignoreTags.Count; i++) {
            if (collision.CompareTag(ignoreTags[i])) return false;
        }
        return true;
    }
}
