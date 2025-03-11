using UnityEngine;
using System.Collections;

public class DeathSpell : MonoBehaviour
{
    [SerializeField] private int damageAmount = 20;
    [SerializeField] private float delayBeforeDamage = 1f;
    [SerializeField] private float animationDuration = 1f;

    private bool isPlayerInRange = false; 
    private Animator animator;

    private void Start()
    {
        
        SoundEffectManager.Instance.PlaySoundEffect(Resources.Load<AudioClip>("SoundEffects/SummonSpell"), 1f);
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            isPlayerInRange = true;
            HeroHealth playerHealth = col.GetComponent<HeroHealth>();

            if (playerHealth != null)
            {
                StartCoroutine(CastSpell(playerHealth));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            isPlayerInRange = false; 
        }
    }

    private IEnumerator CastSpell(HeroHealth playerHealth)
    {
        yield return new WaitForSeconds(delayBeforeDamage);

        if (isPlayerInRange)
        {
            playerHealth.TakeDamage(damageAmount); 
        }

        yield return new WaitForSeconds(animationDuration);

        Destroy(gameObject);
    }
}
