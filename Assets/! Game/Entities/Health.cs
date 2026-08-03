using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour {
    [Header("Basic Stats")]
    [SerializeField] int maxHealth;
    [SerializeField] float IFrames;
    int currentHealth;
    bool canBeHurt = true;

    public delegate void DieEvent();
    public event DieEvent Die;

    void OnDisable() { Die = null; }

    public void Hurt(int damage) {
        if (!canBeHurt) return;
        currentHealth -= damage;
        StartCoroutine(StartIFrame());

        if (currentHealth < 0) {
            currentHealth = 0;
            Die?.Invoke();

            if (!CompareTag("Player")) Destroy(gameObject);
        }
    }

    public void Heal(int damage) {
        currentHealth += damage;

        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
    }

    IEnumerator StartIFrame() {
        canBeHurt = false;
        yield return new WaitForSeconds(IFrames);
        canBeHurt = true;
    }


}
