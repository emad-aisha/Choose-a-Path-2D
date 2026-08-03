using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [SerializeField] float followPercent;
    [SerializeField] float speed;
    [SerializeField] float z = -9;

    Vector2 position;

    void Update() {
        position = Vector3.Lerp(transform.position, PlayerManager.instance.GetTransform().position, followPercent);
    }

    void FixedUpdate() {
        transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * speed);
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }



}
