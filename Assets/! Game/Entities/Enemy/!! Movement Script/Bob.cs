using UnityEngine;


public class Bob : MonoBehaviour {
    [Header("Bob Stats")]
    [SerializeField, Range(0.5f, 2)] float speed;
    [SerializeField, Range(0.4f, 2)] float height;
    float bobValue;

    bool isUp = true;

    void FixedUpdate() {
        if (isUp) {
            bobValue += Time.deltaTime;
            if (bobValue > height) isUp = false;
        }
        else {
            bobValue -= Time.deltaTime;
            if (bobValue < -height) isUp = true;
        }

        transform.position += new Vector3(0, bobValue * speed * Time.deltaTime, 0);
    }

}
