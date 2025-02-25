using System;
using System.Collections;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    [SerializeField] private float damageCooldown = 1f;
    public GameObject hero;
    public GameObject damageText;
    private Color originalColor;
    [SerializeField] private float currentMoveSpeed;
    [SerializeField] private float currentHealth;
    [SerializeField] private float currentDamage;
    [SerializeField] private float damageEffectDuration = 0.2f;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private KillCounter killCounter;
    [SerializeField] private GameObject experiencePrefab;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject sackOfGoldPrefab;
    [SerializeField] public GameObject keyPrefab;
    [SerializeField, Range(0, 100)] private float coinDropRate = 20f;
    [SerializeField, Range(0, 100)] private float sackOfGoldDropRate = 5f;
    [SerializeField, Range(0, 100)] private float experienceDropRate = 70f;
    private int dropCount = 0;
    private Animator animator;
    private bool isDead = false;

    void Start()
    {
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

    private bool canDamage = true;

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            HeroHealth hero = col.GetComponent<HeroHealth>();
            hero.TakeDamage((int)currentDamage);
        }
    }

    public void TryDealDamage(GameObject target)
    {
        if (canDamage)
        {
            StartCoroutine(DealDamageCoroutine(target));
        }
    }

    private IEnumerator DealDamageCoroutine(GameObject target)
    {
        HeroHealth heroHealth = target.GetComponent<HeroHealth>();
        if (heroHealth != null)
        {
            heroHealth.TakeDamage(((int)enemyData.Damage));
        }
        canDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canDamage = true;
    }

    public void Die()
    {
        if (isDead) return; // Prevent multiple executions
        isDead = true;
        currentMoveSpeed = 0;
        
        animator.SetTrigger("Death");
        float deathAnimationTime = animator.GetCurrentAnimatorStateInfo(0).length;

        if(keyPrefab != null){
            DropItem(keyPrefab);
        } 
        else 
        {
            // Tính toán item nào sẽ được drop
            GameObject itemToDrop = GetRandomDropItem();

            if (itemToDrop != null)
            {
                // Drop một item duy nhất
                DropItem(itemToDrop); // 100% vì đã chọn item để drop
            }
        }
        
        killCounter.AddKill();
        Destroy(gameObject, deathAnimationTime);
    }

    private GameObject GetRandomDropItem()
    {
        float randomValue = UnityEngine.Random.Range(0f, 100f);

        // Xác định thứ tự ưu tiên drop item
        if (randomValue <= experienceDropRate)
        {
            return experiencePrefab;
        }
        else if (randomValue <= experienceDropRate + coinDropRate)
        {
            return coinPrefab;
        }
        else if (randomValue <= experienceDropRate + coinDropRate + sackOfGoldDropRate)
        {
            return sackOfGoldPrefab;
        }

        return null; // Không drop gì nếu không khớp
    }

    private void DropItem(GameObject prefab)
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
