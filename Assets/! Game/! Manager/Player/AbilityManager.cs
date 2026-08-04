using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class AbilityManager : MonoBehaviour {
    public static AbilityManager instance;

    [Header("Grapple Stats")]
    [SerializeField] Grapple grapple;


    void Awake() {
        if (instance == null) instance = this;
        if (grapple) grapple.StartGrapple += StartGrapple;
    }

    void OnDisable() {
        if (grapple) grapple.StartGrapple -= StartGrapple;
    }

    // SETTERS
    public void SetGrapple(Grapple _grapple, Grapple.Direction direction, float distance, float speed, float windup) {
        grapple = _grapple;
        SetGrappleDirection(direction);
        SetGrappleDistance(distance);
        SetGrappleSpeed(speed);
        SetGrapleWindup(windup);
    }
    public void SetGrappleDirection(Grapple.Direction direction) { grapple.direction = direction; }
    public void SetGrappleDistance(float distance) { grapple.distance = distance; }
    public void SetGrappleSpeed(float speed) { grapple.speed = speed; }
    public void SetGrapleWindup(float windup) { grapple.windup = windup; }

    void StartGrapple() { StartCoroutine(Grapple()); }

    public int allowedTimesInPlace;
    IEnumerator Grapple() {
        if (grapple.direction == global::Grapple.Direction.None) yield break;

        Vector2 direction = grapple.direction switch {
            global::Grapple.Direction.Side => PlayerManager.instance.GetTransform().right,
            global::Grapple.Direction.Up => PlayerManager.instance.GetTransform().up,
            _ => Vector2.zero
        };

        PlayerManager.instance.StopPlayerGravity();
        PlayerManager.instance.SetPlayerCanMove(false);
        Debug.DrawRay(transform.position, direction * grapple.distance, Color.green, 0.5f);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, grapple.distance, ~grapple.ignoreLayers);

        // TODO: this should be enough time for the grapple animation


        yield return new WaitForSeconds(grapple.windup);
        // stop player

        if (hit.collider == null) {
            // otherwise fall
        }
        else {
            // if it hit something, go there
            int timesInPlace = 0;
            Vector2 playerPosition = transform.position;
            Vector2 lastPlayerPosition = Vector2.zero;
            while ((grapple.direction == global::Grapple.Direction.Side && math.distance(playerPosition, hit.point) > (PlayerManager.instance.GetPlayerWidth() / 2)) ||
                    (grapple.direction == global::Grapple.Direction.Up && math.distance(playerPosition, hit.point) > (PlayerManager.instance.GetPlayerHeight() / 2))) {

                PlayerManager.instance.GetRigidbody().linearVelocity = direction * grapple.speed;
                playerPosition = transform.position; // update player pos

                if (lastPlayerPosition == playerPosition) timesInPlace++;
                if (timesInPlace > allowedTimesInPlace) { Debug.Log("escape"); break; }

                lastPlayerPosition = playerPosition;
                yield return new WaitForFixedUpdate();
            }
            Debug.Log("fin");
        }

        // unstop player
        PlayerManager.instance.SetPlayerCanMove(true);
        PlayerManager.instance.StartPlayerGravity();
    }


}
