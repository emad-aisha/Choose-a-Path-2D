using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D)), RequireComponent(typeof(SpriteRenderer))]
public class IdleWalk : MonoBehaviour {
    [Header("Basic Stats")]
    [SerializeField] float speed;
    [SerializeField] float idleTime;
    bool isIdle = false;

    [Header("Stop")]
    [SerializeField] Trigger floorObject; // checks if there is somewthing where it is going


    void Start() {
        GetComponent<BoxCollider2D>().isTrigger = true;
        tag = "Enemy";

        floorObject.TriggerExit += FlipEnemy;
    }

    void OnDisable() {
        floorObject.TriggerExit -= FlipEnemy;
    }

    void Update() {
        if (!isIdle) transform.position += transform.right * (speed * Time.deltaTime);
    }

    void FlipEnemy() {
        StartCoroutine(IdleTimer());
    }

    IEnumerator IdleTimer() {
        isIdle = true;
        yield return new WaitForSeconds(idleTime);
        isIdle = false;
        transform.Rotate(0, 180, 0);
    }

}
