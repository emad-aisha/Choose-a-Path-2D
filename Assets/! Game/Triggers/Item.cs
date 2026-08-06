using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour {
    [SerializeField] List<Door> doorsToClose = new();
    [SerializeField] GameObject reward;

    void Start() {
        gameObject.tag = "Trigger";
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            CloseDoors();

            Debug.Log("Item Got Sequence");
            GameObject physicalReward = Instantiate(reward, AbilityManager.instance.gameObject.transform);
            GiveReward(physicalReward);

            Destroy(gameObject);
        }
    }

    void CloseDoors() {
        for (int i = 0; i < doorsToClose.Count; i++) {
            doorsToClose[i].SetOpen(false);
        }
    }

    void GiveReward(GameObject physicalReward) {
        if (reward.TryGetComponent(out Grapple grapple)) {
            AbilityManager.instance.SetGrapple(grapple);
        }
        else if (reward.TryGetComponent(out JumpBoost jumpBoost)) {
            AbilityManager.instance.SetJumpBoost(jumpBoost);
        }
        else if (reward.TryGetComponent(out Sprint sprint)) {
            AbilityManager.instance.SetSprint(sprint);
        }
        else if (reward.TryGetComponent(out Attack attack)) {
            AbilityManager.instance.SetAttack(physicalReward);
        }
        else {
            Debug.Log("Reward is not a player ability");
        }
    }


}
