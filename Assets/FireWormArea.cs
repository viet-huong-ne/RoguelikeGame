using System.Collections;
using UnityEngine;

public class FireWormArea : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    [SerializeField] private float currentDamage;
    private bool canDamage = true;  // Flag to manage damage cooldown

    private FireWormStats fireWormStats;

    void Awake()
    {
        currentDamage = enemyData.Damage;
        fireWormStats = GetComponentInParent<FireWormStats>();  // Get reference to BODStats
    }

    protected virtual void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player") && canDamage)
        {
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
        yield return new WaitForSeconds(2f);  // Adjust the cooldown duration as needed
        canDamage = true;  // Re-enable damage after cooldown
    }
}
