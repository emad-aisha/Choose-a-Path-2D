using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour {
    public static RespawnManager instance;
    List<Water> water = new();

    [SerializeField] float respawnWaitTime;

    // EVENT SETUP
    void Awake() {
        if (instance == null) instance = this;
        PlayerManager.instance.GetHealth().Die += StartRespawn;
    }

    void OnDisable() {
        PlayerManager.instance.GetHealth().Die -= StartRespawn;
        for (int i = 0; i < water.Count; i++) {
            water[i].Respawn -= StartRespawn;
        }
    }


    // EVENTS
    void StartRespawn() {
        Debug.Log("Respawn");
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn() {
        float time = 0;
        while (time < respawnWaitTime) {
            time += Time.deltaTime;
            MovementManager.instance.SetPlayerCanMove(false);
            MovementManager.instance.StopPlayerGravity();

            yield return new WaitForSeconds(Time.deltaTime);
        }

        PlayerManager.instance.GetTransform().position = CheckpointManager.instance.GetCheckpointTransform();
        MovementManager.instance.SetPlayerCanMove(true);
        MovementManager.instance.StartPlayerGravity();
    }

    // Setters
    public void AddWaterEvent(Water _water) {
        _water.Respawn += StartRespawn;
        water.Add(_water);
    }

}
