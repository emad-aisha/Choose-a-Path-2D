using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(BoxCollider2D))]
public class Lever : MonoBehaviour {
    [Header("Basic Stats")]
    [SerializeField] Health health;
    [SerializeField] List<Door> doorsToOpen = new();
    [SerializeField] List<Door> doorsToClose = new();

    void Start() {
        health.Die += Flip;
    }

    void OnDisable() {
        health.Die -= Flip;
    }

    void Flip() {
        gameObject.tag = "Trigger";
        for (int i = 0; i < doorsToOpen.Count; i++) {
            doorsToOpen[i].SetOpen(true);
        }

        for (int i = 0; i < doorsToClose.Count; i++) {
            doorsToClose[i].SetOpen(false);
        }
    }

}
