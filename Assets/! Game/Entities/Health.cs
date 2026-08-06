using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour {
    [Header("Basic Stats")]
    [SerializeField] int maxHealth;
    [SerializeField] float IFrames;
    int currentHealth;

    [SerializeField] float stunTime = 0.1f;

    [Header("visualizer")]
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] int flashTimes;
    [SerializeField] Color deathColor = Color.darkRed;
    [SerializeField] Color iFramesColor1 = Color.gray2;
    [SerializeField] Color iFramesColor2 = Color.gray5;

    bool canBeHurt = true;
    bool isDead = false;
    static bool isTimePaused = false;

    public delegate void DieEvent();
    public event DieEvent Die; // TODO: do seperate die logic in own scripts
    public event DieEvent Fling; // TODO: do seperate die logic in own scripts

    void Start() { currentHealth = maxHealth; }

    void OnDisable() {
        Die = null;
        Fling = null;
    }

    public void Hurt(int damage) {
        if (!canBeHurt || isDead) return;
        currentHealth -= damage;

        Debug.Log(name + " Hurt");
        if (!isTimePaused) StartCoroutine(StunTime());
        StartCoroutine(StartIFrame());
        Fling?.Invoke();

        if (currentHealth < 0) {
            currentHealth = 0;
            Die?.Invoke();
            isDead = true;
        }
    }

    public void Heal(int damage) {
        Debug.Log(name + " Heal");
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

        float time = 0;
        while (time < IFrames) {
            SetColor(iFramesColor1);
            yield return new WaitForSeconds(IFrames / (flashTimes * 0.5f));
            SetColor(iFramesColor2);
            yield return new WaitForSeconds(IFrames / (flashTimes * 0.5f));

            time += IFrames / flashTimes;
        }

        SetColor(Color.white);
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

    void SetColor(Color color) { if (sprite) sprite.color = color; }


}
