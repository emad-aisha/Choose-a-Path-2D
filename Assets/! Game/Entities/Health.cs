using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour {
    [Header("Basic Stats")]
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] int maxHealth;
    int currentHealth;

    [Header("Hurt Visualizer")]
    [SerializeField] float hurtTime = 0.1f;
    [SerializeField] Color hurtColor = Color.darkRed;

    [Header("Player I-Frames")]
    [SerializeField] float IFrames;
    [SerializeField] int flashTimes;
    [SerializeField] Color[] iFramesColor = new Color[2] { Color.gray2, Color.gray5 };

    bool canBeHurt = true;
    bool isDead = false;

    public delegate void DieEvent();
    public event DieEvent Die; // TODO: do seperate die logic in own scripts
    public event DieEvent Fling;

    void Start() {
        if (TryGetComponent(out SpriteRenderer spriteRenderer)) sprite = spriteRenderer;
        currentHealth = maxHealth;
    }

    void OnDisable() {
        Die = null;
        Fling = null;
    }

    public void Hurt(int damage) {
        if (!canBeHurt || isDead) return;
        currentHealth -= damage;

        Debug.Log(name + " Hurt");
        StartCoroutine(FlashRed());

        MovementManager.instance.StartKnockback();
        StartCoroutine(StartIFrame());
        Fling?.Invoke();

        if (currentHealth <= 0) {
            currentHealth = 0;
            Die?.Invoke(); // TODO: make a death sequence thingy for all enemies
            isDead = true;
            if (gameObject.CompareTag("Enemy")) {
                Destroy(gameObject);
            }
        }
    }

    public void Heal(int damage) {
        Debug.Log(name + " Heal");
        currentHealth += damage;

        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
    }

    public void SetMaxHealth(int value) { maxHealth = value; }

    // TIMERS
    IEnumerator StartIFrame() {
        if (IFrames <= 0) yield break;
        canBeHurt = false;

        float time = 0;
        while (time < IFrames) {
            yield return new WaitForSeconds(IFrames / (flashTimes * 0.5f));
            SetColor(iFramesColor[0]);
            yield return new WaitForSeconds(IFrames / (flashTimes * 0.5f));
            SetColor(iFramesColor[1]);

            time += IFrames / flashTimes;
        }

        SetColor(Color.white);
        canBeHurt = true;
    }

    IEnumerator FlashRed() {
        Color ogColor = sprite.color;
        SetColor(hurtColor);
        yield return new WaitForSeconds(hurtTime);
        if (!gameObject.CompareTag("Player")) SetColor(ogColor);
    }

    void SetColor(Color color) { if (sprite) sprite.color = color; }


}
