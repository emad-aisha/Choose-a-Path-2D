using UnityEngine;
using System.Collections.Generic;

public class CheckpointManager : MonoBehaviour {
    public static CheckpointManager instance;

    List<Checkpoint> checkpoints = new();
    Vector3 activeCheckpointPosition;

    void Awake() {
        if (instance == null) instance = this;
        checkpoints.AddRange(gameObject.GetComponents<Checkpoint>());
    }


    public void SetActiveCheckpoint(Checkpoint newCheckpoint) { activeCheckpointPosition = newCheckpoint.transform.position; }
    public Vector3 GetCheckpointTransform() { return activeCheckpointPosition; }


}
