using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private int damage = 20;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the collider has an IHealth component
        IHealth health = collider.GetComponent<IHealth>();
        if (health != null)
        {
            // Call Damage method on any object that implements IHealth
            health.TakeDamage(damage);
        }
    }
}
