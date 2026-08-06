using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Checkpoint : MonoBehaviour {
    [SerializeField] GameObject visualizer;
    [SerializeField] bool isDebugging;

    void Start() {
        GetComponent<BoxCollider2D>().isTrigger = true;
        gameObject.tag = "Trigger";


        if (visualizer) {
            visualizer.tag = "Trigger";
            visualizer.SetActive(isDebugging);
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (CheckpointManager.instance.GetCheckpointTransform() != transform.position && collision.CompareTag("Player")) {
            Debug.Log("checkpoint gt");
            CheckpointManager.instance.SetActiveCheckpoint(this);
        }
    }


}
