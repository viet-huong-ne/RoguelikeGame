using UnityEngine;

public class BODAttack : MonoBehaviour
{
    [SerializeField] private EnemyScriptableObject enemyData;  // Reference to the EnemyScriptableObject

    // This method is called when another collider enters the trigger collider attached to this object
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered the attack area is the player
        if (other.CompareTag("Player"))
        {
            // Get the HeroHealth component from the player
            HeroHealth playerHealth = other.GetComponent<HeroHealth>();

            // If player has HeroHealth script, apply damage from the EnemyScriptableObject
            if (playerHealth != null && enemyData != null)
            {
                playerHealth.TakeDamage((int)enemyData.Damage);  // Apply damage from the scriptable object
            }
        }
    }
}
