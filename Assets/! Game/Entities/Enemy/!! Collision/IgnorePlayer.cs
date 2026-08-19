using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class IgnorePlayer : MonoBehaviour {

    void Start() {
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), PlayerManager.instance.GetCollider());
    }

}
