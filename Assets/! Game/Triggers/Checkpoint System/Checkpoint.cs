using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Checkpoint : MonoBehaviour {


    void OnTriggerEnter2D(Collider2D collision) {
        if (CheckpointManager.instance.GetCheckpointTransform() != transform.position && collision.CompareTag("Player")) {
            Debug.Log("checkpoint gt");
            CheckpointManager.instance.SetActiveCheckpoint(this);
        }
    }


}
