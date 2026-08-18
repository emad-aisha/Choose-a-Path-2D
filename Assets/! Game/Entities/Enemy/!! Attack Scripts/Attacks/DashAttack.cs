using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class DashAttack : BasicAttack {
    [Header("Dash Stats")]
    [SerializeField] float speed;
    [SerializeField] float dashTime;

    Vector3 playerPosition;
    Rigidbody2D rigidBody;

    void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
    }


    override protected IEnumerator AttackLogic() {
        Debug.Log("Dash");
        playerPosition = PlayerManager.instance.GetTransform().position;

        // attack logic
        rigidBody.linearVelocity = (playerPosition - stopPosition).normalized * speed;
        yield return new WaitForSeconds(dashTime);
        rigidBody.linearVelocity = Vector2.zero;

    }

}
