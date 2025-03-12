using UnityEngine;

public class PortionItem : MonoBehaviour
{
    [SerializeField] private int healAmount = 50;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SoundEffectManager.Instance.PlaySoundEffect(Resources.Load<AudioClip>("SoundEffects/Heal"), 1f);
            HeroHealth heroHealth = other.GetComponent<HeroHealth>();
            if (heroHealth != null)
            {
                heroHealth.Heal(healAmount);
                Debug.Log($"Player healed by {healAmount} points.");
            }

            Destroy(gameObject);
        }
    }
}
