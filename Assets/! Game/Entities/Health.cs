using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour {
    [SerializeField] float stunTime = 0.1f;

    [Header("Basic Stats")]
    [SerializeField] int maxHealth;
    [SerializeField] float IFrames;
    int currentHealth;
    bool canBeHurt = true;
    static bool isTimePaused = false;

    public delegate void DieEvent();
    public event DieEvent Die; // TODO: do seperate die logic in own scripts

    void OnDisable() { Die = null; }

    public void Hurt(int damage) {
        if (!canBeHurt) return;
        currentHealth -= damage;
        if (!isTimePaused) StartCoroutine(StunTime());
        StartCoroutine(StartIFrame());

        if (currentHealth < 0) {
            currentHealth = 0;
            Die?.Invoke();
        }
    }

    public void Heal(int damage) {
        currentHealth += damage;

        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
    }

    // GETTERS
    public bool IsTimePaused() { return isTimePaused; }


    // TIMERS
    IEnumerator StartIFrame() {
        canBeHurt = false;
        yield return new WaitForSeconds(IFrames);
        canBeHurt = true;
    }

    IEnumerator StunTime() {
        isTimePaused = true;
        float originalTimeScale = Time.timeScale;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(stunTime);
        Time.timeScale = originalTimeScale;
        isTimePaused = false;
    }


}
