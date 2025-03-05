using System.Collections;
using UnityEngine;

public class BODStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    private BODMovement bodMovement;
    [SerializeField] private float damageCooldown = 2f;  // Set your cooldown duration
    public GameObject hero;
    public GameObject damageText;
    private Color originalColor;
    [SerializeField] private float currentMoveSpeed;
    [SerializeField] private float currentHealth;
    [SerializeField] private float currentDamage;
    [SerializeField] private float damageEffectDuration = 0.2f;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private KillCounter killCounter;
    [SerializeField] public GameObject keyPrefab;
    private int dropCount = 0;
    private Animator animator;
    private bool isDead = false;
    private bool canDamage = true;  // This flag ensures damage is only applied after cooldown

    void Start()
    {
        bodMovement = GetComponent<BODMovement>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        killCounter = GameObject.Find("KCO").GetComponent<KillCounter>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    void Awake()
    {
        currentMoveSpeed = enemyData.MoveSpeed;
        currentDamage = enemyData.Damage;
        currentHealth = enemyData.MaxHealth;
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        StartCoroutine(ShowDamageEffect());
        if (currentHealth <= 0)
        {
            Die();
        }
        RectTransform textTransform = Instantiate(damageText).GetComponent<RectTransform>();
        textTransform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        Canvas canvas = GameObject.FindObjectOfType<Canvas>();
        textTransform.SetParent(canvas.transform);
        PopUpDamage popup = textTransform.GetComponent<PopUpDamage>();
        if (popup != null)
        {
            popup.textMesh.text = dmg.ToString();
        }
    }

    private IEnumerator ShowDamageEffect()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(damageEffectDuration);
        spriteRenderer.color = originalColor;
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && canDamage)
        {
            animator.SetTrigger("Attack");
            // Damage the player
            HeroHealth hero = col.GetComponent<HeroHealth>();
            if (hero != null)
            {
                hero.TakeDamage((int)currentDamage);
            }
            
            // Start cooldown
            StartCoroutine(DamageCooldown());
        }
    }

    private IEnumerator DamageCooldown()
    {
        canDamage = false;  // Disable damage during cooldown
        yield return new WaitForSeconds(damageCooldown);  // Wait for the cooldown duration
        canDamage = true;  // Re-enable damage after cooldown
    }

    public void Die()
    {
        if (isDead) return; // Prevent multiple executions
        isDead = true;
        bodMovement.StopMovement();
        animator.SetTrigger("Death");
        float deathAnimationTime = animator.GetCurrentAnimatorStateInfo(0).length;

        DropKey(keyPrefab);
        
        killCounter.AddKill();
        Destroy(gameObject, deathAnimationTime);
    }

    private void DropKey(GameObject prefab)
    {
        if (prefab != null)
        {
            // Calculate spawn position with an offset to avoid overlap
            float offsetX = 0.5f * (dropCount % 3) - 0.5f;
            float offsetY = -0.5f * (dropCount / 3); 

            Vector3 spawnPosition = transform.position + new Vector3(offsetX, offsetY, 0);

            Instantiate(prefab, spawnPosition, Quaternion.identity);

            dropCount++;
        }
        else
        {
            Debug.LogError("Prefab not assigned in the Inspector.");
        }
    }
}
