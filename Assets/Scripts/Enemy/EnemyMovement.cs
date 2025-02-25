using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private EnemyScriptableObject enemyData; // ScriptableObject containing enemy stats
    [SerializeField] private Animator animator; // Animator to control enemy animations

    private Transform player;
    private bool isKnockedBack = false; // Flag to track knockback state
    private Vector2 knockbackDirection; // Direction of knockback
    private float knockbackDuration = 0.5f; // Knockback duration
    private float knockbackTimeLeft; // Remaining knockback time

    private void Start()
    {
        // Find the player's transform (assuming only one HeroKnight instance exists in the scene)
        HeroKnight heroKnight = FindObjectOfType<HeroKnight>();
        if (heroKnight != null)
        {
            player = heroKnight.transform;
        }
        else
        {
            Debug.LogError("No HeroKnight found in the scene.");
        }
    }

    private void Update()
    {
        if (player == null) return; // Exit if no player found

        // Handle knockback
        if (isKnockedBack)
        {
            knockbackTimeLeft -= Time.deltaTime;
            transform.position += (Vector3)knockbackDirection * enemyData.moveSpeed * Time.deltaTime;

            if (knockbackTimeLeft <= 0)
            {
                isKnockedBack = false;
            }
            return; // Skip other actions during knockback
        }
        
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        Debug.Log($"Current Move Speed: {enemyData.moveSpeed}");

        // Move towards the player
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, enemyData.moveSpeed * Time.deltaTime);

        // Flip the enemy to face the player
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // Update animator's speed parameter
        if (animator != null)
        {
            animator.SetFloat("Speed", enemyData.moveSpeed);
        }
    }

    // Apply knockback from external sources
    public void ApplyKnockback(Vector2 direction, float duration)
    {
        knockbackDirection = direction;
        knockbackDuration = duration;
        knockbackTimeLeft = duration;
        isKnockedBack = true;
    }

    // Optionally set the player target dynamically
    public void SetPlayer(GameObject playerObject)
    {
        player = playerObject.transform;
    }
}
