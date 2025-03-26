using System.Collections;
using UnityEngine;

public class ImpalerAttackArea : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    [SerializeField] private float currentDamage;
    private bool isPlayerInRange = false;
    private ImpalerStats bossStats;
    public Vector3 attackOffset;
    private HeroHealth playerHealth;
    private HeroKnight playerKnight;

    void Awake()
    {
        currentDamage = enemyData.Damage;
        bossStats = GetComponentInParent<ImpalerStats>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            isPlayerInRange = true;
            playerHealth = col.GetComponent<HeroHealth>();
            playerKnight = col.GetComponent<HeroKnight>();
            if (playerHealth != null && playerKnight != null)
            {
                StartCoroutine(CastAttack());
            }
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    private IEnumerator CastAttack()
    {
        bossStats.TriggerAttackAnimation();
        yield return new WaitForSeconds(0.3f); // Wait for attack animation to complete

        if (isPlayerInRange && playerHealth != null && playerKnight != null)
        {
            PerformDamage(currentDamage);
        }
    }

    public void PerformDamage(float damageAmount)
    {
        float radius = 10f; // Adjust based on attack range
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D col = Physics2D.OverlapCircle(
            pos,
            radius,
            LayerMask.GetMask("Player")
        );

        if (col != null && isPlayerInRange)
        {
            HeroHealth hero = col.GetComponent<HeroHealth>();
            HeroKnight heroKnight = col.GetComponent<HeroKnight>();
            if (hero != null && heroKnight != null)
            {
                hero.TakeDamage((int)damageAmount);
                heroKnight.Stun();
            }
        }
    }

    // Debug to visualize the OverlapCircle
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }
}
