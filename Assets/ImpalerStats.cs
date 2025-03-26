using System.Collections;
using UnityEngine;

public class ImpalerStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    private ImpalerMovement impalerMovement;
    [SerializeField] private float damageCooldown = 2f; 
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
    private bool isAttacking = false; // New state for attacking

    void Start()
    {
        impalerMovement = GetComponent<ImpalerMovement>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        killCounter = GameObject.Find("KCO").GetComponent<KillCounter>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        GameObject backgroundMusicObject = GameObject.Find("BackgroundMusicManager");
        if (backgroundMusicObject != null)
        {
            BackgroundMusicController musicController = backgroundMusicObject.GetComponent<BackgroundMusicController>();
            if (musicController != null)
            {
                AudioClip newMusicClip = Resources.Load<AudioClip>("Music/Impaler_Theme");
                if (newMusicClip != null)
                {
                    musicController.ChangeMusic(newMusicClip);
                }
                else
                {
                    Debug.LogWarning("New music clip not found in Resources/Music/Impaler_Theme!");
                }
            }
            else
            {
                Debug.LogWarning("BackgroundMusicController component not found on BackgroundMusic object!");
            }
        }
        else
        {
            Debug.LogWarning("BackgroundMusic object not found in the scene!");
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

    public void TriggerAttackAnimation()
    {
        if (isDead || isAttacking) return;

        isAttacking = true; // Set attacking state
        impalerMovement.StopMovement(); // Stop movement before attacking

        int attackID = GetWeightedAttackID();
        animator.SetInteger("AttackID", attackID);

        StartCoroutine(ResetAttack());
    }

    private int GetWeightedAttackID()
    {
        int roll = Random.Range(1, 101); // Roll between 1 and 100
        if (roll <= 10) return 1; // 10% chance for attack 1
        if (roll <= 28) return 2; // 18% chance for attack 2
        if (roll <= 46) return 3; // 18% chance for attack 3
        if (roll <= 64) return 4; // 18% chance for attack 4
        if (roll <= 82) return 5; // 18% chance for attack 5
        return 6; // 18% chance for attack 6
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(1.5f); // Wait for attack duration
        animator.SetInteger("AttackID", 0);
        isAttacking = false; // Reset attacking state
        impalerMovement.ResumeMovement(); // Resume movement after attack
    }

    public void Die()
    {    
        SoundEffectManager.Instance.PlaySoundEffect(Resources.Load<AudioClip>("SoundEffects/BossDeath"), 1f);
        if (isDead) return;
        isDead = true;
        impalerMovement.StopMovement();
        currentMoveSpeed = 0;
        animator.SetTrigger("Death");
        float deathAnimationTime = animator.GetCurrentAnimatorStateInfo(0).length;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true;
        }
        DropKey(keyPrefab);

        killCounter.AddKill();
        Destroy(gameObject, deathAnimationTime);
    }

    private void DropKey(GameObject prefab)
    {
        if (prefab != null)
        {
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

    public void DealDamage()
    {
        Transform attackArea = transform.Find("AttackArea"); // Find child object
        if (attackArea != null)
        {
            ImpalerAttackArea attackScript = attackArea.GetComponent<ImpalerAttackArea>();
            if (attackScript != null)
            {
                attackScript.PerformDamage(currentDamage); // Call method in ImpalerAttackArea
            }
        }
        else
        {
            Debug.LogWarning("AttackArea not found on Impaler!");
        }
    }
}
