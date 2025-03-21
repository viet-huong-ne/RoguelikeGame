using System.Collections;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    [SerializeField] private float currentDamage;
    private bool canDamage = true;  // Flag to manage damage cooldown
    void Awake()
    {
        currentDamage = enemyData.Damage;
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            HeroHealth hero = col.GetComponent<HeroHealth>();
            hero.TakeDamage((int)currentDamage);
        }
        // Start cooldown
        StartCoroutine(DamageCooldown());
    }

    private IEnumerator DamageCooldown()
    {
        canDamage = false;  // Disable damage during cooldown
        yield return new WaitForSeconds(1f);  // Adjust the cooldown duration as needed
        canDamage = true;  // Re-enable damage after cooldown
    }
}
