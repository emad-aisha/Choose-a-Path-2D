using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class AbilityManager : MonoBehaviour {
    public static AbilityManager instance;

    [Header("Attack Stats")]
    [SerializeField] GameObject attack;
    [SerializeField] HurtBox hurtBox;

    [Header("Grapple Stats")]
    [SerializeField] Grapple grapple;
    [SerializeField] int maxInPlaceTimes;

    [Header("Sprint Stats")]
    [SerializeField] Sprint sprint;
    bool stopSprinting = false;

    [Header("Jump Stats")]
    [SerializeField] JumpBoost jumpBoost;


    void Awake() { if (instance == null) instance = this; }
    void OnDisable() {
        if (grapple) grapple.StartGrapple -= StartGrapple;
        if (sprint) sprint.StartSprint -= StartSprint; // dont forget to set the event
        if (sprint) sprint.EndSprint -= EndSprint;
        if (hurtBox) hurtBox.Hit -= MovementManager.instance.StartKnockback;
    }


    // ATTACK ----
    public void SetAttack(GameObject _attack) {
        attack = _attack;
        hurtBox = _attack.GetComponentInChildren<HurtBox>(true);
        hurtBox.Hit += MovementManager.instance.StartKnockback;
        hurtBox.gameObject.SetActive(false);
    }
    public Attack GetAttack() {
        if (!attack) return null;
        return attack.GetComponent<Attack>();
    }


    // GRAPPLE ----
    public void SetGrapple(Grapple _grapple) {
        grapple = _grapple;
        grapple.StartGrapple += StartGrapple;
    }

    void StartGrapple() { StartCoroutine(Grapple()); }
    IEnumerator Grapple() {
        if (grapple.direction == global::Grapple.Direction.None) yield break;

        Vector2 direction = grapple.direction switch {
            global::Grapple.Direction.Side => PlayerManager.instance.GetTransform().right,
            global::Grapple.Direction.Up => PlayerManager.instance.GetTransform().up,
            _ => Vector2.zero
        };

        MovementManager.instance.StopPlayerGravity();
        MovementManager.instance.SetPlayerCanMove(false);
        Debug.DrawRay(transform.position, direction * grapple.distance, Color.green, 0.5f);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, grapple.distance, ~grapple.ignoreLayers);

        // TODO: this should be enough time for the grapple animation
        yield return new WaitForSeconds(grapple.windup);

        if (hit.collider == null) { }
        else {
            // if it hit something, go there
            int timesInPlace = 0;
            Vector2 playerPosition = transform.position;
            Vector2 lastPlayerPosition = Vector2.zero;
            while ((grapple.direction == global::Grapple.Direction.Side && math.distance(playerPosition, hit.point) > (PlayerManager.instance.GetPlayerWidth() / 2)) ||
                    (grapple.direction == global::Grapple.Direction.Up && math.distance(playerPosition, hit.point) > (PlayerManager.instance.GetPlayerHeight() / 2))) {

                MovementManager.instance.GetRigidbody().linearVelocity = direction * grapple.speed;
                playerPosition = transform.position; // update player pos

                if (lastPlayerPosition == playerPosition) timesInPlace++;
                if (timesInPlace > maxInPlaceTimes) { break; }

                lastPlayerPosition = playerPosition;
                yield return new WaitForFixedUpdate();
            }
        }

        // unstop player
        MovementManager.instance.SetPlayerCanMove(true);
        MovementManager.instance.StartPlayerGravity();
    }


    // SPRINT ----
    public void SetSprint(Sprint _sprint) {
        sprint = _sprint;
        sprint.StartSprint += StartSprint; // dont forget to set the event
        sprint.EndSprint += EndSprint; // dont forget to set the event
    }

    void StartSprint() { StartCoroutine(Sprint()); }
    IEnumerator Sprint() {
        stopSprinting = false;
        float sprintMod = sprint.GetSprintMod();

        float speedUp = 1;
        float time = 0;
        while (speedUp < sprintMod) {
            yield return new WaitForSeconds(Time.deltaTime / sprint.GetTimeToSpeed());
            time += Time.deltaTime / sprint.GetTimeToSpeed();

            speedUp = sprintMod * Mathf.Lerp(0, 1, time);
            MovementManager.instance.GetRigidbody().linearVelocityX *= speedUp;
            if (stopSprinting) { yield break; }
        }

        // set final sprint values
        MovementManager.instance.SetPlayerSprint(true, sprintMod);
    }
    void EndSprint() {
        stopSprinting = true;
        MovementManager.instance.SetPlayerSprint(false, 1);
    }


    // JUMP BOOST ----
    public void SetJumpBoost(JumpBoost _jumpBoost) {
        jumpBoost = _jumpBoost;
        MovementManager.instance.SetJumpValue(jumpBoost.GetJumpValue());
    }


    // HELPER
    public IEnumerator FlipPlayerCoroutine() {
        int safety = 0;
        while (hurtBox != null && hurtBox.gameObject.activeSelf && safety < 200) {
            safety++;
            yield return new WaitForEndOfFrame();
        }
        PlayerManager.instance.GetTransform().Rotate(0, 180, 0);
    }

}
