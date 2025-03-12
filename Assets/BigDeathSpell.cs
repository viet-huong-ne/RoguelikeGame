using System.Collections;
using UnityEngine;

public class BigDeathSpell : MonoBehaviour
{
    [SerializeField] private int damageAmount = 150;
    [SerializeField] private float delayBeforeDamage = 2f;
    [SerializeField] private float animationDuration = 1f;

    private Cinemachine.CinemachineImpulseSource impulseSource;

    private void Start()
    {
        SoundEffectManager.Instance.PlaySoundEffect(Resources.Load<AudioClip>("SoundEffects/Kirin"), 1f);
        // Tìm hoặc gắn impulse source
        impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
        if (impulseSource == null)
        {
            Debug.LogError("CinemachineImpulseSource không được tìm thấy trên GameObject!");
        }
    }

    private IEnumerator CastSpell(HeroHealth playerHealth)
    {
        yield return new WaitForSeconds(delayBeforeDamage);

        // Gọi rung màn hình
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse();
        }

        // Gây sát thương
        playerHealth.TakeDamage(damageAmount);

        yield return new WaitForSeconds(animationDuration);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            HeroHealth playerHealth = col.GetComponent<HeroHealth>();
            if (playerHealth != null)
            {
                StartCoroutine(CastSpell(playerHealth));
            }
        }
    }
}
